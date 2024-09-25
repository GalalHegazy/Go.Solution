using Go.APIs.Errors;
using Go.APIs.Helpers;
using Go.APIs.MiddleWare;
using Go.Core.Repositories.Contract;
using Go.Infrastructure;
using Go.Infrastructure.Basket_Repository;
using Go.Infrastructure._Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Go.Infrastructure._IdentityData;
using Go.Infrastructure._IdentityData.DataSeed;
using Microsoft.AspNetCore.Identity;
using Go.Core.Entities.Identity;
using Go.Core.Services.Contract;
using Go.Application.AuthServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Go.Core;
using Go.Application.OrderServices;
using Go.Application.ProductServices;
using Go.Application.PaymentServices;
using Go.Application.CacheServices;

namespace Go.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services 

            // Add services to the container.
            builder.Services.AddControllers();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Allow DI For StoreDbContext , Get Connection String For DB 
            builder.Services.AddDbContext<StoreDbContext>(options =>
                            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulteConnection"))
                          );

            // Allow DI For AppIdentityDbContext , Get Connection String For DB 
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
                         );

            // Allow DI For Redis , Create Connection String For DB Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));  // Connection String
            });

            //(services.AddAuthentication();)
            // Allow DI For Register Securtiy (Main Services(User Manger,SignInManger,RoleManger) and Manor Services and Defult Configration)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>(); // Add DI For stores

            // To Change Defult Scheme To Bearer ,And Add Authentication Handeler To Scheme Bearer For Token  
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Change To  Defult Scheme Bearer 
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Make Defult Scheme Bearer with out Write name of Scheme [Authorize]

            }).AddJwtBearer(option => // Add Authentication Handeler
                     {
                         option.TokenValidationParameters = new TokenValidationParameters()
                         {
                             // Clims
                             ValidateAudience = true,
                             ValidAudience = builder.Configuration["JWT:ValidAudience"],
                             ValidateIssuer = true,
                             ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                             ValidateLifetime = true,
                             ClockSkew = TimeSpan.Zero,

                             // Security Key
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"]))
                         };
                     });

            // ReFactor For Genration InvaildModelStatueResponse (Vaildtion Error)
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(E => E.Value.Errors)
                                                         .Select(E => E.ErrorMessage).ToList();

                    var response = new APIValidationErrorResponce()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            // Allow DI For Mainor Services Required for Cors
            builder.Services.AddCors(options =>
            {
                // Add Plloicy
                options.AddPolicy("MyPloicy", PloicyOptions =>
                {
                    PloicyOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
                });
            });



            // Allow DI For All Controllers from (IGenericRepository) XX  RepositoryMethod In Class UnitOfWork It Will Create Obj from IGenericRepository<> Not CLR
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Allow DI For (IUnitOfWork) 
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            // Allow DI For IMapper 
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Allow DI For (IAuthService)
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

            // Allow DI For (IOrderService)
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            // Allow DI For (IProductService)
            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));

            // Allow DI For (IPaymentService)
            builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            // Allow DI For (IBasketRepository) (Redis)
            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            // Allow DI For (ICacheService) (Redis)
            builder.Services.AddSingleton(typeof(ICacheService), typeof(CacheService));



            #endregion

            var app = builder.Build();

            #region Updata DataBase 
            using var Scoped = app.Services.CreateScope();
            var Services = Scoped.ServiceProvider;
            var _storeDbContext = Services.GetRequiredService<StoreDbContext>();
            var _appIdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _storeDbContext.Database.MigrateAsync(); // To Update DataBase For (StoreDbContext)
                await StoreDbContextSeed.SeedAsync(_storeDbContext); // To Add DataSeed For (Product Data,Order Data)

                await _appIdentityDbContext.Database.MigrateAsync(); // To Update DataBase For (AppIdentityDbContext)
                var userManger = Services.GetRequiredService<UserManager<ApplicationUser>>();//Create Obj From userManger Explicitly
                await ApplicationDbContextSeed.SeedUsersAsync(userManger); // To Add DataSeed
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Has Been Occured During Apply The Migration");
            }
            #endregion

            #region Configure Kestrel Middlewares 

            // Use MiddleWare For Handel ExceptionServerError
            app.UseMiddleware<ExceptionMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use MiddleWare To Create Handel NotFound EndPoint Like (https://localhost:7145/)
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Allow to my EndPonit to Consume in anthore origin (Domine like :Project angluer) 
            app.UseCors("MyPloicy");  

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}

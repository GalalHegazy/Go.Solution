using AdminDashBoard.MappingProfiles;
using Go.Core;
using Go.Core.Entities.Identity;
using Go.Infrastructure;
using Go.Infrastructure._Data;
using Go.Infrastructure._IdentityData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminDashBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services 
            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // Allow DI For StoreDbContext , Get Connection String For DB 
            builder.Services.AddDbContext<StoreDbContext>(options =>
                            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulteConnection"))
                          );

            // Allow DI For AppIdentityDbContext , Get Connection String For DB 
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
                         );


            //(services.AddAuthentication();)
            // Allow DI For Register Securtiy (Main Services(User Manger,SignInManger,RoleManger) and Manor Services and Defult Configration)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>(); // Add DI For stores


            // Allow DI For IMapper 
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Allow DI For (IUnitOfWork) 
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));



            #endregion

            var app = builder.Build();

            #region Configure Kestrel Middlewares 

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                       pattern: "{controller=Admin}/{action=LogIn}/{id?}"

                       );   
            });
            #endregion

            app.Run();
        }
    }
}
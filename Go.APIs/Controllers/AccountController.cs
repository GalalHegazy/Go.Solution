using AutoMapper;
using Go.APIs.Dtos.AccountDtos;
using Go.APIs.Errors;
using Go.APIs.Extenstions;
using Go.Core.Entities.Identity;
using Go.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Security.Claims;

namespace Go.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager
                                ,SignInManager<ApplicationUser> signInManager
                                ,IAuthService authService
                                ,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost("SignIn")]
        public async Task<ActionResult<UserDto>> LogIn(SignInDto signInDto)
        {
            var user = await _userManager.FindByEmailAsync(signInDto.Email);

            if (user is null) return Unauthorized(new APIResponce(401, "Incorrect Email Or Password!!"));

           var flag = await _signInManager.CheckPasswordSignInAsync(user,signInDto.Password,false);

            if(!flag.Succeeded) return Unauthorized(new APIResponce(404, "Incorrect Email Or Password!!"));

            return Ok(new UserDto()
            {
                Email = signInDto.Email,
                Name = signInDto.Email.Split("@")[0],
                Token =  await _authService.CreateTokenAsync(user)    
            });
        }
        [HttpPost("SignUp")]
        public async Task<ActionResult<UserDto>> SignUp(SignUpDto signUpDto)
        {
        
            var newUser = new ApplicationUser()
            {
                UserName = signUpDto.Email.Split("@")[0],
                DisplayName = signUpDto.Name,
                Email = signUpDto.Email,
                PhoneNumber = signUpDto.Phone
            };

            var result = await _userManager.CreateAsync(newUser, signUpDto.Password);
            
            if (!result.Succeeded) 
                return BadRequest(new APIValidationErrorResponce() { Errors = result.Errors.Select(E => E.Description) });
          
            return Ok(new UserDto()
            {
                Name= signUpDto.Name,
                Email= signUpDto.Email,
                Token = await _authService.CreateTokenAsync(newUser)
            });
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // Get User By Privete Claims (Email)
            var email = User.FindFirstValue(ClaimTypes.Email);
            
            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                Name = user.UserName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user) // مؤقتا
            });
        }
        [HttpGet("GetAddress")]
        [Authorize]
        public  async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }
       [HttpPut("UpdateAddress")]
       [Authorize]
        public async Task<ActionResult<Address>> UpdateUserAddress(AddressDto addressDto)
        {
            var MappedAddress = _mapper.Map<AddressDto,Address>(addressDto);  // Get Mapped From AddressDto To Address (Retuen Address)

            var user = await _userManager.FindUserWithAddressAsync(User); //Get Useraddress By Claims  (Retuen User)
                         // Has No Value    //Has Value
            MappedAddress.ApplicationUserId = user.Id; // To Set ApplicationUserId To Make Tracking

            user.Address = MappedAddress; // To Set Updated Address

            var result = await _userManager.UpdateAsync(user); // Updateed Address

            if(!result.Succeeded) return BadRequest(new APIValidationErrorResponce() {Errors = result.Errors.Select(e=>e.Description) });

            return Ok(addressDto);  
        }



    }
}

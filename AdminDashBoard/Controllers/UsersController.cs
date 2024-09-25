using AdminDashBoard.Extensions;
using AdminDashBoard.ViewModels.Users;
using AutoMapper;
using Go.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashBoard.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<ApplicationUser> userManager
                              ,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchInputRole)
        {
            if (string.IsNullOrEmpty(SearchInputRole))
            {
                var Users =  _userManager.Users.Select(
                 U => new UserViewModel()
                 {
                     Id = U.Id,
                     UserName = U.UserName,
                     Email = U.Email,
                     PhoneNumber = U.PhoneNumber,
                     Roles = _userManager.GetRolesAsync(U).Result
                 }).ToList();
                return View(Users);
            }
            else
            {
                var Users = await  _userManager.FindByEmailAsync(SearchInputRole);
                var MappedUser = new UserViewModel()
                {
                    Id = Users.Id,
                    UserName = Users.UserName,
                    Email = Users.Email,
                    PhoneNumber = Users.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(Users).Result
                };

                return PartialView("PartialViews/UserTablePartial", new List<UserViewModel> { MappedUser });
            }
        }
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var MappedUser = _mapper.Map<UserViewModel>(user);

            return View(viewName, MappedUser);

        }

        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, nameof(Update));
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel userVM, [FromRoute] string id)
        {
            if (id != userVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    user.UserName = userVM.UserName;
                    user.PhoneNumber = userVM.PhoneNumber;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userVM);
        }
        public Task<IActionResult> Delete(string Id)
        {
            return Details(Id, nameof(Delete));
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string email)
        {
            try
            {
                var User = await _userManager.FindByEmailAsync(email);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

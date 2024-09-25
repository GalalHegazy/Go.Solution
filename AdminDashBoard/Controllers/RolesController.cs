using AdminDashBoard.ViewModels.Roles;
using AutoMapper;
using Go.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminDashBoard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RolesController(RoleManager<IdentityRole> roleManager
                              , UserManager<ApplicationUser> userManager
                              , IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchInputRole)
        {
            if (string.IsNullOrEmpty(SearchInputRole))
            {
                var Roles = await _roleManager.Roles.ToListAsync();
                var MappedRoles = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(Roles);
                return View(MappedRoles);
            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(SearchInputRole);
                var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
                return View(new List<RoleViewModel>() { MappedRole });
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                var user = await _roleManager.CreateAsync(MappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var Role = await _roleManager.FindByIdAsync(id);

            if (Role is null)
                return NotFound();

            var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);

            return View(viewName, MappedRole);

        }
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, nameof(Update));
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleViewModel roleVM, [FromRoute] string id)
        {
            if (id != roleVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = roleVM.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }
        public Task<IActionResult> Delete(string Id)
        {
            return Details(Id, nameof(Delete));
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string name)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(name);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
                return NotFound();

            ViewBag.RoleId = roleId;

            var usersInRole = new List<UserInRoleViewModel>();
            var userInRole = new UserInRoleViewModel();
            var Users = await _userManager.Users.ToListAsync();


            foreach (var user in Users)
            {
                userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewModel> usersInRoleVM)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is not null)
            {
                foreach (var user in usersInRoleVM)
                {

                    var userId = await _userManager.FindByIdAsync(user.UserId);

                    if (user.IsSelected && !(await _userManager.IsInRoleAsync(userId, role.Name)))
                        await _userManager.AddToRoleAsync(userId, role.Name);
                    else if (!user.IsSelected && await _userManager.IsInRoleAsync(userId, role.Name))
                        await _userManager.RemoveFromRoleAsync(userId, role.Name);
                }

                return RedirectToAction("Update", new { id = roleId });
            }
            else
            {
               NotFound();
            }

            return View(usersInRoleVM);
        }
    }
}

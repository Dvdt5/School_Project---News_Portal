using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt.Extensions;
using School_Project___News_Portal.Models;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace School_Project___News_Portal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(IMapper mapper, INotyfService notyf, IConfiguration config, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _mapper = mapper;
            _notyf = notyf;
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userModels = _mapper.Map<List<UserModel>>(users);
            return View(userModels);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(UserModel model)
        {

            var user = new AppUser();
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhotoUrl = "no-img.png";
            var identityResult = await _userManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    _notyf.Error(item.Description);
                }
                return View(model);
            }

            var user1 = await _userManager.FindByNameAsync(model.UserName);
            var roleExist = await _roleManager.RoleExistsAsync("Member");
            if (!roleExist)
            {
                var role = new AppRole { Name = "Member" };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user1, "Member");
            _notyf.Success("User added");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);

            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserModel model)
        {
    
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;

            await _userManager.UpdateAsync(user);
            _notyf.Success("User Updated");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);

            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (await _userManager.IsInRoleAsync(user ,"Admin")) 
            {
                _notyf.Error("You dont have permission to delete admin");
                return RedirectToAction("Index");
            }

            await _userManager.DeleteAsync(user);
            _notyf.Success("User Successfuly Deleted");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserRole(string id)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var user = await _userManager.FindByIdAsync(id);
            var userRoleModels = new List<UserRoleModel>();
            foreach (var role in roles)
            {
                var userRoleModel = new UserRoleModel();
                userRoleModel.UserId = id;
                userRoleModel.RoleId = role.Id;
                userRoleModel.RoleName = role.Name;
                userRoleModel.IsInRole = await _userManager.IsInRoleAsync(user, role.Name);
                userRoleModels.Add(userRoleModel);
            }
            return View(userRoleModels);
        }

        public async Task<IActionResult> UserRoleAdd(string id, string userId)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction("UserRole", new { id = userId });
        }

        public async Task<IActionResult> UserRoleDelete(string id, string userId)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRoleAsync(user, role.Name);

            return RedirectToAction("UserRole", new { id = userId });
        }
    }
}

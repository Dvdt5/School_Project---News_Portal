using AspNetCoreHero.ToastNotification.Notyf.Models;
using Microsoft.AspNetCore.Mvc;
using School_Project___News_Portal.Models;
using System.Diagnostics;
using NETCore.Encrypt.Extensions;
using School_Project___News_Portal.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using School_Project___News_Portal.Repositories;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity;

namespace School_Project___News_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyf;
        private readonly IFileProvider _fileProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, INotyfService notyf, IFileProvider fileProvider, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _config = config;
            _notyf = notyf;
            _fileProvider = fileProvider;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
               
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                _notyf.Error("This username does not exists");
                return View(model);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.KeepMe, true);

            if (signInResult.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("PhotoUrl", user.PhotoUrl));
                await _userManager.AddClaimAsync(user, new Claim("FullName", user.FullName));
                return RedirectToAction("Index", "Admin");
            }

            if (signInResult.IsLockedOut)
            {
                _notyf.Error("User Access locked until  " + user.LockoutEnd + ".");

                return View(model);
            }
            _notyf.Error("Incorrect Password or UserName Entered :" + await _userManager.GetAccessFailedCountAsync(user) + "/3");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var rootFolder = _fileProvider.GetDirectoryContents("wwwroot");
            var photoUrl = "no-img.png";
            if (model.PhotoFile != null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(model.PhotoFile.FileName);
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "UserPhotos").PhysicalPath, filename);
                using var stream = new FileStream(photoPath, FileMode.Create);
                model.PhotoFile.CopyTo(stream);
                photoUrl = filename;
            }

            var identityResult = await _userManager.CreateAsync(new() { UserName = model.UserName, Email = model.Email, FullName = model.FullName, PhotoUrl = photoUrl }, model.Password);

            if (!identityResult.Succeeded) 
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    _notyf.Error(item.Description);
                }
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            var roleExist = await _roleManager.RoleExistsAsync("Member");
            if (!roleExist)
            {
                var role = new AppRole { Name = "Member" };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, "Member");          

            _notyf.Success("Successfly registerd User.");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var claimsToRemove = await _userManager.GetClaimsAsync(user);
                foreach (var claim in claimsToRemove)
                {

                    await _userManager.RemoveClaimAsync(user, claim);
                }
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

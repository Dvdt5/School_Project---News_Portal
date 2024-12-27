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
using AutoMapper;

namespace School_Project___News_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsItemRepository _newsItemRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyf;
        private readonly IFileProvider _fileProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, INotyfService notyf, IFileProvider fileProvider, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, NewsItemRepository newsItemRepository, CategoryRepository categoryRepository, IMapper mapper)
        {
            _logger = logger;
            _config = config;
            _notyf = notyf;
            _fileProvider = fileProvider;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _newsItemRepository = newsItemRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var newsItems = await _newsItemRepository.GetAllAsync();
            var newsItemModels = _mapper.Map<List<NewsItemModel>>(newsItems);
            ViewBag.NewsItems = newsItemModels;

            var categories = await _categoryRepository.GetAllAsync();
            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);
            ViewBag.Categories = categoryModels;

            return View(ViewBag);
        }

        public async Task<IActionResult> NewsItem(int id)
        {
            var newsItem = await _newsItemRepository.GetByIdAsync(id);
            var newsItemModel = _mapper.Map<NewsItemModel>(newsItem);
            ViewBag.NewsItem = newsItemModel;

            var category = await _categoryRepository.GetByIdAsync(newsItem.CategoryId);
            var categoryModel = _mapper.Map<CategoryModel>(category);
            ViewBag.Category = categoryModel;

            return View(ViewBag);
        }

        public async Task<IActionResult> Category(int id)
        {
            var newsItems = await _newsItemRepository.GetAllAsync();
            var newsItemModels = _mapper.Map<List<NewsItemModel>>(newsItems);
            ViewBag.NewsItems = newsItemModels.Where(x => x.CategoryId == id);

            var categories = await _categoryRepository.GetAllAsync();
            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);
            ViewBag.Categories = categoryModels;
            ViewBag.Category = categoryModels.FirstOrDefault(x => x.Id == id);

            return View(ViewBag);
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);
            ViewBag.Categories = categoryModels;

            return View(ViewBag);
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
                var photoPath = Path.Combine(rootFolder.First(x => x.Name == "userPhotos").PhysicalPath, filename);
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

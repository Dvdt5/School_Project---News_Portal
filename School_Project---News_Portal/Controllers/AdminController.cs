using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NETCore.Encrypt.Extensions;
using Microsoft.AspNetCore.Identity;
using School_Project___News_Portal.Models;

namespace School_Project___News_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly NewsItemRepository _newsItemRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IConfiguration _config;
        private readonly IFileProvider _fileProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;


        public AdminController(IMapper mapper, INotyfService notyf, IConfiguration config, IFileProvider fileProvider, NewsItemRepository newsItemRepository, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _mapper = mapper;
            _notyf = notyf;
            _config = config;
            _fileProvider = fileProvider;
            _newsItemRepository = newsItemRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index()
        {
            var newsItems = await _newsItemRepository.GetAllAsync();
            var newsItemModels = _mapper.Map<List<NewsItemModel>>(newsItems);
            return View(newsItemModels);
        }

        public async Task<IActionResult> Profile()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.Users.Where(s => s.UserName == userName).FirstOrDefaultAsync();
            var userModel = _mapper.Map<RegisterModel>(user);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(RegisterModel model)
        {
            var userName = User.Identity.Name;
            var user = await _userManager.Users.Where(s => s.UserName == userName).FirstOrDefaultAsync();

            
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;

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
            user.PhotoUrl = photoUrl;

            await _userManager.UpdateAsync(user);
            _notyf.Success("User Data Updated");
            return RedirectToAction("Index", "Admin");
        }


    }
}

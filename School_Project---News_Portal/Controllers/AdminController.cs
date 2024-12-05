using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NETCore.Encrypt.Extensions;

namespace School_Project___News_Portal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IConfiguration _config;
        private readonly IFileProvider _fileProvider;

        public AdminController(UserRepository userRepository, IMapper mapper, INotyfService notyf, IConfiguration config, IFileProvider fileProvider)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _notyf = notyf;
            _config = config;
            _fileProvider = fileProvider;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var userName = User.Claims.First(c => c.Type == "UserName").Value;
            var user = await _userRepository.Where(s => s.UserName == userName).FirstOrDefaultAsync();
            var userModel = _mapper.Map<RegisterModel>(user);
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(RegisterModel model)
        {
            var userName = User.Claims.First(c => c.Type == "UserName").Value;
            var user = await _userRepository.Where(s => s.UserName == userName).FirstOrDefaultAsync();
            
            if (_userRepository.Where(s => s.UserName == model.UserName && s.Id != user.Id).Count() > 0)
            {
                _notyf.Error("Username Already Taken!");
                return View(model);
            }
            if (_userRepository.Where(s => s.Email == model.Email && s.Id != user.Id).Count() > 0)
            {
                _notyf.Error("Email already taken!");
                return View(model);
            }
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Updated = DateTime.Now;
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
            user.Password = MD5Hash(model.Password);
            await _userRepository.UpdateAsync(user);
            _notyf.Success("User Data Updated");
            return RedirectToAction("Index", "Admin");
        }

        public string MD5Hash(string pass)
        {
            var salt = _config.GetValue<string>("AppSettings:MD5Salt");
            var password = pass + salt;
            var hashed = password.MD5();
            return hashed;
        }
    }
}

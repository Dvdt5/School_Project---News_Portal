using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using School_Project___News_Portal.Models;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;

namespace School_Project___News_Portal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly IConfiguration _config;

        public UserController(UserRepository userRepository, IMapper mapper, INotyfService notyf, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _notyf = notyf;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
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
           
            if (_userRepository.Where(s => s.UserName == model.UserName).Count() > 0)
            {
                _notyf.Error("UserName is already taken!");
                return View(model);
            }
            if (_userRepository.Where(s => s.Email == model.Email).Count() > 0)
            {
                _notyf.Error("Email is already taken!");
                return View(model);
            }
            
            var user = new User();
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Created = DateTime.Now;
            user.Updated = DateTime.Now;
            user.PhotoUrl = "no-img.png";
            user.Password = MD5Hash(model.Password);
            user.Role = model.Role;
            await _userRepository.AddAsync(user);
            _notyf.Success("User added");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id) 
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);

            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);

            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Role = model.Role;
            user.Updated = DateTime.Now;

            await _userRepository.UpdateAsync(user);
            _notyf.Success("User Updated");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);

            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserModel model)
        {
            await _userRepository.DeleteAsync(model.Id);
            _notyf.Success("User Successfuly Deleted");
            return RedirectToAction("Index");
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

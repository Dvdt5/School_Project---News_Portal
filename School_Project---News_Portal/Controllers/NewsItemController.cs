using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.Extensions.FileProviders;
using School_Project___News_Portal.Models;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace School_Project___News_Portal.Controllers
{
    [Authorize]
    public class NewsItemController : Controller
    {
        private readonly NewsItemRepository _newsItemRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notfy;
        private readonly IFileProvider _fileProvider;

        public NewsItemController(NewsItemRepository newsItemRepository, IMapper mapper, INotyfService notfy, CategoryRepository categoryRepository, IFileProvider fileProvider)
        {
            _newsItemRepository = newsItemRepository;
            _mapper = mapper;
            _notfy = notfy;
            _categoryRepository = categoryRepository;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var newsItems = await _newsItemRepository.GetAllAsync();
            var newsItemModels = _mapper.Map<List<NewsItemModel>>(newsItems);
            return View(newsItemModels);
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoriesSelectList = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.Categories = categoriesSelectList; 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewsItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newsItem = _mapper.Map<NewsItem>(model);

            await _newsItemRepository.AddAsync(newsItem);
            _notfy.Success("News Item Successfuly Added");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var newsItem = await _newsItemRepository.GetByIdAsync(id);
            var newsItemModel = _mapper.Map<NewsItemModel>(newsItem);

            var categories = await _categoryRepository.GetAllAsync();
            var categoriesSelectList = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.Categories = categoriesSelectList;

            return View(newsItemModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(NewsItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newsItem = _mapper.Map<NewsItem>(model);
            newsItem.PhotoUrl = "www";
            newsItem.Updated = DateTime.Now;
            await _newsItemRepository.UpdateAsync(newsItem);
            _notfy.Success("News Succesfuly Updated");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var newsItem = await _newsItemRepository.GetByIdAsync(id);
            var newsItemModel = _mapper.Map<NewsItemModel>(newsItem);

            return View(newsItemModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(NewsItemModel model)
        {
            await _newsItemRepository.DeleteAsync(model.Id);
            _notfy.Success("News Item Successfuly Deleted");
            return RedirectToAction("Index");
        }
    }
}

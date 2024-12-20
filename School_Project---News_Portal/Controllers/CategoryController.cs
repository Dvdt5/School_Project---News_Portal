﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using School_Project___News_Portal.Repositories;
using School_Project___News_Portal.ViewModels;
using School_Project___News_Portal.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace School_Project___News_Portal.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly INotyfService _notfy;

        public CategoryController(CategoryRepository categoryRepository, IMapper mapper, INotyfService notfy)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _notfy = notfy;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryModels = _mapper.Map<List<CategoryModel>>(categories);

            return View(categoryModels);
        }

        public IActionResult Add() 
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var category = _mapper.Map<Category>(model);

            category.Created = DateTime.Now;
            category.Updated = DateTime.Now;

            await _categoryRepository.AddAsync(category);
            _notfy.Success("Successfuly Created Category");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            var categoryModel = _mapper.Map<CategoryModel>(category);

            return View(categoryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var category = await _categoryRepository.GetByIdAsync(model.Id);

            category.Name = model.Name;
            category.IsActive = model.IsActive;
            category.Updated = DateTime.Now;
            await _categoryRepository.UpdateAsync(category);
            _notfy.Success("Successfuly Updated Category");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            var categoryModel = _mapper.Map<CategoryModel>(category);

            return View(categoryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryModel model)
        {
            await _categoryRepository.DeleteAsync(model.Id);
            _notfy.Success("Successfuly Deleted Category");
            return RedirectToAction("Index");
        }
    }
}

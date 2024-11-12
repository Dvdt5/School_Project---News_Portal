﻿using AutoMapper;
using School_Project___News_Portal.Models;
using School_Project___News_Portal.ViewModels;

namespace School_Project___News_Portal.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
        }
    }
}
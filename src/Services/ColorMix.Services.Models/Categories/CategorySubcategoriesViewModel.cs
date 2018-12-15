﻿using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;
using AutoMapper;

namespace ColorMix.Services.Models.Categories
{
    public class CategorySubcategoriesViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<SubCategoryViewModel> SubCategories { get; set; }
        
    }
}

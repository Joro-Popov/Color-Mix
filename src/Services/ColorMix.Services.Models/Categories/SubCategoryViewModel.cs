using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System;

namespace ColorMix.Services.Models.Categories
{
    public class SubCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ProductsCount { get; set; }
    }
}

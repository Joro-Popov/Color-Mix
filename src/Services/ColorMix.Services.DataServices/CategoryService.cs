using System;
using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Categories;
using System.Collections.Generic;
using System.Linq;

namespace ColorMix.Services.DataServices
{
    public class CategoryService : ICategoryService
    {
        public ColorMixContext DbContext { get; }

        public CategoryService(ColorMixContext dbContext)
        {
            DbContext = dbContext;
        }
        public IEnumerable<CategoryViewModel> GetAllCategories()
        {
            var categories = DbContext.Categories.To<CategoryViewModel>().ToList();

            return categories;
        }

        public bool CheckIfCategoryExists(Guid id)
        {
            return this.DbContext.Categories.Any(c => c.Id == id);
        }

        public string GetCategoryName(Guid id)
        {
            return this.DbContext.Categories.FirstOrDefault(c => c.Id == id)?.Name;
        }

        public IEnumerable<CategorySubcategoriesViewModel> GetAllCategoriesAndSubCategories()
        {
            //var categorySubcategories = this.DbContext.Categories
            //    .To<CategorySubcategoriesViewModel>()
            //    .ToList();

            // TODO: Map with Automapper

            var categorySubcategories = this.DbContext.Categories
                .Select(c => new CategorySubcategoriesViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    SubCategories = c.SubCategories
                        .Where(sc => sc.CategoryId == c.Id)
                        .Select(sbc => new SubCategoryViewModel()
                        {
                            Name = sbc.Name,
                            Id = sbc.Id,
                            ProductsCount = sbc.Products.Count
                        }).ToList()
                });

            return categorySubcategories;
        }
    }
}

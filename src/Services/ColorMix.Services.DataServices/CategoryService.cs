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

        public bool CheckIfCategoryExists(Guid categoryId)
        {
            return this.DbContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CheckIfSubCategoryExists(Guid? subCategoryId)
        {
            return subCategoryId == null || this.DbContext.SubCategories.Any(x => x.Id == subCategoryId);
        }

        public string GetCategoryName(Guid id)
        {
            return this.DbContext.Categories.FirstOrDefault(c => c.Id == id)?.Name;
        }

        public IEnumerable<SideMenuViewModel> GetAllCategoriesAndSubCategories()
        {
            var categorySubcategories = this.DbContext.Categories
                .To<SideMenuViewModel>()
                .ToList();
            
            return categorySubcategories;
        }
    }
}

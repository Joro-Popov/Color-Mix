using System;
using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Categories;
using System.Collections.Generic;
using System.Linq;
using ColorMix.Data.Models;

namespace ColorMix.Services.DataServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ColorMixContext dbContext;

        public CategoryService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<CategoryViewModel> GetAllCategories()
        {
            var categories = this.dbContext.Categories.To<CategoryViewModel>().ToList();

            return categories;
        }

        public bool CheckIfCategoryExists(Guid categoryId)
        {
            return this.dbContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CheckIfSubCategoryExists(Guid? subCategoryId)
        {
            return subCategoryId == null || this.dbContext.SubCategories.Any(x => x.Id == subCategoryId);
        }

        public string GetCategoryName(Guid id)
        {
            return this.dbContext.Categories.FirstOrDefault(c => c.Id == id)?.Name;
        }

        public Guid GetCategoryId(string categoryName, string subCategoryName = null)
        {
            if (subCategoryName != null)
            {
                return this.dbContext.Categories
                    .FirstOrDefault(x => x.Name == categoryName)
                    .SubCategories.FirstOrDefault(s => s.Name == subCategoryName).Id;
            }

            return this.dbContext.Categories
                .FirstOrDefault(x => x.Name == categoryName).Id;
        }

        public IEnumerable<SideMenuViewModel> GetAllCategoriesAndSubCategories()
        {
            var categorySubcategories = this.dbContext.Categories
                .To<SideMenuViewModel>()
                .ToList();
            
            return categorySubcategories;
        }

        public IEnumerable<string> GetSubCategoryNames(string categoryName)
        {
            var subCategoryNames = this.dbContext.Categories
                .FirstOrDefault(c => c.Name == categoryName)
                .SubCategories
                .Select(x => x.Name)
                .ToList();

            return subCategoryNames;
        }

        public void CreateCategory(CreateCategoryViewModel model)
        {
            var category = new Category()
            {
                Name = model.CategoryName
            };

            if (model.SubCAtegoryNames != null)
            {
                var subCategories = model.SubCAtegoryNames
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => new SubCategory()
                    {
                        Name = x.First().ToString().ToUpper() + x.Substring(1),
                        Category = category
                    }).ToList();

                category.SubCategories = subCategories;
            }

            this.dbContext.Categories.Add(category);
            this.dbContext.SaveChanges();
        }
    }
}

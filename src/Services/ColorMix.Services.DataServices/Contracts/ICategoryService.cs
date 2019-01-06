using System;
using ColorMix.Services.Models.Categories;
using System.Collections.Generic;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAllCategories();

        bool CheckIfCategoryExists(Guid categoryId);

        bool CheckIfSubCategoryExists(Guid? subCategoryId);

        string GetCategoryName(Guid id);

        Guid GetCategoryId(string categoryName, string subCategoryName = null);

        IEnumerable<SideMenuViewModel> GetAllCategoriesAndSubCategories();

        IEnumerable<string> GetSubCategoryNames(string categoryName);

        void CreateCategory(CreateCategoryViewModel model);

        void CreateSubCategory(CreateCategoryViewModel model);
    }
}

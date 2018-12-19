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

        IEnumerable<SideMenuViewModel> GetAllCategoriesAndSubCategories();
    }
}

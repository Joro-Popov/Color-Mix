using ColorMix.Services.Models.Categories;
using System.Collections.Generic;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAllCategories();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Services.Models.Products;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IProductService
    {
        IEnumerable<ProductsViewModel> GetProductsByCategory(Guid id);

        IEnumerable<ProductsViewModel> GetProductsBySubCategory(Guid categoryId, Guid subCategoryId);

        DetailsViewModel GetProductDetails(Guid id);

        bool CheckIfProductExists(Guid id);
    }
}

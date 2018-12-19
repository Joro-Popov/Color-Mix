using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Services.Models.Products;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IProductService
    {
        IEnumerable<ProductsViewModel> GetProductsByCategory(Guid categoryId, Guid? subCategoryId = null);

        DetailsViewModel GetProductDetails(Guid id);

        bool CheckIfProductExists(Guid id);
    }
}

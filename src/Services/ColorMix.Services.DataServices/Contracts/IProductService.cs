using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Services.Models.Products;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IProductService
    {
        AllProductsViewModel GetProductsByCategory(Guid categoryId, int? page , Guid? subCategoryId = null);

        DetailsViewModel GetProductDetails(Guid id);

        bool CheckIfProductExists(Guid id);
    }
}

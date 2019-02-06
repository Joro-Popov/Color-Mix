using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Data.Models;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Products;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IProductService
    {
        AllProductsViewModel GetProductsByCategory(Guid categoryId, int? pageNumber , Guid? subCategoryId = null);

        DetailsViewModel GetProductDetails(Guid id);

        EditProductViewModel GetProduct(Guid id);

        Size GetProductSize(string size);

        IEnumerable<ProductViewModel> GetRandomProducts(int count);

        IEnumerable<UnavailableProductViewModel> GetAllUnavailableProducts();

        bool CheckIfProductExists(Guid id);
        
        void CreateProduct(CreateProductViewModel model);

        void ChangeProductInfo(EditProductViewModel model);

        void DeleteProduct(Guid id);

        void RestoreProduct(Guid id);
    }
}

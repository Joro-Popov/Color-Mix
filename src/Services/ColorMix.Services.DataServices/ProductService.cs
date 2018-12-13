using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Products;

namespace ColorMix.Services.DataServices
{
    public class ProductService : IProductService
    {
        public Task<IEnumerable<ProductsViewModel>> GetProductsByCategory(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

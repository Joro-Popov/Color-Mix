using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Services.Models.Products;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductsViewModel>> GetProductsByCategory(Guid id);
    }
}

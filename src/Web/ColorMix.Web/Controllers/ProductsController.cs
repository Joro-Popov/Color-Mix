using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;

namespace ColorMix.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [Authorize]
        public async Task<IActionResult> ProductsByCategory(Guid id)
        {
            //var products = await this.productService.GetProductsByCategory(id);

            return this.View();
        }
    }
}

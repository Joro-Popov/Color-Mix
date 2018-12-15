using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ColorMix.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private const string UNEXISTING_CATEGORY = "Възникна грешка!";

        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }

        [Authorize]
        public IActionResult ProductsByCategory(Guid id)
        {
            if (!categoryService.CheckIfCategoryExists(id))
            {
                return View("Error", new ErrorViewModel() { Message = UNEXISTING_CATEGORY });
            }

            var products = productService.GetProductsByCategory(id);

            this.ViewData["CategoryName"] = this.categoryService.GetCategoryName(id);

            return View(products);
        }

        public IActionResult ProductsBySubCategory(Guid categoryId, Guid subCategoryId)
        {

            if (!categoryService.CheckIfCategoryExists(categoryId))
            {
                return View("Error", new ErrorViewModel() { Message = UNEXISTING_CATEGORY });
            }

            var products = productService.GetProductsBySubCategory(categoryId, subCategoryId);

            this.ViewData["CategoryName"] = this.categoryService.GetCategoryName(categoryId);

            return this.View("ProductsByCategory", products);
        }
    }
}

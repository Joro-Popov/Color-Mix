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
        public IActionResult ProductsByCategory(Guid categoryId, Guid? subCategoryId)
        {
            if (!this.categoryService.CheckIfCategoryExists(categoryId) || 
                !this.categoryService.CheckIfSubCategoryExists(subCategoryId))
            {
                return View("Error", new ErrorViewModel() { Message = UNEXISTING_CATEGORY });
            }

            var products = productService.GetProductsByCategory(categoryId, subCategoryId);

            this.ViewData["CategoryName"] = this.categoryService.GetCategoryName(categoryId);

            return View(products);
        }

        [Authorize]
        public IActionResult Details(Guid id)
        {

            if (!this.productService.CheckIfProductExists(id))
            {
                return View("Error", new ErrorViewModel() { Message = UNEXISTING_CATEGORY });
            }

            var details = this.productService.GetProductDetails(id);

            return this.View(details);
        }
    }
}

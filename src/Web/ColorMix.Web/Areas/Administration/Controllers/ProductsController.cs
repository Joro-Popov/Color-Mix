using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : AdminController
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;

        public ProductsController(ICategoryService categoryService, IProductService productService)
        {
            this.categoryService = categoryService;
            this.productService = productService;
        }

        public IActionResult CreateProduct()
        {
            var categoryData = this.categoryService.GetAllCategoriesAndSubCategories();

            this.ViewData["Categories"] = categoryData.Select(x => x.Name).ToList();

            return this.View();
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductViewModel model)
        {
            if (!ModelState.IsValid) return this.View(model);
            
            this.productService.CreateProduct(model);

            var categoryId = this.categoryService.GetCategoryId(model.Category);

            return this.RedirectToAction("ProductsByCategory", "Products", new {categoryId = categoryId});
        }
        public IActionResult GetSubCategoryNames(string categoryName)
        {
            var subCategoryNames = this.categoryService.GetSubCategoryNames(categoryName);

            return Json(subCategoryNames);
        }
    }
}

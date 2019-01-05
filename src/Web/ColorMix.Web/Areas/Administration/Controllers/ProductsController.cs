using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

            return this.RedirectToAction("ProductsByCategory", "Products", new {categoryId});
        }

        public IActionResult GetSubCategoryNames(string categoryName)
        {
            var subCategoryNames = this.categoryService.GetSubCategoryNames(categoryName);

            return Json(subCategoryNames);
        }

        public IActionResult EditProduct(Guid id)
        {
            var product = this.productService.GetProduct(id);

            var categoryData = this.categoryService.GetAllCategoriesAndSubCategories();

            this.ViewData["Categories"] = categoryData.Select(x => x.Name).ToList();

            return this.View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(EditProductViewModel model)
        {
            if (!ModelState.IsValid) return this.View(model);

            this.productService.ChangeProductInfo(model);

            return this.RedirectToAction("Details", "Products", new {model.Id});
        }

        public IActionResult DeleteProduct(Guid id)
        {
            var categoryName = this.productService.GetProduct(id).Category;
            var categoryId = this.categoryService.GetCategoryId(categoryName);

            this.productService.DeleteProduct(id);
            
            return this.RedirectToAction("ProductsByCategory", "Products", new {categoryId});
        }
    }
}

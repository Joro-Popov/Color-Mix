using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ColorMix.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private const string ERROR = "Възникна грешка!";

        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }
        
        public IActionResult ProductsByCategory(Guid categoryId, int? page, Guid? subCategoryId)
        {
            if (!this.categoryService.CheckIfCategoryExists(categoryId) || 
                !this.categoryService.CheckIfSubCategoryExists(subCategoryId))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            var products = productService.GetProductsByCategory(categoryId, page, subCategoryId);
            
            this.ViewData["CategoryName"] = this.categoryService.GetCategoryName(categoryId);

            return View(products);
        }
        
        public IActionResult Details(Guid id)
        {
            if (!this.productService.CheckIfProductExists(id))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            var details = this.productService.GetProductDetails(id);
            
            return this.View(details);
        }
    }
}

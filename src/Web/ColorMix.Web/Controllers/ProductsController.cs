﻿using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ColorMix.Web.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.categoryService = categoryService;
        }
        
        public IActionResult ProductsByCategory(Guid categoryId, int? pageNumber, Guid? subCategoryId)
        {
            if (!this.categoryService.CheckIfCategoryExists(categoryId) || 
                !this.categoryService.CheckIfSubCategoryExists(subCategoryId))
            {
                return View("Error", new ErrorViewModel() { Message = ERROR });
            }

            var products = productService.GetProductsByCategory(categoryId, pageNumber, subCategoryId);
            
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

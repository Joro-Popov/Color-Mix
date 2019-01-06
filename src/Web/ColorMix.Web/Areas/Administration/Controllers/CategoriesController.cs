using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : AdminController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult CreateCategory()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid) return this.View(model);

            this.categoryService.CreateCategory(model);

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult CreateSubCategory()
        {
            var categoryData = this.categoryService.GetAllCategoriesAndSubCategories();

            this.ViewData["Categories"] = categoryData.Select(x => x.Name).ToList();

            return this.View();
        }

        [HttpPost]
        public IActionResult CreateSubCategory(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categoryData = this.categoryService.GetAllCategoriesAndSubCategories();

                this.ViewData["Categories"] = categoryData.Select(x => x.Name).ToList();

                return this.View(model);
            }
            
            this.categoryService.CreateSubCategory(model);

            return this.RedirectToAction("Index", "Home");
        }
    }
}

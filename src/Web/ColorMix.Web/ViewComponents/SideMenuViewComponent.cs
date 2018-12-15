using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.ViewComponents
{
    public class SideMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public SideMenuViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categorySubcategories = this.categoryService.GetAllCategoriesAndSubCategories();

            return this.View(categorySubcategories);
        }
    }
}

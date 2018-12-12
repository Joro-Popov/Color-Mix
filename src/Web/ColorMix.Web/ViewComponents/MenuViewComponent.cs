using ColorMix.Services.DataServices.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ColorMix.Web.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public MenuViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = categoryService.GetAllCategories();

            return View(categories);
        }
    }
}

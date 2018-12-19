using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Services.Models.Products
{
    public class AllProductsViewModel
    {
        public Guid? SubCategoryId { get; set; }

        public Guid CategoryId { get; set; }

        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}

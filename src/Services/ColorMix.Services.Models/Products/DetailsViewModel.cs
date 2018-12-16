using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Products
{
    public class DetailsViewModel : IMapFrom<Product>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string Size { get; set; }

        public string Material { get; set; }

        public bool IsAvailable { get; set; }

        public string Brand { get; set; }

        public ICollection<ProductsViewModel> RandomProducts { get; set; }
    }
}

using System;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Products
{
    public class ProductViewModel : IMapFrom<Product>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public Guid SubCategoryId { get; set; }

        public Guid CategoryId { get; set; }
    }
}

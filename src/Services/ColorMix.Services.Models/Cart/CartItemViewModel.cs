using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System;
using System.Collections.Generic;

namespace ColorMix.Services.Models.Cart
{
    public class CartItemViewModel : IMapFrom<Product>
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public ICollection<string> Sizes { get; set; }

        public decimal Total => Quantity * Price;
    }
}

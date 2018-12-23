using System;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ColorMix.Services.Models.Cart;

namespace ColorMix.Services.Models.Products
{
    public class DetailsViewModel : IMapFrom<Product>
    {
        public CartItemViewModel CartItem { get; set; }

        public string Description { get; set; }

        public string Material { get; set; }

        public string Color { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<ProductViewModel> RandomProducts { get; set; }
    }
}

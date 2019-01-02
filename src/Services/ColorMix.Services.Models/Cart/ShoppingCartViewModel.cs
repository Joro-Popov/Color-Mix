using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Cart
{
    public class ShoppingCartViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        [Range(1, 10)]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Изберете размер!")]
        public string Size { get; set; }

        public decimal Total => Quantity * Price;

    }
}

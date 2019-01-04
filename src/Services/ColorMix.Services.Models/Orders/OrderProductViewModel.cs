using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Orders
{
    public class OrderProductViewModel
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

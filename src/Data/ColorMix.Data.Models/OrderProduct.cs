using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class OrderProduct
    {
        public Guid OrderId { get; set; }
        
        public virtual Order Order { get; set; }

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; set; }

        // Not mapped?
        public decimal UnitTotalPrice { get; set; }
    }
}

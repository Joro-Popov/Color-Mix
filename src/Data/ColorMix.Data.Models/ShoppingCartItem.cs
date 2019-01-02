using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class ShoppingCartItem : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public Guid ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}

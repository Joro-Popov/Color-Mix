using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class ShoppingCartItem : BaseEntity
    {
        public virtual Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public virtual Guid ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}

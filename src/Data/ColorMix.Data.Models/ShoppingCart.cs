using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class ShoppingCart : BaseEntity
    {
        public string UserId { get; set; }

        public virtual ColorMixUser User { get; set; }

        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
    }
}

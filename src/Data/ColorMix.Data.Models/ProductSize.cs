using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class ProductSize
    {
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public Guid SizeId { get; set; }

        public virtual Size Size { get; set; }
    }
}

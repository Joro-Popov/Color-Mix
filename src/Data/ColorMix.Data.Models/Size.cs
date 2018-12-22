using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class Size : BaseEntity
    {
        public string Abbreviation { get; set; }

        public virtual ICollection<ProductSize> Products { get; set; } = new HashSet<ProductSize>();
    }
}

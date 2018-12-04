using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}

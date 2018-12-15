using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

        public virtual ICollection<SubCategory> SubCategories { get; set; } = new HashSet<SubCategory>();
        
    }
}

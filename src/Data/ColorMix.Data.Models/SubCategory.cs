using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<CategorySubcategories> CategorySubCategories { get; set; } = new HashSet<CategorySubcategories>();
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}

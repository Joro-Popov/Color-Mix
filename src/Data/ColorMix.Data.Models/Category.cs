using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<CategorySubcategories> CategorySubCategories { get; set; } = new HashSet<CategorySubcategories>();
    }
}

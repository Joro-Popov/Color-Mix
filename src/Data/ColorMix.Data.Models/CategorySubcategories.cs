using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class CategorySubcategories
    {
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public Guid SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}

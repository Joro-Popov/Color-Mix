using System;
using System.Collections.Generic;

namespace ColorMix.Data.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Material { get; set; }

        public string Brand { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public Guid SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
    }
}

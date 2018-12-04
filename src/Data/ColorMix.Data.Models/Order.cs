using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ColorMix.Data.Models.Enumerations;

namespace ColorMix.Data.Models
{
    public class Order : BaseEntity
    {
        public OrderStatus Status { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ColorMixUser User { get; set; }

        // Not mapped?
        public decimal OrderTotalPrice { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();

        public virtual Invoice Invoice { get; set; }
    }
}

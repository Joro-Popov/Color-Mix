using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public class Invoice : BaseEntity
    {
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

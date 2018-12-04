using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Data.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}

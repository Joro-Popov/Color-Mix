using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ColorMix.Data.Models
{
    public class Address : BaseEntity
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int ZipCode { get; set; }
        
        [Required]
        public string UserId { get; set; }

        public virtual ColorMixUser User { get; set; }
    }
}

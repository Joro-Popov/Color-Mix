using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ColorMix.Data.Models
{
    public class ColorMixUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual Address Address { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}

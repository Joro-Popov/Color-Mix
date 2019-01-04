using System;
using System.Collections.Generic;
using System.Text;

namespace ColorMix.Services.Models.Orders
{
    public class OrderAddressViewModel
    {
        public string Receiver { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int ZipCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}

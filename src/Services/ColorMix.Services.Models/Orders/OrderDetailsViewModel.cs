using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AutoMapper;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Orders
{
    public class OrderDetailsViewModel
    {
        public string OrderNumber { get; set; }

        public IList<OrderProductViewModel> Products { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public OrderAddressViewModel Address { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Administration
{
    public class UnavailableProductViewModel : IMapFrom<Product>
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }
    }
}

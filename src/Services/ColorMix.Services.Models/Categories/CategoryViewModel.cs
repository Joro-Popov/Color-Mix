using System;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping.Contracts;

namespace ColorMix.Services.Models.Categories
{
    public class CategoryViewModel : IMapFrom<Category>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}

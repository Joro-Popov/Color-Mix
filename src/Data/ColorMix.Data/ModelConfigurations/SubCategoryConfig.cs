﻿using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasOne(x => x.Category)
                .WithMany(x => x.SubCategories)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}

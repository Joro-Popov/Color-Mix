using System;
using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.SubCategory)
                   .WithMany(s => s.Products)
                   .HasForeignKey(p => p.SubCategoryId);

            builder.HasOne(p => p.Category)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}

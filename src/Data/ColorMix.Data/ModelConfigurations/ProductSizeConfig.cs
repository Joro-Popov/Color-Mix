using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class ProductSizeConfig : IEntityTypeConfiguration<ProductSize>

    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.HasKey(x => new {x.ProductId, x.SizeId});

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Sizes)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Size)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.SizeId);
        }
    }
}

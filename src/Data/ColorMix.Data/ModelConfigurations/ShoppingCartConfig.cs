using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace ColorMix.Data.ModelConfigurations
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasOne(x => x.User)
                .WithOne(x => x.ShoppingCart)
                .HasForeignKey<ShoppingCart>(x => x.UserId);
        }
    }
}

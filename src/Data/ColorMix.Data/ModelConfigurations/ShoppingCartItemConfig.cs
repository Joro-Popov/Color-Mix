using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class ShoppingCartItemConfig : IEntityTypeConfiguration<ShoppingCartItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.HasOne(x => x.ShoppingCart)
                .WithMany(x => x.ShoppingCartItems)
                .HasForeignKey(x => x.ShoppingCartId);
        }
    }
}

using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<ColorMixUser>
    {
        public void Configure(EntityTypeBuilder<ColorMixUser> builder)
        {
            builder.HasOne(x => x.Address)
                .WithOne(x => x.User)
                .HasForeignKey<Address>(x => x.UserId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.User)
                   .WithMany(u => u.Orders)
                   .HasForeignKey(o => o.UserId);

            builder.HasOne(o => o.Invoice)
                   .WithOne(i => i.Order)
                   .HasForeignKey<Invoice>(i => i.OrderId);
        }
    }
}

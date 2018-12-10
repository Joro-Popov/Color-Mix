using ColorMix.Data.ModelConfigurations;
using ColorMix.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ColorMix.Data
{
    public class ColorMixContext : IdentityDbContext<ColorMixUser>
    {
        public ColorMixContext(DbContextOptions<ColorMixContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AddressConfig());
            builder.ApplyConfiguration(new OrderConfig());
            builder.ApplyConfiguration(new OrderProductConfig());
            builder.ApplyConfiguration(new ProductConfig());
            builder.ApplyConfiguration(new CategorySubcategoriesConfig());

            base.OnModelCreating(builder);
        }
    }
}

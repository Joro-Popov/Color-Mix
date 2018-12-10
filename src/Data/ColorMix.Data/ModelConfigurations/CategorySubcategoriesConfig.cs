using ColorMix.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColorMix.Data.ModelConfigurations
{
    public class CategorySubcategoriesConfig : IEntityTypeConfiguration<CategorySubcategories>
    {
        public void Configure(EntityTypeBuilder<CategorySubcategories> builder)
        {
            builder.HasKey(x => new { x.CategoryId, x.SubCategoryId });

            builder.HasOne(x => x.Category)
                .WithMany(x => x.CategorySubCategories)
                .HasForeignKey(x => x.CategoryId);

            builder.HasOne(x => x.SubCategory)
                .WithMany(x => x.CategorySubCategories)
                .HasForeignKey(x => x.SubCategoryId);
        }
    }
}

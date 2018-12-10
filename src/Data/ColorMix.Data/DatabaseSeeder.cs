using ColorMix.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using SubCategory = ColorMix.Data.Models.SubCategory;

namespace ColorMix.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(RoleManager<IdentityRole> roleManager, ColorMixContext dbContext)
        {
            var roles = new string[] {"Admin", "User"};

            SeedRoles(roles, roleManager);
            SeedCategories(dbContext);
            SeedSubCategories(dbContext);
        }

        private static void SeedRoles(string[] roles, RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in roles)
            {
                var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();

                if (role != null) return;

                var result = roleManager
                    .CreateAsync(new IdentityRole() { Name = roleName, NormalizedName = roleName.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() }).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static void SeedCategories(ColorMixContext dbContext)
        {
            var categories = new List<Category>
            {
                new Category(){Name = "Men"},
                new Category(){Name = "Women"},
                new Category(){Name = "Kids"},
                new Category(){Name = "Others"},
            };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();
        }

        private static void SeedSubCategories(ColorMixContext dbContext)
        {
            var subcategories = new List<SubCategory>
            {
                new SubCategory(){Name = "Hats"},
                new SubCategory(){Name = "T-shirts"},
                new SubCategory(){Name = "Tops"},
                new SubCategory(){Name = "Sweatshirt"},
                new SubCategory(){Name = "Vests"},
                new SubCategory(){Name = "Scarf"},
                new SubCategory(){Name = "Jackets"},
                new SubCategory(){Name = "Shorts"},
                new SubCategory(){Name = "Trousers"},
                new SubCategory(){Name = "Jeans"},
                new SubCategory(){Name = "Shoes"},
                new SubCategory(){Name = "Accessories"},
                new SubCategory(){Name = "Dresses"},
                new SubCategory(){Name = "Skirts"},
                new SubCategory(){Name = "Bags"},

            };

            dbContext.SubCategories.AddRange(subcategories);
            dbContext.SaveChanges();
        }
    }
}

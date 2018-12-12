using ColorMix.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SubCategory = ColorMix.Data.Models.SubCategory;

namespace ColorMix.Data
{
    public class DatabaseSeeder
    {
        private readonly RequestDelegate next;

        public DatabaseSeeder(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, RoleManager<IdentityRole> roleManager, ColorMixContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

            var roles = new string[] { "Admin", "User" };

            if (!(dbContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                dbContext.Database.Migrate();

                SeedRoles(roles, roleManager);
                SeedCategories(dbContext);
                SeedSubCategories(dbContext);
            }

            await this.next(context);
        }
        
        private void SeedRoles(string[] roles, RoleManager<IdentityRole> roleManager)
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

        private void SeedCategories(ColorMixContext dbContext)
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

        private void SeedSubCategories(ColorMixContext dbContext)
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

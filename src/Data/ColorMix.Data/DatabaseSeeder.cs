using ColorMix.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task InvokeAsync(HttpContext context,
                                      RoleManager<IdentityRole> roleManager,
                                      ColorMixContext dbContext)
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

            dbContext.Database.Migrate();

            SeedRoles(roles, roleManager);
            SeedCategories(dbContext);
            SeedSubCategories(dbContext);

            await next(context);
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
            if (dbContext.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category(){Name = "Мъже"},
                new Category(){Name = "Жени"},
                new Category(){Name = "Деца"},
                new Category(){Name = "Други"},
            };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();
        }

        private void SeedSubCategories(ColorMixContext dbContext)
        {
            if (dbContext.SubCategories.Any()) return;

            var subcategories = new List<SubCategory>
            {
                new SubCategory(){Name = "Шапки"},
                new SubCategory(){Name = "Тениски"},
                new SubCategory(){Name = "Блузи"},
                new SubCategory(){Name = "Суитчъри"},
                new SubCategory(){Name = "Жилетки"},
                new SubCategory(){Name = "Шалове"},
                new SubCategory(){Name = "Якета"},
                new SubCategory(){Name = "Къси панталони"},
                new SubCategory(){Name = "Панталони"},
                new SubCategory(){Name = "Дънки"},
                new SubCategory(){Name = "Обувки"},
                new SubCategory(){Name = "Аксесоари"},
                new SubCategory(){Name = "Рокли"},
                new SubCategory(){Name = "Поли"},
                new SubCategory(){Name = "Чанти"},

            };

            dbContext.SubCategories.AddRange(subcategories);
            dbContext.SaveChanges();
        }
    }
}


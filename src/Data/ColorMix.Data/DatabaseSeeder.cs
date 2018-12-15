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

            var categories = dbContext.Categories
                .ToList();
            
            foreach (var category in categories)
            {
                var subCategories = new List<SubCategory>
                {
                    new SubCategory(){Name = "Шапки", CategoryId = category.Id},
                    new SubCategory(){Name = "Ризи", CategoryId = category.Id},
                    new SubCategory(){Name = "Тениски", CategoryId = category.Id},
                    new SubCategory(){Name = "Блузи", CategoryId = category.Id},
                    new SubCategory(){Name = "Суитчъри", CategoryId = category.Id},
                    new SubCategory(){Name = "Жилетки", CategoryId = category.Id},
                    new SubCategory(){Name = "Шалове", CategoryId = category.Id},
                    new SubCategory(){Name = "Якета", CategoryId = category.Id},
                    new SubCategory(){Name = "Къси панталони",CategoryId = category.Id},
                    new SubCategory(){Name = "Панталони",CategoryId = category.Id},
                    new SubCategory(){Name = "Дънки",CategoryId = category.Id},
                    new SubCategory(){Name = "Обувки",CategoryId = category.Id},
                    new SubCategory(){Name = "Аксесоари",CategoryId = category.Id},
                    
                };

                if (category.Name == "Жени")
                {
                    subCategories.Add(new SubCategory() {Name = "Рокли", CategoryId = category.Id});
                    subCategories.Add(new SubCategory() {Name = "Поли", CategoryId = category.Id});
                    subCategories.Add(new SubCategory() {Name = "Чанти", CategoryId = category.Id});
                }

                dbContext.SubCategories.AddRange(subCategories);
                dbContext.SaveChanges();
            }
        }
    }
}


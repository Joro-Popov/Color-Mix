using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit;

namespace ColorMix.Services.DataServices.Tests
{
    public class CategoryServiceTests
    {
        private readonly ColorMixContext dbContext;
        private readonly CategoryService categoryService;

        public CategoryServiceTests()
        {
            dbContext = new ColorMixContext(new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options);

            categoryService = new CategoryService(dbContext);

            Mapper.Reset();

            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );
        }

        [Fact]
        public void GetAllCategoriesShouldReturnAllPresentCategories()
        {
            foreach (var entity in dbContext.Categories)
                dbContext.Categories.Remove(entity);

            dbContext.SaveChanges();

            var categories = new List<Category>()
            {
                new Category() { Id = Guid.NewGuid(), Name = "Men" },
                new Category() { Id = Guid.NewGuid(), Name = "Woman" },
                new Category() { Id = Guid.NewGuid(), Name = "Kids" }

            };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();

            var expected = categoryService.GetAllCategories().Count();
            var actual = this.dbContext.Categories.Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckIfCategoryExistsShouldReturnProperValue()
        {
            var id = Guid.NewGuid();

            var category = new Category() { Id = id, Name = "Men" };

            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            var shouldReturnTrue = categoryService.CheckIfCategoryExists(id);
            var shouldReturnFalse = categoryService.CheckIfCategoryExists(Guid.NewGuid());

            Assert.True(shouldReturnTrue);
            Assert.False(shouldReturnFalse);
        }

        [Fact]
        public void GetCategoryNameShouldReturnProperValue()
        {
            var id = Guid.NewGuid();

            var category = new Category() { Id = id, Name = "Men" };

            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            var actual = categoryService.GetCategoryName(id);
            var actualNull = categoryService.GetCategoryName(Guid.NewGuid());

            Assert.Equal(category.Name, actual);
            Assert.NotSame(category.Name, actualNull);
        }

        [Fact]
        public void GetCategoryIdShouldReturnCategoryIdAndSubCategoryIdIfPresent()
        {
            var categoryId = Guid.NewGuid();
            var subCategoryId = Guid.NewGuid();

            var category = new Category() { Id = categoryId, Name = "Men" };
            var subCategory = new SubCategory() { Id = subCategoryId, Name = "Shirts", Category = category };

            dbContext.Categories.Add(category);
            dbContext.SubCategories.Add(subCategory);
            dbContext.SaveChanges();
            

            var actualCategoryId = categoryService.GetCategoryId(category.Name);
            var actualSubCategoryId = categoryService.GetCategoryId(category.Name, subCategory.Name);

            Assert.Equal(actualCategoryId, categoryId);
            Assert.NotEqual(Guid.NewGuid(), categoryId);

            Assert.Equal(actualSubCategoryId, subCategoryId);
            Assert.NotEqual(Guid.NewGuid(), subCategoryId);
        }

        [Fact]
        public void GetAllCategoriesAndSubCategoriesShouldReturnAllPresentCategoriesAndTheirSubCategories()
        {
            foreach (var entity in dbContext.Categories)
                dbContext.Categories.Remove(entity);

            dbContext.SaveChanges();

            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = Guid.NewGuid(), Name = "Men",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Shirts"},
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Jackets"},
                        new SubCategory() {Id = Guid.NewGuid(), Name = "T-Shirts"}
                    }
                },
                new Category()
                {
                    Id = Guid.NewGuid(), Name = "Woman",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Shirts"},
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Jackets"},
                        new SubCategory() {Id = Guid.NewGuid(), Name = "T-Shirts"}
                    }
                },
                new Category()
                {
                    Id = Guid.NewGuid(), Name = "Kids",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Shirts"},
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Jackets"},
                        new SubCategory() {Id = Guid.NewGuid(), Name = "T-Shirts"}
                    }
                }
            };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();

            var dbCategories = this.categoryService.GetAllCategoriesAndSubCategories();

            Assert.Equal(3,dbCategories.Count());
            Assert.Equal(3,dbCategories.First().SubCategories.Count);
        }

        [Fact]
        public void GetSubCategoryNamesShouldReturnAllPresentNamesForCurrentCategory()
        {
            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = Guid.NewGuid(), Name = "Men",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Shirts"},
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Jackets"},
                        new SubCategory() {Id = Guid.NewGuid(), Name = "T-Shirts"}
                    }
                },
                new Category()
                {
                    Id = Guid.NewGuid(), Name = "Woman",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Shirts"},
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Jackets"},
                        new SubCategory() {Id = Guid.NewGuid(), Name = "T-Shirts"}
                    }
                },
                new Category()
                {
                    Id = Guid.NewGuid(), Name = "Kids",
                    SubCategories = new List<SubCategory>()
                    {
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Shirts"},
                        new SubCategory() { Id = Guid.NewGuid(), Name = "Jackets"},
                        new SubCategory() {Id = Guid.NewGuid(), Name = "T-Shirts"}
                    }
                }
            };

            dbContext.Categories.AddRange(categories);
            dbContext.SaveChanges();

            var subCategoryNames1 = this.categoryService.GetSubCategoryNames("Men");
            var subCategoryNames2 = this.categoryService.GetSubCategoryNames("Woman");
            var subCategoryNames3 = this.categoryService.GetSubCategoryNames("Kids");

            Assert.Equal(3, subCategoryNames1.Count());
            Assert.Equal(3, subCategoryNames2.Count());
            Assert.Equal(3, subCategoryNames3.Count());
        }

        [Fact]
        public void CreateCategoryShouldAddNewCategoryToDatabase()
        {
            foreach (var entity in dbContext.Categories)
                dbContext.Categories.Remove(entity);

            dbContext.SaveChanges();

            var model = new CreateCategoryViewModel()
            {
                CategoryName = "Men",
                SubCаtegoryNames = "Shirts,T-Shirts,Jackets,Hats"
            };

            this.categoryService.CreateCategory(model);

            var actualCategoryName = this.dbContext.Categories.First().Name;
            var actualSubCategoriesCount = this.dbContext.Categories.First().SubCategories.Count;

            Assert.Equal("Men",actualCategoryName);
            Assert.Equal(4, actualSubCategoriesCount);
        }

        [Fact]
        public void CreateSubCategoryShouldAddNewSubCategoriesToDesiredOne()
        {
            var id = Guid.NewGuid();

            var category = new Category() { Id = id, Name = "Men" };

            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            var model = new CreateCategoryViewModel()
            {
                CategoryName = "Men",
                SubCаtegoryNames = "Shirts,T-Shirts,Jackets,Hats"
            };

            this.categoryService.CreateSubCategory(model);

            var actualSubCategoriesCount = this.dbContext.Categories.First().SubCategories.Count;
            
            Assert.Equal(4, actualSubCategoriesCount);
        }
    }
}

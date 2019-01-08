using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.Models.Administration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ColorMix.Services.DataServices.Tests
{
    public class ProductServiceTests
    {
        private readonly ColorMixContext dbContext;
        private readonly CategoryService categoryService;
        private readonly ProductService productService;

        public ProductServiceTests()
        {
            this.dbContext = new ColorMixContext(new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options);

            this.categoryService = new CategoryService(dbContext);
            
            var configuration = new Mock<IConfigurationRoot>();
            configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("");
            
            this.productService = new ProductService(dbContext, categoryService, configuration.Object);

            Mapper.Reset();

            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );
        }

        [Fact]
        public void GetProductsByCategoryShouldNotReturnEmptyCollection()
        {
            var categoryId = Guid.NewGuid();
            var subCategoryId = Guid.NewGuid();

            var category = new Category() { Id = categoryId, Name = "Men" };
            var subCategory = new SubCategory() { Id = subCategoryId, Name = "Shirts", Category = category };

            dbContext.Categories.Add(category);
            dbContext.SubCategories.Add(subCategory);
            dbContext.SaveChanges();

            var productId = Guid.NewGuid();

            var product = new Product()
            {
                Id = productId,
                Name = "jacket",
                Brand = "X-3",
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Color = "white",
                Description = "someDescription",
                IsAvailable = true,
                Material = "cotton",
                Price = 22.32m,
                ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                Sizes = new List<ProductSize>()
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var productsByCategoryAndSubcategory = productService.GetProductsByCategory(categoryId, null, subCategoryId);
            var productsByCategory = productService.GetProductsByCategory(categoryId, null);

            Assert.NotEmpty(productsByCategoryAndSubcategory.Products);
            Assert.NotEmpty(productsByCategory.Products);
        }

        [Fact]
        public void GetProductDetailsShouldReturnPopulatedObject()
        {
            var categoryId = Guid.NewGuid();
            var subCategoryId = Guid.NewGuid();

            var category = new Category() { Id = categoryId, Name = "Men" };
            var subCategory = new SubCategory() { Id = subCategoryId, Name = "Shirts", Category = category };

            dbContext.Categories.Add(category);
            dbContext.SubCategories.Add(subCategory);
            dbContext.SaveChanges();

            var productId = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var productId3 = Guid.NewGuid();

            var product = new Product()
            {
                Id = productId,
                Name = "jacket",
                Brand = "X-3",
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Color = "white",
                Description = "someDescription",
                IsAvailable = true,
                Material = "cotton",
                Price = 22.32m,
                ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                Sizes = new List<ProductSize>()
            };

            var product2 = new Product()
            {
                Id = productId2,
                Name = "jacket",
                Brand = "X-3",
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Color = "white",
                Description = "someDescription",
                IsAvailable = true,
                Material = "cotton",
                Price = 22.32m,
                ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                Sizes = new List<ProductSize>()
            };

            var product3 = new Product()
            {
                Id = productId3,
                Name = "jacket",
                Brand = "X-3",
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Color = "white",
                Description = "someDescription",
                IsAvailable = true,
                Material = "cotton",
                Price = 22.32m,
                ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                Sizes = new List<ProductSize>()
            };

            dbContext.Products.Add(product);
            dbContext.Products.Add(product2);
            dbContext.Products.Add(product3);
            dbContext.SaveChanges();

            var details = productService.GetProductDetails(productId);

            Assert.NotNull(details);
            Assert.NotEmpty(details.RandomProducts);
        }

        [Fact]
        public void GetProductShouldReturnCorrectProduct()
        {
            var categoryId = Guid.NewGuid();
            var subCategoryId = Guid.NewGuid();

            var category = new Category() { Id = categoryId, Name = "Men" };
            var subCategory = new SubCategory() { Id = subCategoryId, Name = "Shirts", Category = category };

            dbContext.Categories.Add(category);
            dbContext.SubCategories.Add(subCategory);
            dbContext.SaveChanges();

            var productId = Guid.NewGuid();

            var product = new Product()
            {
                Id = productId,
                Name = "jacket",
                Brand = "X-3",
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Color = "white",
                Description = "someDescription",
                IsAvailable = true,
                Material = "cotton",
                Price = 22.32m,
                ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                Sizes = new List<ProductSize>()
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();

            var dbProduct = productService.GetProduct(productId);

            Assert.Equal(productId, dbProduct.Id);
        }

        [Theory]
        [InlineData("S")]
        [InlineData("M")]
        [InlineData("L")]
        [InlineData("XL")]
        public void GetProductSizeShouldReturnCorrectSize(string size)
        {
            var sizes = new List<Size>()
            {
                new Size(){Id = Guid.NewGuid(),Abbreviation = "S"},
                new Size(){Id = Guid.NewGuid(),Abbreviation = "M"},
                new Size(){Id = Guid.NewGuid(),Abbreviation = "L"},
                new Size(){Id = Guid.NewGuid(),Abbreviation = "XL"}
            };

            dbContext.Sizes.AddRange(sizes);
            dbContext.SaveChanges();

            var dbSize = productService.GetProductSize(size);

            Assert.Equal(size, dbSize.Abbreviation);
        }

        [Fact]
        public void DeleteProductShouldRemoveProductFromDatabase()
        {
            var categoryId = Guid.NewGuid();
            var subCategoryId = Guid.NewGuid();

            var category = new Category() { Id = categoryId, Name = "Men" };
            var subCategory = new SubCategory() { Id = subCategoryId, Name = "Shirts", Category = category };

            dbContext.Categories.Add(category);
            dbContext.SubCategories.Add(subCategory);
            dbContext.SaveChanges();
            
            var productId = Guid.NewGuid();

            var product = new Product()
            {
                Id = productId,
                Name = "jacket",
                Brand = "X-3",
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Color = "white",
                Description = "someDescription",
                IsAvailable = true,
                Material = "cotton",
                Price = 22.32m,
                ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                Sizes = new List<ProductSize>()
            };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            
            this.productService.DeleteProduct(productId);

            Assert.Null(dbContext.Products.FirstOrDefault(x => x.Id == productId));
        }
    }
}

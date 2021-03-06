﻿using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ColorMix.Data.Models;
using ColorMix.Services.Models.Administration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using X.PagedList;
using Size = ColorMix.Data.Models.Size;

namespace ColorMix.Services.DataServices
{
    public class ProductService : IProductService
    {
        private readonly ColorMixContext dbContext;
        private readonly ICategoryService categoryService;
        private readonly IConfiguration configuration;
        private const int PAGE_SIZE = 9;

        public ProductService(ColorMixContext dbContext, ICategoryService categoryService, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.categoryService = categoryService;
            this.configuration = configuration;
        }

        public AllProductsViewModel GetProductsByCategory(Guid categoryId, int? pageNumber, Guid? subCategoryId = null)
        {
            var products = dbContext.Products
                .Where(p => p.CategoryId == categoryId && p.IsAvailable)
                .To<ProductViewModel>()
                .ToList();

            if (subCategoryId != null)
            {
                products = products.Where(p => p.SubCategoryId == subCategoryId).ToList();
            }

            var nextPage = pageNumber ?? 1;
            
            var allProducts = new AllProductsViewModel()
            {
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Products = products.ToPagedList(nextPage, PAGE_SIZE)
            };

            return allProducts;
        }

        public DetailsViewModel GetProductDetails(Guid id)
        {
            var details = dbContext.Products
                .Where(p => p.Id == id)
                .To<DetailsViewModel>()
                .First();
            
            details.RandomProducts = GetRandomProducts(id).ToList();

            return details;
        }

        public EditProductViewModel GetProduct(Guid id)
        {
            var product = this.dbContext.Products
                .Where(x => x.Id == id)
                .To<EditProductViewModel>()
                .First();

            return product;
        }

        public Size GetProductSize(string size)
        {
            var productSize = this.dbContext.Sizes.FirstOrDefault(s => s.Abbreviation == size);

            return productSize;
        }

        public IEnumerable<ProductViewModel> GetRandomProducts(int count)
        {
            var random = new Random();

            var randomProducts = new HashSet<ProductViewModel>();

            var products = dbContext.Products
                .To<ProductViewModel>()
                .ToList();

            var end = products.Count < count ? products.Count : count;

            while (randomProducts.Count < end)
            {
                var index = random.Next(0, products.Count);

                randomProducts.Add(products[index]);
            }

            return randomProducts;
        }

        public bool CheckIfProductExists(Guid id)
        {
            return dbContext.Products.Any(p => p.Id == id);
        }

        public void CreateProduct(CreateProductViewModel model)
        {
            var product = new Product()
            {
                Name = model.Name,
                Brand = model.Brand,
                CategoryId = this.categoryService.GetCategoryId(model.Category),
                SubCategoryId = this.categoryService.GetCategoryId(model.Category, model.SubCategory),
                Color = model.Color,
                Description = model.Description,
                IsAvailable = true,
                Material = model.Material,
                Price = model.Price,
                ImageUrl = this.GetImageUrl(model.Image)
            };
            
            var sizes = model.Sizes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new ProductSize()
                {
                    Product = product,
                    Size = this.GetProductSize(x) ?? new Size() { Abbreviation = x }
                }).ToList();

            product.Sizes = sizes;

            this.dbContext.Products.Add(product);
            this.dbContext.SaveChanges();
        }

        public void ChangeProductInfo(EditProductViewModel model)
        {
            var product = this.dbContext.Products
                .FirstOrDefault(x => x.Id == model.Id);

            product.Name = model.Name;
            product.Color = model.Color;
            product.Brand = model.Brand;
            product.Material = model.Material;
            product.Description = model.Description;
            product.Price = model.Price;
            
            var sizes = model.Sizes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            ChangeProductSize(model, product, sizes);

            if (model.Image != null)
            {
                product.ImageUrl = this.GetImageUrl(model.Image);
            }

            this.dbContext.Products.Update(product);
            this.dbContext.SaveChanges();
        }
        
        public void DeleteProduct(Guid id)
        {
            var product = this.dbContext.Products
                .FirstOrDefault(x => x.Id == id);

            product.IsAvailable = false;

            this.dbContext.Products.Update(product);
            this.dbContext.SaveChanges();
        }

        public void RestoreProduct(Guid id)
        {
            var productToRestore = this.dbContext
                .Products.FirstOrDefault(x => x.Id == id);

            productToRestore.IsAvailable = true;

            this.dbContext.Products.Update(productToRestore);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<UnavailableProductViewModel> GetAllUnavailableProducts()
        {
            var unavailableProducts = this.dbContext.Products
                .Where(p => !p.IsAvailable)
                .To<UnavailableProductViewModel>();

            return unavailableProducts;
        }

        private IEnumerable<ProductViewModel> GetRandomProducts(Guid productId)
        {
            var random = new Random();

            var randomProducts = new HashSet<ProductViewModel>();

            var category = dbContext.Products
                .FirstOrDefault(p => p.Id == productId)?.Category;

            var products = dbContext.Products
                .Where(p => p.CategoryId == category.Id && p.Id != productId)
                .To<ProductViewModel>()
                .ToList();

            var end = products.Count < 4 ? products.Count : 4;

            while (randomProducts.Count < end)
            {
                var index = random.Next(0, products.Count);

                randomProducts.Add(products[index]);
            }

            return randomProducts;
        }
        
        private string GetImageUrl(IFormFile image)
        {
            var apiKey = this.configuration["Authentication:Cloudinary:ApiKey"];
            var apiSecret = this.configuration["Authentication:Cloudinary:ApiSecret"];
            var cloudName = this.configuration["Authentication:Cloudinary:CloudName"];

            var account = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(account);
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName, image.OpenReadStream())
            };
            
            var uploadResult = cloudinary.Upload(uploadParams);
            
            return uploadResult.SecureUri.ToString();
        }

        private void ChangeProductSize(EditProductViewModel model, Product product, ICollection<string> sizes)
        {
            if (product.Sizes.Count > sizes.Count)
            {
                var sizesToRemove = product.Sizes
                    .Where(x => !sizes.Contains(x.Size.Abbreviation))
                    .ToList();

                foreach (var size in sizesToRemove)
                {
                    product.Sizes.Remove(size);
                }
            }
            else if (product.Sizes.Count < sizes.Count)
            {
                var sizesToAdd = model.Sizes
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !product.Sizes.Select(s => s.Size.Abbreviation).Contains(x))
                    .Select(x => new ProductSize()
                    {
                        Product = product,
                        Size = this.GetProductSize(x) ?? new Size() { Abbreviation = x }
                    })
                    .ToList();

                foreach (var size in sizesToAdd)
                {
                    product.Sizes.Add(size);
                }
            }
        }
    }
}

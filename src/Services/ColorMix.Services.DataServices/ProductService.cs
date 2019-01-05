using AutoMapper.QueryableExtensions;
using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ColorMix.Data.Models;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Cart;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Size = ColorMix.Data.Models.Size;

namespace ColorMix.Services.DataServices
{
    public class ProductService : IProductService
    {
        private readonly ColorMixContext dbContext;
        private readonly ICategoryService categoryService;
        private const int PAGE_SIZE = 9;

        public ProductService(ColorMixContext dbContext, ICategoryService categoryService)
        {
            this.dbContext = dbContext;
            this.categoryService = categoryService;
        }

        public AllProductsViewModel GetProductsByCategory(Guid categoryId, int? page, Guid? subCategoryId = null)
        {
            var products = dbContext.Products
                .Where(p => p.CategoryId == categoryId)
                .To<ProductViewModel>()
                .ToList();

            if (subCategoryId != null)
            {
                products = products.Where(p => p.SubCategoryId == subCategoryId).ToList();
            }

            var nextPage = page ?? 1;

            var pagedProducts = products.ToPagedList(nextPage, PAGE_SIZE);

            var allProducts = new AllProductsViewModel()
            {
                CategoryId = categoryId,
                SubCategoryId = subCategoryId,
                Products = pagedProducts
            };

            return allProducts;
        }

        public DetailsViewModel GetProductDetails(Guid id)
        {
            var details = dbContext.Products
                .Where(p => p.Id == id)
                .To<DetailsViewModel>()
                .First();
            
            var randomProducts = GetRandomProducts(id).ToList();

            details.RandomProducts = randomProducts;

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
            return this.dbContext.Sizes.FirstOrDefault(s => s.Abbreviation == size);
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
                .Distinct()
                .Where(x => product.Sizes.Any(s => s.Size.Abbreviation != x))
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

            if (product.CategoryId != this.categoryService.GetCategoryId(model.Category))
            {
                product.CategoryId = this.categoryService.GetCategoryId(model.Category);
            }

            if (product.SubCategoryId != this.categoryService.GetCategoryId(model.Category, model.SubCategory))
            {
                product.CategoryId = this.categoryService.GetCategoryId(model.Category, model.SubCategory);
            }

            var sizes = model.Sizes
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Where(x => product.Sizes.Any(s => s.Size.Abbreviation != x))
                .Select(x => new ProductSize()
                {
                    Product = product,
                    Size = this.GetProductSize(x) ?? new Size() { Abbreviation = x }
                }).ToList();

            product.Sizes = sizes;

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

            this.dbContext.Products.Remove(product);
            this.dbContext.SaveChanges();
        }

        private IEnumerable<ProductViewModel> GetRandomProducts(Guid productId)
        {
            var random = new Random();

            var randomProducts = new HashSet<ProductViewModel>();

            var category = dbContext.Products
                .Include(x => x.Category)
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
            var account = new Account("colormix", "578625178927514", "Sbnv_9RaLWphaerau3bWDJh2c_A");
            var cloudinary = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.Name, image.OpenReadStream())
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUri.ToString();
        }
    }
}

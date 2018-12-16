using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ColorMix.Services.DataServices
{
    public class ProductService : IProductService
    {
        private readonly ColorMixContext dbContext;

        public ProductService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ProductsViewModel> GetProductsByCategory(Guid id)
        {
            var products = dbContext.Products
                .Where(p => p.CategoryId == id)
                .To<ProductsViewModel>()
                .ToList();

            return products;
        }

        public IEnumerable<ProductsViewModel> GetProductsBySubCategory(Guid categoryId, Guid subCategoryId)
        {
            var products = dbContext.Products
                .Where(p => p.CategoryId == categoryId && p.SubCategoryId == subCategoryId)
                .To<ProductsViewModel>()
                .ToList();

            return products;
        }

        public DetailsViewModel GetProductDetails(Guid id)
        {
            var details = this.dbContext.Products
                .Where(p => p.Id == id)
                .ProjectTo<DetailsViewModel>()
                .First();

            var randomProducts = this.GetRandomProducts(id).ToList();

            details.RandomProducts = randomProducts;

            return details;
        }

        public bool CheckIfProductExists(Guid id)
        {
            return this.dbContext.Products.Any(p => p.Id == id);
        }

        private IEnumerable<ProductsViewModel> GetRandomProducts(Guid productId)
        {
            var random = new Random();

            var randomProducts = new HashSet<ProductsViewModel>();
            
            var category = this.dbContext.Products
                .Include(x => x.Category)
                .FirstOrDefault(p => p.Id == productId)?.Category;

            var products = this.dbContext.Products
                .Where(p => p.CategoryId == category.Id && p.Id != productId)
                .To<ProductsViewModel>()
                .ToList();

            var end = products.Count < 3 ? products.Count : 3;
            
            while (randomProducts.Count < end)
            {
                var index = random.Next(0, products.Count);

                randomProducts.Add(products[index]);
            }

            return randomProducts;
        }
    }
}

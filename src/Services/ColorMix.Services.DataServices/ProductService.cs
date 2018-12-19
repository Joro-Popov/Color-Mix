using AutoMapper.QueryableExtensions;
using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorMix.Services.DataServices
{
    public class ProductService : IProductService
    {
        private readonly ColorMixContext dbContext;

        public ProductService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ProductsViewModel> GetProductsByCategory(Guid categoryId, Guid? subCategoryId = null)
        {
            var products = dbContext.Products
                .Where(p => p.CategoryId == categoryId)
                .To<ProductsViewModel>()
                .ToList();

            if (subCategoryId != null)
            {
                products = products.Where(p => p.SubCategoryId == subCategoryId).ToList();
            }

            return products;
        }

        public DetailsViewModel GetProductDetails(Guid id)
        {
            var details = dbContext.Products
                .Where(p => p.Id == id)
                .ProjectTo<DetailsViewModel>()
                .First();

            //var product = this.dbContext.Products.FirstOrDefault(x => x.Id == categoryId);

            //var details = this.mapper.Map<DetailsViewModel>(product);

            var randomProducts = GetRandomProducts(id).ToList();

            details.RandomProducts = randomProducts;

            return details;
        }

        public bool CheckIfProductExists(Guid id)
        {
            return dbContext.Products.Any(p => p.Id == id);
        }

        private IEnumerable<ProductsViewModel> GetRandomProducts(Guid productId)
        {
            var random = new Random();

            var randomProducts = new HashSet<ProductsViewModel>();

            var category = dbContext.Products
                .Include(x => x.Category)
                .FirstOrDefault(p => p.Id == productId)?.Category;

            var products = dbContext.Products
                .Where(p => p.CategoryId == category.Id && p.Id != productId)
                .To<ProductsViewModel>()
                .ToList();

            var end = products.Count < 4 ? products.Count : 4;

            while (randomProducts.Count < end)
            {
                var index = random.Next(0, products.Count);

                randomProducts.Add(products[index]);
            }

            return randomProducts;
        }
    }
}

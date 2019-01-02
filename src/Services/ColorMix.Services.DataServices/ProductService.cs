using AutoMapper.QueryableExtensions;
using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ColorMix.Services.Models.Cart;
using X.PagedList;

namespace ColorMix.Services.DataServices
{
    public class ProductService : IProductService
    {
        private readonly ColorMixContext dbContext;
        private const int PAGE_SIZE = 9;

        public ProductService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
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
                .Include(x => x.Sizes)
                .ThenInclude(x => x.Size)
                .Where(p => p.Id == id)
                .To<DetailsViewModel>()
                .First();
            
            var randomProducts = GetRandomProducts(id).ToList();

            details.RandomProducts = randomProducts;

            return details;
        }

        public bool CheckIfProductExists(Guid id)
        {
            return dbContext.Products.Any(p => p.Id == id);
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
    }
}

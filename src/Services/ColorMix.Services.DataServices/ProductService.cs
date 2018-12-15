﻿using ColorMix.Data;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Products;
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
    }
}

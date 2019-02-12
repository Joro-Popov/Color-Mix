using System;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.DataServices.Helpers;
using ColorMix.Services.Models.Cart;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Services.Models.Products;
using Microsoft.AspNetCore.Identity;

namespace ColorMix.Services.DataServices
{
    public class CartService : ICartService
    {
        private readonly ColorMixContext dbContext;
        private readonly UserManager<ColorMixUser> userManager;

        public CartService(ColorMixContext dbContext, 
                           UserManager<ColorMixUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public IEnumerable<ShoppingCartViewModel> GetAllCartProducts(ISession session, string userId)
        {
            if (userId != null)
            {
                var user = this.dbContext.Users
                    .FirstOrDefault(x => x.Id == userId);

                if (user?.ShoppingCart == null)
                {
                    user.ShoppingCart = new ShoppingCart(){ UserId = userId };

                    this.dbContext.SaveChanges();
                }

                var items =  user.ShoppingCart.ShoppingCartItems
                    .Select(x => new ShoppingCartViewModel()
                    {
                        Id = x.ProductId,
                        Name = x.Product.Name,
                        Price = x.Product.Price,
                        Size = x.Size,
                        Quantity = x.Quantity,
                        ImageUrl = x.Product.ImageUrl,
                        Brand = x.Product.Brand
                    });

                return items;
            }

            var cart = SessionHelper.Get<List<ShoppingCartViewModel>>(session, "cart");

            return cart ?? new List<ShoppingCartViewModel>();
        }

        public void AddToCart(DetailsViewModel product, ISession session, string userId)
        {
            if (userId != null)
            {
                AddProductToDbCart(product, userId);
            }
            else
            {
                AddProductToSessionCart(product, session);
            }
        }

        public void MoveFromSessionCartToDbCart(ISession session, string userId)
        {
            var user = this.dbContext.Users
                .FirstOrDefault(x => x.Id == userId);
                
            if (user?.ShoppingCart == null)
            {
                var shoppingCart = new ShoppingCart()
                {
                    UserId = userId
                };

                user.ShoppingCart = shoppingCart;

                this.dbContext.SaveChanges();
            }

            var products = SessionHelper.Get<List<ShoppingCartViewModel>>(session, "cart");

            if (products == null) return;

            for (var i = 0; i < products.Count(); i++)
            {
                var shoppingCartItem = new ShoppingCartItem()
                {
                    ProductId = products[i].Id,
                    Size = products[i].Size,
                    ShoppingCartId = user.ShoppingCart.Id,
                    Quantity = products[i].Quantity
                };

                AddToShoppingCart(user, shoppingCartItem);
            }

            session.Clear();

            this.dbContext.SaveChanges();
        }

        public void Remove(Guid productId,ISession session, ClaimsPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                var cart = SessionHelper.Get<List<DetailsViewModel>>(session, "cart");
                var index = GetProductIndex(productId,session);

                cart.RemoveAt(index);

                SessionHelper.Set(session, "cart", cart);
            }
            else
            {
                var userId = this.userManager.GetUserId(principal);

                var user = this.dbContext.Users
                    .FirstOrDefault(x => x.Id == userId);

                var item = user.ShoppingCart.ShoppingCartItems
                    .ToList()
                    .FirstOrDefault(x => x.ProductId == productId);

                user.ShoppingCart.ShoppingCartItems.Remove(item);

                this.dbContext.SaveChanges();
            }
        }

        private int GetProductIndex(Guid productId,ISession session)
        {
            var cart = SessionHelper.Get<List<ShoppingCartViewModel>>(session, "cart");
            
            return cart.Any(x => x.Id == productId) ? cart.FindIndex(x => x.Id == productId) : -1;
        }

        private void AddProductToSessionCart(DetailsViewModel product, ISession session)
        {
            var shoppingCartItem = new ShoppingCartViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Size = product.Sizes.FirstOrDefault(x => x != null),
                Price = product.Price,
                Quantity = product.Quantity,
                Brand = product.Brand,
                ImageUrl = product.ImageUrl
            };

            if (SessionHelper.Get<List<ShoppingCartViewModel>>(session, "cart") == null)
            {
                var cart = new List<ShoppingCartViewModel>();
                
                cart.Add(shoppingCartItem);

                SessionHelper.Set(session, "cart", cart);
            }
            else
            {
                var cart = SessionHelper.Get<List<ShoppingCartViewModel>>(session, "cart");

                var index = GetProductIndex(product.Id,session);

                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(shoppingCartItem);
                }

                SessionHelper.Set(session, "cart", cart);
            }
        }

        private void AddProductToDbCart(DetailsViewModel product, string userId)
        {
            var user = this.dbContext.Users
                .FirstOrDefault(u => u.Id == userId);

            if (user?.ShoppingCart == null)
            {
                var shoppingCart = new ShoppingCart()
                {
                    UserId = userId
                };

                user.ShoppingCart = shoppingCart;

                this.dbContext.SaveChanges();
            }

            var shoppingCartItem = new ShoppingCartItem()
            {
                ProductId = product.Id,
                Size = product.Sizes.FirstOrDefault(x => x != null),
                ShoppingCartId = user.ShoppingCart.Id,
                Quantity = product.Quantity
            };

            AddToShoppingCart(user, shoppingCartItem);

            this.dbContext.SaveChanges();
        }

        private static void AddToShoppingCart(ColorMixUser user, ShoppingCartItem shoppingCartItem)
        {
            if (user.ShoppingCart.ShoppingCartItems.Any(x => x.ProductId == shoppingCartItem.ProductId))
            {
                var index = user.ShoppingCart.ShoppingCartItems.ToList().FindIndex(x => x.ProductId == shoppingCartItem.ProductId);

                user.ShoppingCart.ShoppingCartItems.ToList()[index].Quantity += shoppingCartItem.Quantity;
            }
            else
            {
                user.ShoppingCart.ShoppingCartItems.Add(shoppingCartItem);
            }
        }
    }
}

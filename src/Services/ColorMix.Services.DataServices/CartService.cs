using System;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.DataServices.Helpers;
using ColorMix.Services.Models.Cart;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using ColorMix.Data;
using ColorMix.Services.Mapping;

namespace ColorMix.Services.DataServices
{
    public class CartService : ICartService
    {
        private const int INITIAL_QUANTITY = 1;

        private readonly ColorMixContext dbContext;

        public CartService(ColorMixContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<CartItemViewModel> GetAllCartProducts(ISession session)
        {
            var cart = SessionHelper.Get<List<CartItemViewModel>>(session, "cart");

            return cart ?? new List<CartItemViewModel>();
        }

        public void AddToCart(CartItemViewModel product, ISession session)
        {
            if (SessionHelper.Get<List<CartItemViewModel>>(session, "cart") == null)
            {
                var cart = new List<CartItemViewModel>();
                
                cart.Add(product);

                SessionHelper.Set(session, "cart", cart);
            }
            else
            {
                var cart = SessionHelper.Get<List<CartItemViewModel>>(session, "cart");

                var index = GetProductIndex(product.Id, session);

                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(product);
                }

                SessionHelper.Set(session, "cart", cart);
            }
        }

        public void Remove(Guid productId, ISession session)
        {
            var cart = SessionHelper.Get<List<CartItemViewModel>>(session, "cart");
            var index = GetProductIndex(productId, session);

            cart.RemoveAt(index);

            SessionHelper.Set(session, "cart", cart);
        }

        private int GetProductIndex(Guid productId, ISession session)
        {
            var cart = SessionHelper.Get<List<CartItemViewModel>>(session, "cart");

            return cart.Any(x => x.Id == productId) ? cart.FindIndex(x => x.Id == productId) : -1;
        }
    }
}

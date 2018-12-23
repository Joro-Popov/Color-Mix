using System;
using System.Collections.Generic;
using System.Text;
using ColorMix.Services.Models.Cart;
using Microsoft.AspNetCore.Http;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface ICartService
    {
        IEnumerable<CartItemViewModel> GetAllCartProducts(ISession session);

        void AddToCart(CartItemViewModel product, ISession session);

        void Remove(Guid productId, ISession session);
    }
}

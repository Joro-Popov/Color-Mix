﻿using ColorMix.Services.Models.Cart;
using ColorMix.Services.Models.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface ICartService
    {
        IEnumerable<ShoppingCartViewModel> GetAllCartProducts(ISession session, string userId);

        void MoveFromSessionCartToDbCart(ISession session, string userId);

        void AddToCart(DetailsViewModel product, ISession session, string userId);

        void Remove(Guid productId,ISession session, ClaimsPrincipal principal);
    }
}

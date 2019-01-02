using ColorMix.Services.Models.Cart;
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
        IEnumerable<ShoppingCartViewModel> GetAllCartProducts(ISession session, ClaimsPrincipal principal);

        void MoveFromSessionCartToDbCart(ISession session, ClaimsPrincipal principal);

        void AddToCart(DetailsViewModel product, ISession session, ClaimsPrincipal principal);

        void Remove(Guid productId, string size, ISession session, ClaimsPrincipal principal);
    }
}

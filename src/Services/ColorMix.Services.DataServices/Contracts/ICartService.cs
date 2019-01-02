using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ColorMix.Services.Models.Cart;
using ColorMix.Services.Models.Products;
using Microsoft.AspNetCore.Http;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface ICartService
    {
       IEnumerable<ShoppingCartViewModel> GetAllCartProducts(ISession session, ClaimsPrincipal user);

        void AddToCart(DetailsViewModel product, ISession session, ClaimsPrincipal principal);

        void Remove(Guid productId, string size, ISession session, ClaimsPrincipal principal);
    }
}

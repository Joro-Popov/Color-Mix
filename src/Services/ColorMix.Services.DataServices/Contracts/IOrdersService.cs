using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Orders;
using ColorMix.Services.Models.Users;

namespace ColorMix.Services.DataServices.Contracts
{
    public interface IOrdersService
    {
        void PlaceOrder(OrdersViewModel model, ClaimsPrincipal principal);

        IEnumerable<MyOrdersViewModel> GetUserOrders(ClaimsPrincipal principal);

        IEnumerable<OrderViewModel> GetAllOrders();

        IEnumerable<OrderViewModel> GetAllSendOrders();

        OrderDetailsViewModel GetOrderDetails(Guid id);
    }
}

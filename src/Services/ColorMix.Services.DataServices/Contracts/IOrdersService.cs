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
        void PlaceOrder(OrdersViewModel model, string userId);

        void SendOrder(Guid orderId);

        bool OrderExists(Guid orderId);

        IEnumerable<MyOrdersViewModel> GetUserOrders(string userId);

        IEnumerable<OrderViewModel> GetAllOrders();

        IEnumerable<OrderViewModel> GetAllSendOrders();

        OrderDetailsViewModel GetOrderDetails(Guid orderId);
    }
}

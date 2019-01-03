using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Data.Models.Enumerations;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Models.Orders;
using Microsoft.AspNetCore.Identity;

namespace ColorMix.Services.DataServices
{
    public class OrdersService : IOrdersService
    {
        private readonly UserManager<ColorMixUser> userManager;
        private readonly ColorMixContext dbContext;

        public OrdersService(UserManager<ColorMixUser> userManager, ColorMixContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public void PlaceOrder(OrdersViewModel model, ClaimsPrincipal principal)
        {
            var userId = this.userManager.GetUserId(principal);

            var order = new Order()
            {
                OrderDate = DateTime.UtcNow,
                OrderTotalPrice = model.Products.Sum(x => x.Total),
                Status = OrderStatus.Waiting,
                UserId = userId
            };

            var orderProducts = model.Products
                .Select(x => new OrderProduct()
                {
                    ProductId = x.Id,
                    Order = order,
                    Quantity = model.Products.Count,
                    UnitTotalPrice = x.Total
                }).ToList();

            order.OrderProducts = orderProducts;

            this.dbContext.Orders.Add(order);

            this.dbContext.SaveChanges();
        }
    }
}

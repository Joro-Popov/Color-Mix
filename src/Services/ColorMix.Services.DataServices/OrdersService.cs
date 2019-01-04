using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Data.Models.Enumerations;
using ColorMix.Services.DataServices.Contracts;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Administration;
using ColorMix.Services.Models.Orders;
using ColorMix.Services.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                Status = OrderStatus.BeingPrepared,
                UserId = userId
            };

            var orderProducts = model.Products
                .Select(x => new OrderProduct()
                {
                    ProductId = x.Id,
                    Order = order,
                    Quantity = model.Products.FirstOrDefault(p => p.Id == x.Id).Quantity,
                    UnitTotalPrice = x.Total
                }).ToList();

            order.OrderProducts = orderProducts;

            var user = this.dbContext.Users
                .FirstOrDefault(x => x.Id == userId);

            user?.ShoppingCart.ShoppingCartItems.Clear();
            
            var invoice = new Invoice()
            {
                Order = order,
                CreatedOn = DateTime.UtcNow
            };

            this.dbContext.Invoices.Add(invoice);
            this.dbContext.Orders.Add(order);

            this.dbContext.SaveChanges();
        }

        public IEnumerable<MyOrdersViewModel> GetUserOrders(ClaimsPrincipal principal)
        {
            var userId = this.userManager.GetUserId(principal);

            var orders = this.dbContext.Orders
                .Where(x => x.UserId == userId)
                .To<MyOrdersViewModel>()
                .ToList();

            return orders;
        }

        public IEnumerable<OrderViewModel> GetAllOrders()
        {
            var orders = this.dbContext.Orders
                .Where(x => x.Status == OrderStatus.BeingPrepared)
                .To<OrderViewModel>()
                .ToList();

            return orders;
        }

        public OrderDetailsViewModel GetOrderDetails(Guid id)
        {
            var order = this.dbContext.Orders
                .FirstOrDefault(x => x.Id == id);

            var orderDetails = new OrderDetailsViewModel()
            {
                OrderNumber = order.Id.ToString().Substring(0,8),
                Products = order.OrderProducts
                    .Select(x => new OrderProductViewModel()
                    {
                        Id = x.Product.Id,
                        Name = x.Product.Name,
                        Quantity = x.Quantity,
                        ImageUrl = x.Product.ImageUrl,
                        UnitPrice = x.Product.Price,
                        TotalPrice = x.UnitTotalPrice
                    }).ToList(),
                Address = new OrderAddressViewModel()
                {
                    Country = order.User.Address.Country,
                    Street = order.User.Address.Street,
                    City = order.User.Address.City,
                    Receiver = $"{order.User.FirstName} {order.User.LastName}",
                    PhoneNumber = order.User.PhoneNumber,
                    ZipCode = order.User.Address.ZipCode
                },
                OrderTotalPrice = order.OrderTotalPrice
            };

            return orderDetails;
        }
    }
}

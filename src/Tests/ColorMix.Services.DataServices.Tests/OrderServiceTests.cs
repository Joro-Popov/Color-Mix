using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using ColorMix.Data;
using ColorMix.Data.Models;
using ColorMix.Data.Models.Enumerations;
using ColorMix.Services.Mapping;
using ColorMix.Services.Models.Cart;
using ColorMix.Services.Models.Categories;
using ColorMix.Services.Models.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ColorMix.Services.DataServices.Tests
{
    public class OrderServiceTests
    {
        private readonly ColorMixContext dbContext;
        private readonly OrdersService orderService;

        public OrderServiceTests()
        {
            this.dbContext = new ColorMixContext(new DbContextOptionsBuilder<ColorMixContext>()
                .UseInMemoryDatabase("ColorMix")
                .Options);

            this.orderService = new OrdersService(this.dbContext);

            Mapper.Reset();

            AutoMapperConfig.RegisterMappings(
                typeof(CategoryViewModel).Assembly
            );
        }

        [Fact]
        public void PlaceOrderShouldCorrectlyAddNewOrder()
        {
            var model = new OrdersViewModel()
            {
                Products = new List<ShoppingCartViewModel>(),
                AddressStreet = "street",
                AddressCity = "city",
                AddressCountry = "country",
                AddressZipCode = 1000,
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345",
                Email = "Email@email.com"
            };

            var id = Guid.NewGuid().ToString();

            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            this.orderService.PlaceOrder(model,id);

            Assert.NotEmpty(dbContext.Orders);
        }

        [Fact]
        public void SendOrderShouldChangeOrderStatusToSend()
        {
            var model = new OrdersViewModel()
            {
                Products = new List<ShoppingCartViewModel>(),
                AddressStreet = "street",
                AddressCity = "city",
                AddressCountry = "country",
                AddressZipCode = 1000,
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345",
                Email = "Email@email.com"
            };

            var id = Guid.NewGuid().ToString();

            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            this.orderService.PlaceOrder(model, id);

            var oldOrder = this.orderService.GetAllOrders().First();

            this.orderService.SendOrder(oldOrder.Id);

            var newOrder = this.orderService.GetAllSendOrders().First();

            Assert.NotEqual(oldOrder.Status, newOrder.Status);
        }

        [Fact]
        public void GetUserOrdersShouldReturnCurrentUserOrders()
        {
            var id = Guid.NewGuid().ToString();

            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.BeingPrepared
                    }
                }
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();
            
            var orders = this.orderService.GetUserOrders(id);

            Assert.Single(orders);
        }

        [Fact]
        public void GetAllOrdersShouldReturnAllOrdersMade()
        {
            var id = Guid.NewGuid().ToString();
            
            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.BeingPrepared
                    },
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.BeingPrepared
                    },
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.BeingPrepared
                    },
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.BeingPrepared
                    }
                }
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            var orders = this.orderService.GetAllOrders();

            var expected = this.dbContext.Orders.Count(x => x.Status == OrderStatus.BeingPrepared);

            Assert.Equal(expected, orders.Count());
        }

        [Fact]
        public void GetAllSendOrdersShouldReturnOrdersWithStatusSend()
        {
            var id = Guid.NewGuid().ToString();
            
            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.Send
                    },
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.Send
                    },
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.Send
                    },
                    new Order()
                    {
                        Id = Guid.NewGuid(),
                        Invoice = new Invoice(),
                        OrderDate = DateTime.UtcNow,
                        OrderProducts = new List<OrderProduct>(),
                        OrderTotalPrice = 0,
                        Status = OrderStatus.Send
                    }
                }
            };

            dbContext.Users.Add(user);

            dbContext.SaveChanges();

            var orders = this.orderService.GetAllSendOrders();

            Assert.Equal(4, orders.Count());
        }

        [Fact]
        public void GetOrderDetailsShouldNotReturnNull()
        {
            var id = Guid.NewGuid().ToString();

            var user = new ColorMixUser()
            {
                Id = id,
                ShoppingCart = new ShoppingCart(),
                Address = new Address(),
                FirstName = "Georgi",
                LastName = "Popov",
                PhoneNumber = "12345678",
                Age = 25,
                Email = "popov@abv.bg"
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var orderId = Guid.NewGuid();

            var order = new Order()
            {
                Id = orderId,
                User = user,
                Invoice = new Invoice(),
                OrderDate = DateTime.UtcNow,
                OrderTotalPrice = 33,
                Status = OrderStatus.BeingPrepared
            };

            var categoryId = Guid.NewGuid();
            var subCategoryId = Guid.NewGuid();

            var category = new Category() { Id = categoryId, Name = "Men" };
            var subCategory = new SubCategory() { Id = subCategoryId, Name = "Shirts", Category = category };

            dbContext.Categories.Add(category);
            dbContext.SubCategories.Add(subCategory);
            dbContext.SaveChanges();

            var orderProducts = new List<OrderProduct>()
            {
                new OrderProduct()
                {
                    Product = new Product()
                    {
                        Id = Guid.NewGuid(),
                        Name = "jacket",
                        Brand = "X-3",
                        CategoryId = categoryId,
                        SubCategoryId = subCategoryId,
                        Color = "white",
                        Description = "someDescription",
                        IsAvailable = true,
                        Material = "cotton",
                        Price = 22.32m,
                        ImageUrl = "https://res.cloudinary.com/colormix/image/upload/v1546720935/ilpsngj9d4p3jvvwdjk1.jpg",
                        Sizes = new List<ProductSize>()
                    },
                    Order = order,
                    Quantity = 1,
                    UnitTotalPrice = 10
                }
            };

            order.OrderProducts = orderProducts;

            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();

            var orderDetails = this.orderService.GetOrderDetails(orderId);

            Assert.NotNull(orderDetails);
        }
    }
}

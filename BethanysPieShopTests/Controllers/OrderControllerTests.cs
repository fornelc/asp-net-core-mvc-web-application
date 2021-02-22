using NUnit.Framework;
using Moq;
using BethanysPieShop.Models;
using BethanysPieShopTests.Mocks.Repositories;
using BethanysPieShop.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BethanysPieShopTests.Mocks.Contexts;

namespace BethanysPieShopTests.Controllers
{
    public class OrderControllerTests
    {
        [Test]
        public void Checkout_WhenShoppingCartIsEmpty_ReturnsAViewResult()
        {
            // Arrange
            AppDbContext context = MockAppDbContext.BuildContextInMemoryDb();

            BuildEmptyCart(context);

            Order order = new Order();
            var mockOrderRepository = new MockOrderRepository()
                .CreateOrder(order);

            var sut = new OrderController(mockOrderRepository.Object,
                new ShoppingCart(context));

            // Act
            var result = sut.Checkout(order);

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            mockOrderRepository.Verify(x => x.CreateOrder(order), Times.Never());
        }

        [Test]
        public void Checkout_WhenShoppingCartContainsItems_ReturnsARedirectToAction()
        {
            // Arrange
            AppDbContext context = MockAppDbContext.BuildContextInMemoryDb();

            BuildCartWithItems(context);

            Order order = new Order();
            var mockOrderRepository = new MockOrderRepository()
                .CreateOrder(order);

            var sut = new OrderController(mockOrderRepository.Object,
                new ShoppingCart(context));

            // Act
            var result = sut.Checkout(order);

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            mockOrderRepository.Verify(x => x.CreateOrder(order), Times.Once());
        }

        private void BuildEmptyCart(AppDbContext context)
        {
            var shoppingCart = new List<ShoppingCartItem>();

            context.ShoppingCartItems.AddRange(shoppingCart);
            context.SaveChanges();
        }

        private void BuildCartWithItems(AppDbContext context)
        {
            var shoppingCart = new[]
            {
                new ShoppingCartItem { ShoppingCartItemId = 1},
                new ShoppingCartItem { ShoppingCartItemId = 2},
                new ShoppingCartItem { ShoppingCartItemId = 3},
                new ShoppingCartItem { ShoppingCartItemId = 4},
            };

            context.ShoppingCartItems.AddRange(shoppingCart);
            context.SaveChanges();
        }

    }
}

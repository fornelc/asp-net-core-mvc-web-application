using BethanysPieShop.Models;
using BethanysPieShopTests.Mocks.Contexts;
using NUnit.Framework;

namespace BethanysPieShopTests.Repositories
{
    public class OrderRepositoryTests
    {
        private AppDbContext _context;

        private ShoppingCart _shoppingCart;

        private OrderRepository _sut;

        [SetUp]
        public void SetUp()
        {
            _context = MockAppDbContext.BuildContextInMemoryDb();
            BuildCartWithItems(_context);
            _shoppingCart = new ShoppingCart(_context);
            _shoppingCart.ShoppingCartId = "1";
            _shoppingCart.ShoppingCartItems = _shoppingCart.GetShoppingCartItems();

            _sut = new OrderRepository(_context, _shoppingCart);
        }

        [TearDown]
        public void TearDown()
        {
            MockAppDbContext.Dispose(_context);
        }

        [Test]
        public void CreateOrder_OrderIsAddedInDbContext()
        {
            // Arrange
            var orderId = 11;
            var order = new Order
            {
                OrderId = orderId
            };

            // Act
            _sut.CreateOrder(order);

            // Assert
            Order orderAdded = _context.Orders.Find(orderId);
            Assert.AreEqual(orderId, orderAdded.OrderId);
            Assert.AreEqual(2, orderAdded.OrderDetails.Count);
            Assert.AreEqual(1, orderAdded.OrderDetails[0].PieId);
            Assert.AreEqual(2, orderAdded.OrderDetails[1].PieId);
        }


        private void BuildCartWithItems(AppDbContext context)
        {
            var shoppingCart = new[]
            {
                new ShoppingCartItem { Pie = new Pie { PieId = 1, Price = 10.0m},
                    Amount = 2,
                    ShoppingCartId = "1"},
                new ShoppingCartItem { Pie = new Pie { PieId = 2, Price = 5.0m},
                    Amount = 1,
                    ShoppingCartId = "1"},
                new ShoppingCartItem { Pie = new Pie { PieId = 3, Price = 10.0m},
                    Amount = 1,
                    ShoppingCartId = "3"},
                new ShoppingCartItem { Pie = new Pie { PieId = 4, Price = 20.0m},
                    Amount = 1,
                    ShoppingCartId = "4"},
            };

            context.ShoppingCartItems.AddRange(shoppingCart);
            context.SaveChanges();
        }
    }
}

using System.Collections.Generic;
using BethanysPieShop.Models;
using BethanysPieShopTests.Mocks.Repositories;
using NUnit.Framework;
using BethanysPieShop.Controllers;
using BethanysPieShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BethanysPieShopTests.Mocks.Contexts;

namespace BethanysPieShopTests.Controllers
{
    public class ShoppingCartControllerTests
    {
        private const string Category1 = "Category1";
        private const string Category2 = "Category2";

        private List<Pie> _pies;

        private AppDbContext _context;

        private MockPieRepository _mockPieRepository;
        private ShoppingCart _shoppingCart;

        private ShoppingCartController _sut;

        [SetUp]
        public void SetUp()
        {
            _pies = BuildPies();
            _mockPieRepository = new MockPieRepository().MockGetAllPies(_pies);

            _context = MockAppDbContext.BuildContextInMemoryDb();
            BuildCartWithItems(_context);
            _shoppingCart = new ShoppingCart(_context);
            _shoppingCart.ShoppingCartId = "1";

            _sut = new ShoppingCartController(_mockPieRepository.Object, _shoppingCart);
        }

        [TearDown]
        public void TearDown()
        {
            MockAppDbContext.Dispose(_context);
        }

        [Test]
        public void Index_ReturnsAViewResult()
        {
            // Act
            var result = _sut.Index();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            var viewModel = _sut.ViewData.Model as ShoppingCartViewModel;
            Assert.AreEqual(_shoppingCart, viewModel.ShoppingCart);
            Assert.AreEqual(25, viewModel.ShoppingCartTotal);
        }

        [Test]
        public void AddToShoppingCart_PieExists_PieIsAddedInPiesList()
        {
            // Act
            var result = _sut.AddToShoppingCart(5);

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            List<ShoppingCartItem> cartItems = _shoppingCart.GetShoppingCartItems();
            Assert.AreEqual(3, cartItems.Count);
            ShoppingCartItem lastItemAdded = cartItems[cartItems.Count - 1];
            Assert.AreEqual(5, lastItemAdded.Pie.PieId);
        }

        [Test]
        public void RemoveFromShoppingCart_PieExists_PieIsRemovedFromPiesList()
        {
            // Act
            var result = _sut.RemoveFromShoppingCart(2);

            // Assert
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
            List<ShoppingCartItem> cartItems = _shoppingCart.GetShoppingCartItems();
            Assert.AreEqual(1, cartItems.Count);
            Assert.AreEqual(1, cartItems[0].Pie.PieId);
        }

        private List<Pie> BuildPies()
        {
            return new List<Pie>
            {
                new Pie { PieId = 2, Price = 5.0m,
                    Category = new Category { CategoryName = Category1} },
                new Pie { PieId = 4, Price = 20.0m,
                    Category = new Category { CategoryName = Category1} },
                new Pie { PieId = 1, Price = 10.0m,
                    Category = new Category { CategoryName = Category1} },
                new Pie { PieId = 5, Price = 15.0m,
                    Category = new Category { CategoryName = Category2} },
                new Pie { PieId = 3, Price = 10.0m,
                    Category = new Category { CategoryName = Category2} },
            };
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

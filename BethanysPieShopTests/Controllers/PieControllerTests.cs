using System;
using System.Collections.Generic;
using BethanysPieShop.Controllers;
using BethanysPieShop.Models;
using BethanysPieShop.ViewModels;
using BethanysPieShopTests.Mocks.Repositories;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;

namespace BethanysPieShopTests.Controllers
{
    public class PieControllerTests
    {
        private const string Category1 = "Category1";
        private const string Category2 = "Category2";

        private List<Pie> _pies;
        private List<Category> _categories;

        private MockPieRepository _mockPieRepository;
        private MockCategoryRepository _mockCategoryRepository;

        private PieController _sut;

        [SetUp]
        public void SetUp()
        {
            _pies = BuildPies();
            _categories = BuildCategories();

            _mockPieRepository = new MockPieRepository().MockGetAllPies(_pies);
            _mockCategoryRepository = new MockCategoryRepository().MockGetAllCategories(BuildCategories());

            _sut = new PieController(_mockPieRepository.Object, _mockCategoryRepository.Object);
        }

        [Test]
        public void List_CategoryIsEmpty_ReturnsAllPiesCategory()
        {
            // Act
            var result = _sut.List("");

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            var viewModel = _sut.ViewData.Model as PiesListViewModel;
            Assert.AreEqual(viewModel.Pies.ToList(), _pies.OrderBy(p => p.PieId));
            Assert.AreEqual(PieController.AllPies, viewModel.CurrentCategory);

        }

        [Test]
        public void List_CategoryIsNotEmpty_ReturnsPiesAndCategory()
        {
            // Arrange
            var categoryName = Category1;

            // Act
            var result = _sut.List(categoryName);

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            var viewModel = _sut.ViewData.Model as PiesListViewModel;
            Assert.AreEqual(viewModel.Pies.ToList(),
                _pies.Where(p => p.Category.CategoryName == categoryName)
                    .OrderBy(p => p.PieId));
            Assert.AreEqual(Category1, viewModel.CurrentCategory);
        }

        private List<Pie> BuildPies()
        {
            return new List<Pie>
            {
                new Pie { PieId = 2,
                    Category = new Category { CategoryName = Category1} },
                new Pie { PieId = 4,
                    Category = new Category { CategoryName = Category1} },
                new Pie { PieId = 1,
                    Category = new Category { CategoryName = Category1} },
                new Pie { PieId = 3,
                    Category = new Category { CategoryName = Category2} },
            };
        }

        private List<Category> BuildCategories()
        {
            return new List<Category>
            {
                new Category { CategoryName = Category1},
                new Category { CategoryName = Category2},
            };
        }
    }
}

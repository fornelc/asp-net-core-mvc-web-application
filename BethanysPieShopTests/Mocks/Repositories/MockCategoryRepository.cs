using System.Collections.Generic;
using BethanysPieShop.Models;
using Moq;

namespace BethanysPieShopTests.Mocks.Repositories
{
    public class MockCategoryRepository : Mock<ICategoryRepository>
    {
        public MockCategoryRepository MockGetAllCategories(List<Category> results)
        {
            Setup(x => x.AllCategories)
                .Returns(results);

            return this;
        } 
    }
}

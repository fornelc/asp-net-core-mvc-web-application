using System;
using System.Collections.Generic;
using BethanysPieShop.Models;
using Moq;

namespace BethanysPieShopTests.Mocks.Repositories
{
    public class MockPieRepository : Mock<IPieRepository>
    {
        public MockPieRepository MockGetPiesOfTheWeek(List<Pie> results)
        {
            Setup(x => x.PiesOfTheWeek)
                .Returns(results);

            return this;
        }

        public MockPieRepository MockGetAllPies(List<Pie> results)
        {
            Setup(x => x.AllPies)
                .Returns(results);

            return this;
        }
    }
}

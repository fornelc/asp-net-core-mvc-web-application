using System;
using System.Collections.Generic;
using BethanysPieShop.Models;
using Moq;

namespace BethanysPieShopTests.Mocks.Repositories
{
    public class MockOrderRepository : Mock<IOrderRepository>
    {
        public MockOrderRepository CreateOrder(Order order)
        {
            Setup(x => x.CreateOrder(order));
            return this;
        }
    }
}

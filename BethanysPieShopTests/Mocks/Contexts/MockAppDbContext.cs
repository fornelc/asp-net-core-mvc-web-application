using System;
using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopTests.Mocks.Contexts
{
    public class MockAppDbContext
    {
        public static AppDbContext BuildContextInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "AppDb")
                            .Options;

            return new AppDbContext(options);
        }

        public static void Dispose(AppDbContext context)
        {
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }
}

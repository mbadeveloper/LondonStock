using LondonStock.Domain;
using Microsoft.EntityFrameworkCore;

namespace LondonStock.Tests.Helpers
{
    public static class DbHelper
    {
        public static DatabaseContext CreateDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("LondonStock")
                .EnableSensitiveDataLogging(true)
                .Options;
            return new DatabaseContext(options);
        }
    }
}

using LondonStock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonStock.Tests.Helpers
{
    public abstract class AbstractDbTests
    {
        protected DatabaseContext databaseContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            databaseContext = DbHelper.CreateDatabaseContext();

            databaseContext.Brokers.AddRange(UnitTestHelpers.GetBrokers);
            databaseContext.Stocks.AddRange(UnitTestHelpers.GetStocks);
            databaseContext.StocksExchange.AddRange(UnitTestHelpers.GetStocksExchange);
            databaseContext.SaveChanges();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            databaseContext.Database.EnsureDeleted();
            databaseContext.Dispose();
        }

        public void ResetStocksExchange()
        {
            databaseContext.StocksExchange.RemoveRange(databaseContext.StocksExchange);
            databaseContext.StocksExchange.AddRange(UnitTestHelpers.GetStocksExchange);
            databaseContext.SaveChanges();
        }
    }
}

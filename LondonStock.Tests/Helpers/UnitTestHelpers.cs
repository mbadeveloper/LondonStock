using LondonStock.Contracts;
using LondonStock.Domain;

namespace LondonStock.Tests.Helpers
{
    public static class UnitTestHelpers
    {
        public static string TickerSymbol1 => "TEST1";
        public static string TickerSymbol2 => "TEST2";
        public static string TickerSymbol3 => "TEST3";
        public static string TickerSymbol4 => "TEST4";
        public static string TickerSymbol5 => "TEST5";

        public static Guid BrokerId1 => new Guid("1c3ad083-03bc-4061-9a2a-74c9e7749e3f");
        public static Guid BrokerId2 => new Guid("d71e0c80-5eb2-4c7c-bda6-d01b3e45bd94");

        public static StockExchangeRequest GetStockExchangeRequest1 => new StockExchangeRequest
        {
            TickerSymbol = TickerSymbol2,
            NumberShares = 128.73M,
            Price = 10.25M,
            ExchangeDate = new DateTime(2023, 11, 12, 14, 55, 21)
        };

        public static StockExchange GetStockExchange1 => new StockExchange
        {
            StockId = 2,
            Stock = new Stock
            {
                Id = 2,
                TickerSymbol = TickerSymbol2,
            },
            NumberShares = 128.73M,
            Price = 10.25M,
            ExchangeDate = new DateTime(2023, 11, 12, 14, 55, 21)
        };

        public static StockExchangeResponse GetStockExchangeResponse1 => new StockExchangeResponse
        {
            TickerSymbol = TickerSymbol2,
            NumberShares = 128.73M,
            Price = 10.25M,
            ExchangeDate = new DateTime(2023, 11, 12, 14, 55, 21)
        };

        public static StockResponse GetStockResponse1 => new StockResponse
        {
            TickerSymbol = TickerSymbol2,
            Price = 10.25M
        };

        public static IEnumerable<Broker> GetBrokers = new List<Broker>
        {
            new Broker
            {
                Id = BrokerId1,
                Name = "Broker 1"
            },
            new Broker
            {
                Id = BrokerId2,
                Name = "Broker 2"
            },

        };

        public static IEnumerable<Stock> GetStocks = new List<Stock>
        {
            new Stock
            {
                Id = 1,
                StockName = "Company 1",
                TickerSymbol = TickerSymbol1
            },
            new Stock
            {
                Id = 2,
                StockName = "Company 2",
                TickerSymbol = TickerSymbol2
            },
            new Stock
            {
                Id = 3,
                StockName = "Company 3",
                TickerSymbol = TickerSymbol3
            },
            new Stock
            {
                Id = 4,
                StockName = "Company 4",
                TickerSymbol = TickerSymbol4
            },
            new Stock
            {
                Id = 5,
                StockName = "Company 5",
                TickerSymbol = TickerSymbol5
            },
        };

        public static IEnumerable<StockExchange> GetStocksExchange => new List<StockExchange>
        {
            StockExchange1,
            StockExchange2,
            StockExchange3,
            StockExchange4,
            StockExchange5,
            StockExchange6,
            StockExchange7,
            StockExchange8,
            StockExchange9,
            StockExchange10
        };

        public static StockExchange StockExchangeTickerSymbol(int id, int tickerSymbolId, decimal numberShares, decimal price, DateTime dateTime) => new StockExchange
        {
            Id = id,
            StockId = tickerSymbolId,
            NumberShares = numberShares,
            Price = price,
            ExchangeDate = dateTime
        };

        public static StockExchange StockExchange1 => StockExchangeTickerSymbol(1, 1, 12.56M, 11.09M, new DateTime(2023, 11, 21, 12, 45, 22));
        public static StockExchange StockExchange2 => StockExchangeTickerSymbol(2, 3, 126.33M, 1.19M, new DateTime(2023, 11, 21, 12, 47, 28));
        public static StockExchange StockExchange3 => StockExchangeTickerSymbol(3, 5, 34.3M, 14.59M, new DateTime(2023, 11, 21, 12, 47, 29));
        public static StockExchange StockExchange4 => StockExchangeTickerSymbol(4, 1, 12M, 11.21M, new DateTime(2023, 11, 21, 12, 47, 8));
        public static StockExchange StockExchange5 => StockExchangeTickerSymbol(5, 5, 7.60M, 12.51M, new DateTime(2023, 11, 21, 12, 30, 2));
        public static StockExchange StockExchange6 => StockExchangeTickerSymbol(6, 1, 78.96M, 12.51M, new DateTime(2023, 11, 21, 12, 17, 8));
        public static StockExchange StockExchange7 => StockExchangeTickerSymbol(7, 3, 8.44M, 1.51M, new DateTime(2023, 11, 21, 12, 40, 23));
        public static StockExchange StockExchange8 => StockExchangeTickerSymbol(8, 3, 54.47M, 2.01M, new DateTime(2023, 11, 21, 12, 39, 6));
        public static StockExchange StockExchange9 => StockExchangeTickerSymbol(9, 5, 4.09M, 12.53M, new DateTime(2023, 11, 21, 12, 39, 19));
        public static StockExchange StockExchange10 => StockExchangeTickerSymbol(10, 1, 3.37M, 11.91M, new DateTime(2023, 11, 21, 12, 37, 5));
    }
}

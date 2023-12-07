using LondonStock.Contracts;
using LondonStock.Domain;

namespace LondonStock.Converters
{
    public class StockConverter : IStockConverter
    {
        public StockExchange ToStockExchange(StockExchangeRequest stockExchangeRequest, int stockId)
        {
            return new StockExchange
            {
                StockId = stockId,
                NumberShares = stockExchangeRequest.NumberShares,
                Price = stockExchangeRequest.Price,
                ExchangeDate = stockExchangeRequest.ExchangeDate
            };
        }

        public StockExchangeResponse ToStockExchangeResponse(StockExchange stockExchange)
        {
            return new StockExchangeResponse
            {
                TickerSymbol = stockExchange.Stock.TickerSymbol,
                NumberShares = stockExchange.NumberShares,
                Price = stockExchange.Price,
                ExchangeDate = stockExchange.ExchangeDate
            };
        }

        public StockResponse ToStockResponse(StockExchange stockExchange)
        {
            return new StockResponse
            {
                TickerSymbol = stockExchange.Stock.TickerSymbol,
                Price = stockExchange.Price
            };
        }
    }
}

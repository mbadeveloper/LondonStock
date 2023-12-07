using LondonStock.Contracts;
using LondonStock.Domain;

namespace LondonStock.Converters
{
    public interface IStockConverter
    {
        StockExchange ToStockExchange(StockExchangeRequest stockExchangeRequest, int tickerSymbolId);
        StockExchangeResponse ToStockExchangeResponse(StockExchange stockExchange);
        StockResponse ToStockResponse(StockExchange stockExchange);
    }
}

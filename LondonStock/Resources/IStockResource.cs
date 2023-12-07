using LondonStock.Contracts;

namespace LondonStock.Resources
{
    public interface IStockResource
    {
        Task<StockExchangeResponse> Post(Guid brokerId, StockExchangeRequest stockExchangeRequest);
        Task<List<StockResponse>> GetAll();
        Task<StockResponse> Get(string tickerSymbol);
        Task<List<StockResponse>> List(string filter);
               
    }
}

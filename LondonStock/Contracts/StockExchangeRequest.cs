namespace LondonStock.Contracts
{
    public class StockExchangeRequest
    {
        public string TickerSymbol { get; set; }
        public decimal Price { get; set; }
        public decimal NumberShares { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}

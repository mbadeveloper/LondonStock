namespace LondonStock.Domain
{
    public class StockExchange
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public decimal Price { get; set; }
        public decimal NumberShares { get; set; }
        public DateTime ExchangeDate { get; set; }
    }
}

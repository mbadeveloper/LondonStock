using Microsoft.EntityFrameworkCore;

namespace LondonStock.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockExchange> StocksExchange { get; set; }
       
    }
}

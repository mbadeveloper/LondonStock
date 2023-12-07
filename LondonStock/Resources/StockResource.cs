using LondonStock.Contracts;
using LondonStock.Converters;
using LondonStock.Domain;
using LondonStock.Resources.Validators;
using LondonStock.Resources.Validators.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LondonStock.Resources
{
    public class StockResource : IStockResource
    {
        private readonly DatabaseContext databaseContext;
        private readonly IStockConverter stockConverter;
        private readonly IStockFilterValidatior stockFilterValidatior;

        public StockResource(DatabaseContext databaseContext,
            IStockConverter stockConverter,
            IStockFilterValidatior stockFilterValidatior)
        {
            this.databaseContext = databaseContext;
            this.stockConverter = stockConverter;
            this.stockFilterValidatior = stockFilterValidatior;
        }

        public async Task<StockExchangeResponse> Post(Guid brokerId, StockExchangeRequest stockExchangeRequest)
        {
            GeneralValidators.ValidateBrokerId(brokerId);

            if (stockExchangeRequest == null)
            {
                throw new ValidationException("Stock Exchange Reqeust cannot be null");
            }

            var broker = await databaseContext.Brokers.FirstOrDefaultAsync(b => b.Id == brokerId);
            if (broker == null)
            {
                throw new ApiClientResponseException("Broker not found", StatusCodes.Status404NotFound);
            }

            var stock = await databaseContext.Stocks.FirstOrDefaultAsync(s => s.TickerSymbol == stockExchangeRequest.TickerSymbol);
            if (stock == null)
            {
                throw new ApiClientResponseException("Stock not found", StatusCodes.Status404NotFound);
            }


            var stockExchange = stockConverter.ToStockExchange(stockExchangeRequest, stock.Id);
            databaseContext.StocksExchange.Add(stockExchange);
            await databaseContext.SaveChangesAsync();

            var newStockExchange = await GetStockExchange(stockExchange.Id);
            return stockConverter.ToStockExchangeResponse(newStockExchange);
        }

        public async Task<List<StockResponse>> GetAll()
        {
            var stocksExchange = await GetStocksExchange(null);

            return stocksExchange.Select(se => stockConverter.ToStockResponse(se)).ToList();
        }

        public async Task<StockResponse> Get(string tickerSymbol)
        {
            if (string.IsNullOrEmpty(tickerSymbol))
            {
                throw new ValidationException("Ticker Symbol cannot be null or empty");
            }

            var stockExchange = await databaseContext
                                        .StocksExchange
                                        .AsNoTracking()
                                        .Include(s => s.Stock)
                                        .OrderByDescending(stockExchange => stockExchange.ExchangeDate)
                                        .FirstOrDefaultAsync(se => se.Stock.TickerSymbol == tickerSymbol);

            if (stockExchange == null)
            {
                throw new ApiClientResponseException("Stock Exchange not exist in the list of stocks", StatusCodes.Status404NotFound);
            }

            return stockConverter.ToStockResponse(stockExchange);
        }

        public async Task<List<StockResponse>> List(string filter)
        {
            List<StockExchange> stocksExchange;

            if (string.IsNullOrEmpty(filter))
            {
                stocksExchange = await GetStocksExchange(null);
            }
            else
            {
                stockFilterValidatior.Validate(filter);

                var filterStockValues = Helpers.GetFilterValues(filter);
                var stockTickerSymbols = await databaseContext
                                                .Stocks
                                                .AsNoTracking()
                                                .Select(s => s.TickerSymbol)
                                                .ToListAsync();

                var notexistStockExchanges = filterStockValues.Except(stockTickerSymbols, StringComparer.InvariantCultureIgnoreCase);

                if(notexistStockExchanges.Any())
                {
                    throw new ValidationException($"The following stock are not valid: {string.Join(',', notexistStockExchanges)}");
                }

                stocksExchange = await GetStocksExchange(filterStockValues);
            }

            return stocksExchange.Select(se => stockConverter.ToStockResponse(se)).ToList();
        }

        private async Task<StockExchange> GetStockExchange(int stockExchangeId)
        {
            return await databaseContext
                .StocksExchange
                .AsNoTracking()
                .Include(s => s.Stock)
                .FirstOrDefaultAsync(se => se.Id == stockExchangeId);
        }

        private async Task<List<StockExchange>> GetStocksExchange(List<string>? stocksFilter)
        {
            var stocksExchange = await databaseContext
                                        .StocksExchange
                                        .AsNoTracking()
                                        .Include(se => se.Stock)
                                        .ToListAsync();

            if (stocksFilter != null && stocksFilter.Any())
            {
                stocksExchange = stocksExchange.Where(se => stocksFilter.Contains(se.Stock.TickerSymbol)).ToList();
            }

            var stockLatestValues = stocksExchange
                                        .GroupBy(se => se.StockId)
                                        .Select(se => se.OrderByDescending(stockExchange => stockExchange.ExchangeDate).First());

            return stockLatestValues.ToList();
        }
    }
}

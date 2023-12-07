using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LondonStock.Resources.Validators
{
    public class StockFilterValidatior : IStockFilterValidatior
    {
        private const string ValidFilterFormat = "Filter should be in format: tickerSymbol in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")";
        public void Validate(string filter)
        {
            if(!filter.StartsWith("tickerSymbol",StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ValidationException($"Filter is not valid. {ValidFilterFormat}");
            }

            if(filter.IndexOf("in") < 0)
            {
                throw new ValidationException("You can filter only by using the in operator");
            }

            var query = Regex.Split(filter, "in", RegexOptions.IgnoreCase).Select(v => v.Trim()).ToList();

            if (!string.Equals(query[0], "tickerSymbol", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ValidationException("You should filter by tickerSymbol");
            }

            if (query[1].StartsWith("(") && query[1].EndsWith(")"))
            {
                var values = Helpers.GetFilterValues(filter);

                if (!values.Any())
                {
                    throw new ValidationException($"Invalid filtering values. {ValidFilterFormat}");
                }
            }
            else
            {
                throw new ValidationException($"Invalid filtering values. {ValidFilterFormat}");
            }
        }
    }
}

using LondonStock.Contracts;
using LondonStock.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using System.ComponentModel.DataAnnotations;

namespace LondonStock.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/stock")]

    public class StockController : ControllerBase
    {
        private readonly IStockResource stockService;

        public StockController(IStockResource stockService)
        {
            this.stockService = stockService;
        }

        /// <summary>
        /// Allows broker to submit a share exchange.
        /// </summary>
        /// <param name="brokerId">Broker identifier.</param>
        /// <param name="stockExchangeRequest">Stock Exchange request.</param>
        /// <returns>StockExchangeResponse</returns>
        [HttpPost]
        [SwaggerOperation(OperationId = "CreateShareExchange")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json", "application/x-msgpack")]
        public async Task<ActionResult<StockExchangeResponse>> Post(Guid brokerId, [FromBody] StockExchangeRequest stockExchangeRequest)
        {
            try
            {
                var result = await stockService.Post(brokerId, stockExchangeRequest);
                return Created(String.Empty, result);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to process the share exchange");
            }
        }

        /// <summary>
        /// Allows to get the current stock value.
        /// </summary>
        /// <param name="tickerSymbol">Ticker symbol</param>
        /// <returns>The current value of the stock.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get(string tickerSymbol)
        {
            try
            {
                var result = await stockService.Get(tickerSymbol);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to return the current stock value");
            }
        }

        /// <summary>
        /// Allows to retrieve all values for all stockes on the market.
        /// </summary>
        /// <returns>List of all stockes on the market.</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "ListStocks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> List()
        {
            try
            {
                var result = await stockService.GetAll();
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to return the current stock value");
            }
        }

        /// <summary>
        /// Allows to retrieve values of selected stockes on the market.
        /// </summary>
        /// <param name="filter">
        /// Supported Filters:
        /// * `tickerSymbol` (`in`)
        /// * example
        /// * `tickerSymbol in ("tickerSymbol2","tickerSymbol5","tickerSymbol3")
        /// </param>
        /// <returns>List of selected stockes</returns>
        [HttpGet]
        [SwaggerOperation(OperationId = "ListStocks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> List([FromQuery] string filter = null)
        {
            try
            {
                var result = await stockService.List(filter);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to return the current stock value");
            }
        }
    }
}

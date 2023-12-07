using FluentAssertions;
using LondonStock.Contracts;
using LondonStock.Converters;
using LondonStock.Domain;
using LondonStock.Tests.Helpers;
using Swashbuckle.SwaggerUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonStock.Tests.Converters
{
    [TestFixture]
    public class StockConverterTests
    {
        private IStockConverter underTest;

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new StockConverter();
        }

        [Test]
        public void WhenStockExchangeConverterRunThenExpectFieldsMappedCorrectly()
        {
            var stockExchangeRequest = UnitTestHelpers.GetStockExchangeRequest1;
            var expectedResult = UnitTestHelpers.GetStockExchange1;

            var result = underTest.ToStockExchange(stockExchangeRequest, 2);

            result.Should().BeEquivalentTo(expectedResult, options => options.Excluding(x => x.Stock));
        }

        [Test]
        public void WhenStockExchangeResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var stockExchange = UnitTestHelpers.GetStockExchange1;
            var expectedResult = UnitTestHelpers.GetStockExchangeResponse1;

            var result = underTest.ToStockExchangeResponse(stockExchange);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void WhenStockResponseConverterRunThenExpectFieldsMappedCorrectly()
        {
            var stockExchange = UnitTestHelpers.GetStockExchange1;
            var expectedResult = UnitTestHelpers.GetStockResponse1;

            var result = underTest.ToStockResponse(stockExchange);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

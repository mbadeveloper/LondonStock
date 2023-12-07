using Castle.Core.Resource;
using FluentAssertions;
using LondonStock.Contracts;
using LondonStock.Converters;
using LondonStock.Domain;
using LondonStock.Resources;
using LondonStock.Resources.Validators;
using LondonStock.Resources.Validators.Exceptions;
using LondonStock.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.ComponentModel.DataAnnotations;

namespace LondonStock.Tests.Resources
{
    [TestFixture]
    public class StockResourceTests : AbstractDbTests
    {
        private IStockResource underTest;
        private readonly IStockConverter stockConverter = new StockConverter();
        private readonly Mock<IStockFilterValidatior> mockStockFilterValidatior = new Mock<IStockFilterValidatior>();

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new StockResource(databaseContext, stockConverter, mockStockFilterValidatior.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mockStockFilterValidatior.Invocations.Clear();
        }

        private static IEnumerable<TestCaseData> InvalidBrokerId() => new[]
        {
            new TestCaseData(new Guid("00000000-0000-0000-0000-000000000000"))
        };

        [Test, TestCaseSource(nameof(InvalidBrokerId))]
        public void GivenInvalidBrokerIdWhenPostShouldThrowException(Guid invalidBrokerId)
        {
            var stockExchangeRequest = UnitTestHelpers.GetStockExchangeRequest1;

            AsyncTestDelegate action = async () => { await underTest.Post(invalidBrokerId, stockExchangeRequest); };

            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Invalid brokerId");
        }

        [Test]
        public void GivenUnexistingBrokerWhenPostShouldThrowException()
        {
            var unexsitBrokerId = new Guid("d00b2203-f3c0-4e04-811b-12e253852207");
            var stockExchangeRequest = UnitTestHelpers.GetStockExchangeRequest1;

            AsyncTestDelegate action = async () => { await underTest.Post(unexsitBrokerId, stockExchangeRequest); };

            var exception = Assert.ThrowsAsync<ApiClientResponseException>(action);
            exception.Message.Should().Be("Broker not found");
            exception.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public void GivenUnexistingStockWhenPostShouldThrowException()
        {
            var stockExchangeRequest = UnitTestHelpers.GetStockExchangeRequest1;
            stockExchangeRequest.TickerSymbol = "not-exist";

            AsyncTestDelegate action = async () => { await underTest.Post(UnitTestHelpers.BrokerId1, stockExchangeRequest); };

            var exception = Assert.ThrowsAsync<ApiClientResponseException>(action);
            exception.Message.Should().Be("Stock not found");
            exception.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]
        public async Task GivenValidStockExchangeRequestWhenPostShouldReturnStockExchangeResponse()
        {
            var stockExchangeRequest = new StockExchangeRequest
            {
                TickerSymbol = UnitTestHelpers.TickerSymbol3,
                Price = 2.12M,
                NumberShares = 75,
                ExchangeDate = new DateTime(2023, 12, 1, 12, 03, 11)
            };

            var expectedStockExchangeResponse = new StockExchangeResponse
            {
                TickerSymbol = UnitTestHelpers.TickerSymbol3,
                Price = 2.12M,
                NumberShares = 75,
                ExchangeDate = new DateTime(2023, 12, 1, 12, 03, 11)
            };

            var result = await underTest.Post(UnitTestHelpers.BrokerId1, stockExchangeRequest);

            Assert.IsNotNull(result);
            result.Should().BeEquivalentTo(expectedStockExchangeResponse);
            ResetStocksExchange();
        }

        [Test, TestCaseSource(nameof(InvalidTickerSymbol))]

        public void GivenInvalidTickerSymbolWhenGetShouldThrowException(string tickerSymbol)
        {
            AsyncTestDelegate action = async () => { await underTest.Get(tickerSymbol); };

            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("Ticker Symbol cannot be null or empty");
        }

        [Test]
        public void GivenUnexistingTickerSymbolWhenGetShouldThrowException()
        {
            var unexistTickerSymbol = "not-exist";

            AsyncTestDelegate action = async () => { await underTest.Get(unexistTickerSymbol); };

            var exception = Assert.ThrowsAsync<ApiClientResponseException>(action);
            exception.Message.Should().Be("Stock Exchange not exist in the list of stocks");
            exception.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Test]

        public async Task GivenExistingTickerSymbolWhenGetShouldReturnLatestStockValue()
        {
            var expectedResult = new StockResponse
            {
                TickerSymbol = UnitTestHelpers.TickerSymbol3,
                Price = 1.19M
            };

            var result = await underTest.Get(UnitTestHelpers.TickerSymbol3);

            Assert.IsNotNull(result);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task WhenGetAllShouldReturnLatestStocksValue()
        {
            var expectedResult = new List<StockResponse>()
            {
                new StockResponse
                {
                    Price = 11.21M,
                    TickerSymbol = "TEST1"
                },
                new StockResponse
                {
                    Price = 1.19M,
                    TickerSymbol = "TEST3"
                },
                new StockResponse
                {
                    Price = 14.59M,
                    TickerSymbol = "TEST5"
                }
            };

            var result = await underTest.GetAll();

            Assert.IsNotNull(result);
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GivenUnexistingStockWhenListShouldThrowException()
        {
            var filter = $"tickerSymbol in (\"{UnitTestHelpers.TickerSymbol3}\",\"{UnitTestHelpers.TickerSymbol5}\",\"{UnitTestHelpers.TickerSymbol2}\",\"not-exist\")"; ;

            AsyncTestDelegate action = async () => { await underTest.List(filter); };

            var exception = Assert.ThrowsAsync<ValidationException>(action);
            exception.Message.Should().Be("The following stock are not valid: not-exist");
        }

        [Test]
        public async Task GivenValidFilterValuesWhenListShouldReturnCorrectStocksList()
        {
            var filter = $"tickerSymbol in (\"{UnitTestHelpers.TickerSymbol3}\",\"{UnitTestHelpers.TickerSymbol5}\",\"{UnitTestHelpers.TickerSymbol2}\")";
            var filterStocks = new List<string>()
            {
                UnitTestHelpers.TickerSymbol3,
                UnitTestHelpers.TickerSymbol5,
                UnitTestHelpers.TickerSymbol2
            };

            var expectedResult = new List<StockResponse>()
            {
                new StockResponse
                {
                    Price = 1.19M,
                    TickerSymbol = "TEST3"
                },
                new StockResponse
                {
                    Price = 14.59M,
                    TickerSymbol = "TEST5"
                }
            };

            mockStockFilterValidatior.Setup(x=>x.Validate(It.IsAny<string>()));
            var result = await underTest.List(filter);

            mockStockFilterValidatior.Verify(v => v.Validate(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<TestCaseData> InvalidTickerSymbol() => new[]
        {
            new TestCaseData(null),
            new TestCaseData("")
        };
    }
}

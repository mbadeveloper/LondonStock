using LondonStock.Contracts;
using LondonStock.Controllers;
using LondonStock.Resources;
using LondonStock.Resources.Validators.Exceptions;
using LondonStock.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace LondonStock.Tests.Controllers
{
    [TestFixture]
    public class StockControllerTests
    {
        private StockController underTest;
        private readonly Mock<IStockResource> mockStockResource = new Mock<IStockResource>();

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new StockController(mockStockResource.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mockStockResource.Invocations.Clear();
        }

        [Test]
        public async Task GivenCallsStockResourceWithCorrectParametersWhenGetShouldSuccess()
        {
            mockStockResource.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new StockResponse());

            var result = await underTest.Get(UnitTestHelpers.TickerSymbol1);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task GivenValidationFailedWhenGetShouldThrowValidationException()
        {
            string exceptionMessage = "Validation exception message";

            mockStockResource.Setup(x => x.Get(It.IsAny<string>())).Throws(new ValidationException(exceptionMessage));

            var result = await underTest.Get(UnitTestHelpers.TickerSymbol1);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenFailedWhenGetShouldThrowException()
        {
            mockStockResource.Setup(x => x.Get(It.IsAny<string>())).Throws(new Exception());

            var result = await underTest.Get(UnitTestHelpers.TickerSymbol1);

            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.AreEqual("Failed to return the current stock value", ((ObjectResult)result).Value);
        }

        [Test]
        public async Task GivenCallsStockResourceWithCorrectParametersWhenPostShouldSuccess()
        {
            mockStockResource.Setup(x => x.Post(It.IsAny<Guid>(), It.IsAny<StockExchangeRequest>())).ReturnsAsync(new StockExchangeResponse());

            var result = await underTest.Post(UnitTestHelpers.BrokerId1, UnitTestHelpers.GetStockExchangeRequest1);

            mockStockResource.Verify(gr => gr.Post(It.IsAny<Guid>(), It.IsAny<StockExchangeRequest>()), Times.Once);

            Assert.IsInstanceOf<CreatedResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)result.Result).StatusCode);
            Assert.IsInstanceOf<StockExchangeResponse>(((CreatedResult)result.Result).Value);
        }

        [Test]
        public async Task GivenValidationFailedWhenPostShouldThrowValidationException()
        {
            string exceptionMessage = "Validation exception message";

            mockStockResource.Setup(x => x.Post(It.IsAny<Guid>(), It.IsAny<StockExchangeRequest>())).Throws(new ValidationException(exceptionMessage));

            var result = await underTest.Post(UnitTestHelpers.BrokerId1, UnitTestHelpers.GetStockExchangeRequest1);

            Assert.IsInstanceOf<ObjectResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result).StatusCode);
            Assert.AreEqual(exceptionMessage, ((ObjectResult)result.Result).Value);
        }

        [Test]
        public async Task GivenFailedWhenPostShouldThrowException()
        {
            mockStockResource.Setup(x => x.Post(It.IsAny<Guid>(), It.IsAny<StockExchangeRequest>())).Throws(new Exception());

            var result = await underTest.Post(UnitTestHelpers.BrokerId1, UnitTestHelpers.GetStockExchangeRequest1);

            Assert.IsInstanceOf<ObjectResult>(result.Result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result).StatusCode);
            Assert.AreEqual("Failed to process the share exchange", ((ObjectResult)result.Result).Value);
        }
    }
}

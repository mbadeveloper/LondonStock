using FluentAssertions;
using LondonStock.Resources.Validators;
using LondonStock.Tests.Helpers;
using System.ComponentModel.DataAnnotations;

namespace LondonStock.Tests.Resources.Validators
{
    [TestFixture]
    public class StockFilterValidatiorTests
    {
        private IStockFilterValidatior underTest;

        [OneTimeSetUp]
        public void SetUp()
        {
            underTest = new StockFilterValidatior();
        }

        [Test, TestCaseSource(nameof(InvalidFilter))]
        public void GivenInvalidFilterWhenValidateShouldThrowException(string filter, string expectedException)
        {
            var stockExchangeRequest = UnitTestHelpers.GetStockExchangeRequest1;

            TestDelegate action = () => { underTest.Validate(filter); };

            var exception = Assert.Throws<ValidationException>(action);
            exception.Message.Should().Be(expectedException);
        }

        [Test, TestCaseSource(nameof(ValidFilter))]
        public void GivenValidFilterWhenValidateShouldValidationSuccess(string filter)
        {
            var stockExchangeRequest = UnitTestHelpers.GetStockExchangeRequest1;

            TestDelegate action = () => { underTest.Validate(filter); };

            Assert.DoesNotThrow(action);
        }


        private static IEnumerable<TestCaseData> InvalidFilter() => new[]
        {
            new TestCaseData("in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")","Filter is not valid. Filter should be in format: tickerSymbol in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")"),
            new TestCaseData("equal (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")","Filter is not valid. Filter should be in format: tickerSymbol in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")"),
            new TestCaseData("tickerSymbol in ()","Invalid filtering values. Filter should be in format: tickerSymbol in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")"),
            new TestCaseData("tickerSymbol in \"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\"","Invalid filtering values. Filter should be in format: tickerSymbol in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")"),
            new TestCaseData("tickerSymbol (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")","You can filter only by using the in operator")
        };

        private static IEnumerable<TestCaseData> ValidFilter() => new[]
        {
            new TestCaseData("tickerSymbol in(\"tickerSymbol2\")"),
            new TestCaseData("tickerSymbol in (\"tickerSymbol2\")"),
            new TestCaseData("tickerSymbol in (\"tickerSymbol2\",\"tickerSymbol5\",\"tickerSymbol3\")"),
            new TestCaseData("tickerSymbol in (\"tickerSymbol5\",\"tickerSymbol7\")")
        };
    }
}

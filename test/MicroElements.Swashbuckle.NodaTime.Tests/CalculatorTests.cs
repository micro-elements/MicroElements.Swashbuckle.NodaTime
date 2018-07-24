using Xunit;

namespace MicroElements.Swashbuckle.NodaTime.Tests
{
    public class CalculatorTest
    {
        [Fact]
        public void Sum()
        {
            Assert.Equal(5, Calculator.Sum(2, 3));
        }

        [Fact]
        public void Div()
        {
            Assert.Equal(3, Calculator.Div(6, 2));
        }
    }
}

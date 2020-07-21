using NUnit.Framework;

namespace Calculater.Tests
{
    [TestFixture]
    public class Tests
    {
        private Calculater _calculater;

        [OneTimeSetUp]
        public void Init()
        {
            _calculater = new Calculater();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _calculater.Close();
        }

        [TestCase("1", ExpectedResult = 1)]
        [TestCase("1+1", ExpectedResult = 2)]
        [TestCase("2*2", ExpectedResult = 4)]
        [TestCase("2/1", ExpectedResult = 2)]
        [TestCase("3/2", ExpectedResult = 1.5)]
        [TestCase("12+23-4567+8", ExpectedResult = -4524)]
        public double TestSimpleExpressions(string expression) => _calculater.Calculate(expression);

        [TestCase("(1)", ExpectedResult = 1)]
        [TestCase("10*(10+20)", ExpectedResult = 300)]
        [TestCase("(10*(10+20))", ExpectedResult = 300)]
        [TestCase("10*((5+5)+(10+10))", ExpectedResult = 300)]
        public double TestBraces(string expression) => _calculater.Calculate(expression);

        [TestCase("2+2*2", ExpectedResult = 6)]
        [TestCase("1+2*3/4", ExpectedResult = 2.5)]
        [TestCase("10*(3+(4-2)*3)", ExpectedResult = 90)]
        public double TestOperationsOrder(string expression) => _calculater.Calculate(expression);
    }
}
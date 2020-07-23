using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Calculater.Tests
{
    [TestFixture]
    [Ignore("These tests are flaky because in Win 10 I cannot find a proper way to guarantee predict when the calculator process is closed")]
    public class CalculaterTests
    {
        [Test]
        public void TestOpenClose()
        {
            var calc = new Calculater();

            Assert.NotNull(Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "Calculator"));

            calc.Close();
            Thread.Sleep(1000); // wait a bit to ensure that Calculator is closed

            Assert.Null(Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "Calculator"));
        }

        [Test]
        public void InputTest()
        {
            var calc = new Calculater();

            var result = calc.Calculate("1234567890N");
            Assert.AreEqual(-1234567890, result);

            calc.Close();
        }
    }
}
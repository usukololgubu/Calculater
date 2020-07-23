using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace Calculater.Tests
{
    [TestFixture]
    public class CalculaterTests
    {
        [Test]
        public void TestOpenClose()
        {
            var calc = new Calculater();

            Assert.NotNull(Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "Calculator"));
            
            calc.Close();
            Thread.Sleep(500); // wait a bit to be sure that Calculator is closed

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
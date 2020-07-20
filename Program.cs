using System;
using System.Linq;

namespace Calculater
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var expression = "20/(10*(4-2))";
            var calc = new Calculater();
            calc.Calculate(expression);

            Console.WriteLine($"{expression}{calc.GetCurrentResults()}");
        }
    }
}
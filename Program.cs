using System;
using System.Linq;

namespace Calculater
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var expression = "12/34*56-78+9=";
            var calc = new Calculater();
            foreach (var c in expression)
            {
                calc.Input(c);
            }

            Console.WriteLine($"{expression}{calc.GetCurrentResults()}");
        }
    }
}
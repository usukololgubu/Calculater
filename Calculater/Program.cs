using System;
using System.IO;

namespace Calculater
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var calc = new Calculater();

            using (var file = new StreamWriter("output.txt"))
            {
                foreach (var expression in File.ReadLines("input.txt"))
                {
                    var result = calc.Calculate(expression);
                    file.WriteLine(result);

                    Console.WriteLine($"{expression,-20} = {result}");
                }
            }

            calc.Close();
        }
    }
}
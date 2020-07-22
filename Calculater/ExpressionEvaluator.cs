using System;
using System.Collections.Generic;

namespace Calculater
{
    public class ExpressionEvaluator
    {
        public static T Evaluate<T>(string expression, Func<string, T> performBinaryOperation)
        {
            var stack = new Stack<string>();
            var current = "";
            foreach (var c in expression)
            {
                switch (c)
                {
                    case '(':
                        stack.Push(current);
                        current = "";
                        break;
                    case ')':
                        current = stack.Pop() + performBinaryOperation(current);
                        break;
                    default:
                        current += c;
                        break;
                }
            }

            return performBinaryOperation(current);
        }
    }
}
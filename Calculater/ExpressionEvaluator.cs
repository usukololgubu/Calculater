using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculater
{
    public class ExpressionEvaluator
    {
        private readonly List<(string value, bool isDigit)> _expression;

        private string Value => _expression.Select(p => p.value).Aggregate((c, n) => c + n);

        private ExpressionEvaluator(List<(string value, bool isDigit)> expression)
        {
            _expression = expression;
        }

        private ExpressionEvaluator(string expression)
        {
            _expression = Split(expression);
        }

        public static T Evaluate<T>(string expression, Func<string, T> performBinaryOperation)
        {
            return TraverseBraces(expression, s => EvaluateWithoutBraces(s, performBinaryOperation));
        }

        private T Evaluate<T>(Func<string, T> performBinaryOperation)
        {
            var expression = PerformNegations()
                .PerformOnlySomeOperations(new[] {'*', '/'}, performBinaryOperation);

            return performBinaryOperation(expression.Value);
        }

        private ExpressionEvaluator PerformNegations()
        {
            string ReplaceExtraNegations(string value)
            {
                var newValue = "";

                newValue = value.Replace("-", "").Replace("+", "");
                if (value.Count(c => c == '-') % 2 == 0)
                {
                    newValue = newValue == "" ? "+" : newValue;
                }
                else
                {
                    newValue += "-";
                }

                return newValue;
            }

            var result = new List<(string value, bool isDigit)>(_expression);
            for (var i = 0; i < result.Count; i++)
            {
                var current = result[i];
                if (current.isDigit)
                {
                    if (i - 1 >= 0)
                    {
                        var previous = result[i - 1];
                        if (previous.value.Length > 1 && previous.value.Contains('-'))
                        {
                            previous.value = previous.value.Replace("-", "");
                            current.value += 'N';
                        }

                        result[i - 1] = previous;
                    }
                }
                else
                {
                    current.value = ReplaceExtraNegations(current.value);
                }

                result[i] = current;
            }

            return new ExpressionEvaluator(result);
        }

        private ExpressionEvaluator PerformOnlySomeOperations<T>(char[] operators, Func<string, T> performBinaryOperation)
        {
            var expression = this;

            while (true)
            {
                var (e, i) = expression.BuildMap()
                    .Select(expression.GetBinaryExpression)
                    .Select((value, index) => (value, index))
                    .FirstOrDefault(a => a.value.IndexOfAny(operators) >= 0);

                if (e == null) break;

                expression = new ExpressionEvaluator(expression.Replace(i, performBinaryOperation(e).ToString()));
            }

            return expression;
        }

        /// <summary>
        /// Splits expression on operators and operands.
        /// </summary>
        /// <remarks>Doesn't support braces.</remarks>
        private List<(string value, bool isDigit)> Split(string expression)
        {
            var result = new List<(string value, bool isDigit)>();
            foreach (var c in expression)
            {
                var currentSymbol = (c.ToString(), isDigit: char.IsDigit(c));

                if (result.Count > 0)
                {
                    var lastElement = result[result.Count - 1];

                    // add to previous if the type is the same, ...
                    if (lastElement.isDigit == currentSymbol.isDigit)
                    {
                        lastElement.value += c;
                        result[result.Count - 1] = lastElement;
                        continue;
                    }
                }

                // ... add new otherwise
                result.Add(currentSymbol);
            }

            return result;
        }

        /// <summary>
        /// Builds a binary expressions map.
        /// </summary>
        private List<(int start, int count)> BuildMap()
        {
            var result = new List<(int start, int count)>();

            for (var i = 0; i < _expression.Count; i++)
            {
                // '-1+2+...' — take four elements ('-1+2')
                if (i == 0 && !_expression[i].isDigit && i + 4 <= _expression.Count)
                    result.Add((i, 4));
                // '3+4+...' — take tree elements ('3+4')
                else if (i + 3 <= _expression.Count) result.Add((i, 3));

                i++; // next must be operator, skip it
            }

            return result;
        }

        private string GetBinaryExpression((int start, int number) tuple)
        {
            return _expression
                .Skip(tuple.start)
                .Take(tuple.number)
                .Select(p => p.value)
                .Aggregate((c, n) => c + n);
        }

        private List<(string value, bool isDigit)> Replace(int index, string value)
        {
            var (start, number) = BuildMap()[index];

            var result = new List<(string value, bool isDigit)>(_expression);
            result.RemoveRange(start, number);
            result.Insert(start, (value, true));

            return result;
        }

        private static T TraverseBraces<T>(string expression, Func<string, T> evaluate)
        {
            var stack = new Stack<string>();
            var current = "";
            foreach (var c in expression)
                switch (c)
                {
                    case '(':
                        stack.Push(current);
                        current = "";
                        break;
                    case ')':
                        current = stack.Pop() + evaluate(current);
                        break;
                    default:
                        current += c;
                        break;
                }

            return evaluate(current);
        }

        private static T EvaluateWithoutBraces<T>(string expression, Func<string, T> performBinaryOperation)
        {
            return new ExpressionEvaluator(expression)
                .Evaluate(performBinaryOperation);
        }
    }
}
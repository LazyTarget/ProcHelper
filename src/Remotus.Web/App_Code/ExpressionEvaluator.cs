using System;
using System.Globalization;
using System.Linq;

namespace Remotus.Web
{
    public class ExpressionEvaluator
    {
        public string Resolve(string expression, object reference)
        {
            var expr = expression;
            var index = expr.IndexOf("{{");
            while (index >= 0)
            {
                var endIndex = expression.IndexOf("}}", index);
                var startIndex = index + "{{".Length;
                var length = endIndex - startIndex;
                var subject = expression.Substring(startIndex, length);

                var value = Evaluate(subject, reference);
                var valueStr = (value ?? "").ToString();
                expr = expr.Replace("{{" + subject + "}}", valueStr);
            }
            return expr;
        }

        public object Evaluate(string expression, object reference)
        {
            var expr = Resolve(expression, reference);
            if (string.IsNullOrWhiteSpace(expr))
            {
                return expr;
            }

            bool boolVal;
            long longVal;
            float floatVal;
            if (bool.TryParse(expr, out boolVal))
            {
                return boolVal;
            }
            else if (float.TryParse(expr, NumberStyles.Any, CultureInfo.InvariantCulture, out floatVal))
            {
                return floatVal;
            }
            else if (long.TryParse(expr, NumberStyles.Any, CultureInfo.InvariantCulture, out longVal))
            {
                return longVal;
            }
            else
            {
                var exprOperator = ExpressionOperator.GetNext(expr, 0);
                if (exprOperator == null || exprOperator.Operator == Operator.None)
                {
                    return expr;
                    //throw new InvalidOperationException("Invalid expression");
                }
                else if (exprOperator.Operator == Operator.Property)
                {
                    var propName = expr.Substring(0, expr.IndexOf('.'));

                    if (!string.IsNullOrWhiteSpace(propName))
                    {
                        var objectMirror = new Lux.Model.MirrorObjectModel(reference);
                        var property = objectMirror.GetProperty(propName);
                        if (property != null)
                        {
                            var remainingExpr = expr.Substring(propName.Length + "'".Length);
                            var value = property.Value;
                            var result = Evaluate(remainingExpr, value);
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid property name");
                        return null;
                    }
                }
                else
                {
                    throw new NotImplementedException($"Expression operator '{exprOperator.Operator}' has not been implemented yet...");
                }
            }
        }

        private class ExpressionOperator
        {
            public Operator Operator { get; private set; }
            public int Index { get; private set; }

            public static string GetOperatorExpression(Operator op)
            {
                switch (op)
                {
                    case Operator.Property:
                        return ".";
                    case Operator.Space:
                        return " ";
                    default:
                        return null;
                }
            }

            public static ExpressionOperator GetNext(string expresssion, int startIndex)
            {
                var operators = Enum.GetValues(typeof (Operator)).Cast<Operator>();

                var result = new ExpressionOperator
                {
                    Operator = Operator.None,
                    Index = -1,
                };
                foreach (var op in operators)
                {
                    var expr = GetOperatorExpression(op);
                    var index = expresssion.IndexOf(expr, startIndex, StringComparison.InvariantCulture);
                    if (index < 0)
                        continue;
                    if (index >= 0)
                    {
                        result.Operator = op;
                        result.Index = index;
                    }
                    if (index == 0)
                        break;
                }
                return result;
            }
        }

        private enum Operator
        {
            None,
            Space,
            Property,
            IndexorStart,
            IndexorEnd,
            MethodStart,
            MethodEnd,
            Equals,
            GreaterThan,
            GreaterOrEqualThan,
            LessThan,
            LessOrEqualThan,
            Modulus,
        }
    }
}
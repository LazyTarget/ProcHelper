using System;
using System.Collections;
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
                var endIndex = expr.IndexOf("}}", index);
                var startIndex = index + "{{".Length;
                var length = endIndex - startIndex;
                var subject = expr.Substring(startIndex, length);

                var valueStr = "";
                if (subject.StartsWith("begin:foreach", StringComparison.InvariantCultureIgnoreCase))
                {
                    var subExpr = subject.Remove(0, "begin:foreach".Length).Trim().Trim(' ', '(', ')').Trim();
                    var inIdx = subExpr.IndexOf(" in ");

                    var target = subExpr.Substring(0, inIdx).Trim();
                    var source = subExpr.Substring(inIdx + " in ".Length).Trim();
                    var sourceValue = Evaluate(source, reference);
                    var loopSource = sourceValue as IEnumerable;

                    if (loopSource != null)
                    {
                        var newRef = new Lux.Model.ObjectModel();
                        var mirror = reference as Lux.Model.MirrorObjectModel ?? new Lux.Model.MirrorObjectModel(reference);
                        //foreach (var property in mirror.GetProperties())
                        //{
                        //    newRef.DefineProperty(property.Name, property.Type, property.Value, property.ReadOnly);
                        //}

                        newRef.DefineProperty(target, null, null, false);

                        var endloopIndex = expr.IndexOf("{{end:foreach}}", endIndex);   // todo: support for nested loops
                        var loopTemplate = expr.Substring(endIndex + "}}".Length, endloopIndex - endIndex - "{{".Length);

                        foreach (var item in loopSource)
                        {
                            newRef.SetPropertyValue(target, item);

                            var itemStr = Resolve(loopTemplate, newRef);
                            valueStr += itemStr;
                        }

                        // todo: custom replace

                        continue;
                    }
                    
                }
                else if (subject.EndsWith("end:foreach", StringComparison.InvariantCultureIgnoreCase))
                {

                }
                else
                {
                    var value = Evaluate(subject, reference);
                    valueStr = (value ?? "").ToString();
                }

                expr = expr.Replace("{{" + subject + "}}", valueStr);

                index = expr.IndexOf("{{");
            }
            return expr;
        }

        public object Evaluate(string expression, object reference)
        {
            //var expr = Resolve(expression, reference);
            var expr = expression;
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
                object subReference = reference;
                int index = 0;
                while (index >= 0)
                {
                    var exprOperator = ExpressionOperator.GetNext(expr, index, includeText: false);
                    if (exprOperator.Operator == Operator.None)
                    {
                        exprOperator = ExpressionOperator.GetNext(expr, index, includeText: true);
                    }

                    if (exprOperator == null)
                    {
                        throw new InvalidOperationException("Invalid expression");
                    }

                    if (exprOperator.Index > index)
                        index = exprOperator.Index;

                    if (exprOperator.Operator == Operator.None)
                    {
                        if (index < 0)
                            throw new InvalidOperationException("Invalid expression");

                        //return expr;
                        break;
                    }
                    else if (exprOperator.Operator == Operator.Space)
                    {
                        index += exprOperator.OperatorExpr.Length;
                    }
                    else if (exprOperator.Operator == Operator.Text)
                    {
                        index += exprOperator.OperatorExpr.Length;
                        subReference = exprOperator.OperatorExpr;
                    }
                    else if (exprOperator.Operator == Operator.Reference)
                    {
                        var textOp = ExpressionOperator.GetNext(expression, exprOperator.Index + 1, includeText: true);
                        if (textOp.Operator == Operator.Text)
                        {
                            index = textOp.Index;
                            subReference = GetReferenceProperty(subReference, textOp.OperatorExpr);

                            var nextOp = ExpressionOperator.GetNext(expression, textOp.Index + 1, includeText: false);
                            if (nextOp.Index > index)
                                index = nextOp.Index;
                        }
                        else
                        {
                            index = textOp.Index;

                            //throw new InvalidOperationException("Missing reference name");
                        }
                    }
                    else if (exprOperator.Operator == Operator.Property)
                    {
                        var textOp2 = ExpressionOperator.GetNext(expression, exprOperator.Index + 1, includeText: true);
                        if (textOp2.Index > index)
                            index = textOp2.Index + (textOp2.OperatorExpr?.Length ?? 0);
                        if (textOp2.Operator == Operator.Text)
                        {
                            var propName = textOp2.OperatorExpr;
                            subReference = GetReferenceProperty(subReference, propName);
                        }
                        else
                        {
                            throw new InvalidOperationException("Missing reference name");
                        }
                    }
                    else if (exprOperator.Operator == Operator.TypeIs)
                    {
                        var op = ExpressionOperator.GetNext(expr, exprOperator.Index + exprOperator.OperatorExpr.Length, includeText: true);
                        if (op.Index > index)
                            index = op.Index + (op.OperatorExpr?.Length ?? 0);
                        if (op.Operator == Operator.Text)
                        {
                            var typeName = op.OperatorExpr;
                            var type = Type.GetType(typeName, true);
                            var cond = type.IsInstanceOfType(subReference);
                            return cond;
                        }
                        else
                        {
                            throw new InvalidOperationException("Invalid syntax. Expected text after TypeIs operator");
                        }
                    }
                    else
                    {
                        throw new NotImplementedException($"Expression operator '{exprOperator.Operator}' has not been implemented yet...");
                    }
                }
                return subReference;
            }
        }

        private object GetReferenceProperty(object reference, string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                Lux.Model.IObjectModel objectMirror = reference as Lux.Model.IObjectModel;

                if (objectMirror == null)
                {
                    objectMirror =
                        reference is Newtonsoft.Json.Linq.JObject
                            ? new JsonObjectModel((Newtonsoft.Json.Linq.JObject) reference)
                            : (reference is System.Xml.Linq.XElement
                                ? new Lux.Model.Xml.XmlObjectModel((System.Xml.Linq.XElement) reference)
                                : (Lux.Model.IObjectModel) new Lux.Model.MirrorObjectModel(reference));
                }
                
                var property = objectMirror.GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.Value;
                    return value;
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

        private class ExpressionOperator
        {
            public string OperatorExpr { get; private set; }
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
                    case Operator.TypeIs:
                        return " is ";
                    case Operator.Reference:
                        return "$";
                    case Operator.Text:
                        return "[TEXT]";
                    default:
                        return null;
                }
            }

            public static ExpressionOperator GetNext(string expression, int startIndex, bool includeText)
            {
                var operators = Enum.GetValues(typeof(Operator)).Cast<Operator>();

                var result = new ExpressionOperator
                {
                    Operator = Operator.None,
                    Index = -1,
                };
                foreach (var op in operators)
                {
                    int index;
                    var expr = GetOperatorExpression(op);
                    if (expr != null)
                    {
                        if (expr == "[TEXT]")
                        {
                            if (!includeText)
                                continue;

                            if (expression.Length <= (startIndex + 1))
                                continue;

                            var firstTextChar = expression.Skip(startIndex).FirstOrDefault(c => !Char.IsWhiteSpace(c));
                            if (firstTextChar == default(char))
                                index = startIndex;
                            else
                                index = expression.IndexOf(firstTextChar, startIndex);

                            if (firstTextChar == '"')
                            {
                                var endIndex = expression.IndexOf('"', index + 1);
                                if (endIndex > index)
                                    expr = expression.Substring(index + "\"".Length, endIndex - index - "\"".Length);
                                else
                                    expr = expression.Substring(index + "\"".Length);
                            }
                            else
                            {
                                var subOp = GetNext(expression, index + 1, includeText: false);
                                if (subOp.Index >= 0 && subOp.Index >= index)
                                {
                                    expr = expression.Substring(index, subOp.Index - index);
                                }
                                else
                                {
                                    expr = expression.Substring(index);
                                }
                            }
                        }
                        else
                        {
                            index = expression.IndexOf(expr, startIndex, StringComparison.InvariantCulture);
                            if (index < 0)
                                continue;
                        }
                    }
                    else
                    {
                        index = -1;
                    }

                    if (index >= 0)
                    {
                        if (result.Index >= 0 && index > result.Index)
                            continue;
                        //if (op == Operator.Text && result.Index >= 0)
                        //    continue;
                        if (op != Operator.Text)
                            if (result.OperatorExpr != null && result.OperatorExpr.Length >= (expr?.Length ?? 0))
                                continue;
                        if (op == Operator.Text && (result.OperatorExpr?.Length ?? 0) > 0)
                            if (expr != null && result.OperatorExpr != null && expr.StartsWith(result.OperatorExpr))
                                continue;

                        result.OperatorExpr = expr;
                        result.Operator = op;
                        result.Index = index;
                    }
                    //if (index == 0)
                    //    break;
                }
                return result;
            }
        }

        private enum Operator
        {
            None,
            Reference,
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
            TypeIs,
            Space,
            Text,
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Remotus.Base;
using Remotus.Web.Rendering;

namespace Remotus.Web.Tests.EvaluatorTests
{
    [TestFixture]
    public class ExpressionEvaluatorTests
    {
        [TestCase]
        public void ExpressionEvaluator_EvaluateBool()
        {
            var expected = true;
            var expression = expected.ToString();

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, null);
            Assert.IsNotNull(result);

            var actual = (bool)result;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void ExpressionEvaluator_EvaluateFloat()
        {
            var expected = 212.73f;
            var expression = expected.ToString(CultureInfo.InvariantCulture);

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, null);
            Assert.IsNotNull(result);

            var actual = (float)result;
            Assert.AreEqual(expected, actual);
        }
        
        [TestCase]
        public void ExpressionEvaluator_EvaluateInt()
        {
            int expected = 212;
            var expression = expected.ToString(CultureInfo.InvariantCulture);

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, null);
            Assert.IsNotNull(result);

            var actual = (int)result;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void ExpressionEvaluator_EvaluateLong()
        {
            long expected = long.MaxValue - 239;
            var expression = expected.ToString(CultureInfo.InvariantCulture);

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, null);
            Assert.IsNotNull(result);

            var actual = (long)result;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void ExpressionEvaluator_EvaluateTypeIs()
        {
            var value = new ProcessDto();
            var expected = value is ProcessDto;
            var expression = $"$Value is \"{typeof(ProcessDto).AssemblyQualifiedName}\"";
            var reference = new {Value = value};

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, reference);
            Assert.IsNotNull(result);

            var actual = (bool)result;
            Assert.AreEqual(expected, actual);
        }

        [TestCase, Ignore("Not implemented")]
        public void ExpressionEvaluator_EvaluateTypeIsNot()
        {
            var value = new ProcessDto();
            var expected = false;
            var expression = $"$Value is \"{typeof(ProcessDto).AssemblyQualifiedName}\" = false";
            var reference = new { Value = value };

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, reference);
            Assert.IsNotNull(result);

            var actual = (bool)result;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void ExpressionEvaluator_EvaluateProperties()
        {
            var value = new ProcessDto
            {
                MainModule = new ProcessModuleDto
                {
                    FileName = "Qwerty.exe",
                },
            };
            var expected = value.MainModule.FileName;
            var expression = "$Value.MainModule.FileName";
            var reference = new {Value = value};

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, reference);
            Assert.IsNotNull(result);

            var actual = (string)result;
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void ExpressionEvaluator_ConditionEvaluator()
        {
            var expected = "Qwerty";
            var expression = $"$Value == \"{expected}\"";

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, new {Value = expected});
            Assert.IsNotNull(result);

            var actual = (string) result;
            Assert.AreEqual(expected, actual);
        }

        
        [TestCase]
        public void ExpressionEvaluator_PropertyResolver()
        {
            var value = new ProcessDto
            {
                Id = 21323,
                ProcessName = "ProcName",
            };
            
            var sb = new StringBuilder();
            sb.AppendLine("<div>");
            sb.AppendLine("\t<p class=\"Property\"><span class=\"PropertyName\">ProcessID: </span><span class=\"PropertyValue\">{{Value.Id}}</span></p>");
            sb.AppendLine("\t<p class=\"Property\"><span class=\"PropertyName\">ProcessName: </span><span class=\"PropertyValue\">{{Value.ProcessName}}</span></p>");
            sb.AppendLine("</div>");
            var expression = sb.ToString();


            var expected = expression
                .Replace("{{Value.Id}}", value.Id.ToString())
                .Replace("{{Value.ProcessName}}", value.ProcessName);

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Resolve(expected, new {Value = value});
            Assert.IsNotNull(result);

            var actual = (string) result;
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void ExpressionEvaluator_EvaluateMethod()
        {
            var calc = new Calculator();
            var reference = new
            {
                Calculator = calc,
            };
            var expected = calc.Add(10, 35);
            var expression = "$Calculator.Add(10, 35)";

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, reference);
            Assert.IsNotNull(result);

            var actual = (int)result;
            Assert.AreEqual(expected, actual);
        }

        [TestCase]
        public void ExpressionEvaluator_EvaluateNestedMethods()
        {
            var calc = new Calculator();
            var reference = new
            {
                Calculator = calc,
            };
            var expected = calc.Add(10, calc.Subtract(35, 15));
            var expression = "$Calculator.Add(10, $Calculator.Subtract(35, 15))";

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(expression, reference);
            Assert.IsNotNull(result);

            var actual = (int)result;
            Assert.AreEqual(expected, actual);
        }


        protected class Calculator
        {
            public int Round(decimal d)
            {
                return (int) decimal.Round(d);
            }

            public int Add(int x, int y)
            {
                return x + y;
            }

            public int Subtract(int x, int y)
            {
                return x - y;
            }
        }
    }
}

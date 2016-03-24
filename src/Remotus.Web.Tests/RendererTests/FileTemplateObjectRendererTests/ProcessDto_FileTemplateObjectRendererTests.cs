using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remotus.Base;
using Remotus.Web.Rendering;

namespace Remotus.Web.Tests.RendererTests.FileTemplateObjectRendererTests
{
    [TestFixture, Ignore("Not implemented")]
    public class ProcessDto_FileTemplateObjectRendererTests : FileTemplateObjectRendererTestBase
    {
        public override FileTemplateObjectRenderer.IFileTemplate GetFileTemplate()
        {
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RendererTests/FileTemplateObjectRendererTests/Templates");
            var filePath = Path.Combine(baseDir, "ProcessDto.render");
            var fileContents = File.ReadAllText(filePath);
            var template = FileTemplateObjectRenderer.FileTemplate.Parse(fileContents);
            return template;
        }


        [TestCase]
        public void CouldParseTemplate()
        {
            var template = GetFileTemplate();
            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Conditions);
            Assert.IsTrue(template.Conditions.Length > 0);
            Assert.IsNotEmpty(template.RawTemplate);
        }


        [TestCase]
        public void ReturnsTrueForValidCondition()
        {
            object obj = new ProcessDto();

            var sut = GetSUT();
            var actual = sut.CanRender(obj);
            Assert.AreEqual(true, actual);
        }

        [TestCase]
        public void ReturnsFalseForInvalidCondition()
        {
            object obj = new Parameter();

            var sut = GetSUT();
            var actual = sut.CanRender(obj);
            Assert.AreEqual(false, actual);
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

            var evaluator = new FileTemplateObjectRenderer.ExpressionEvaluator();
            var result = evaluator.Evaluate(expected, new {Value = value});
            Assert.IsNotNull(result);

            var actual = (string) result;
            Assert.AreEqual(expected, actual);
        }



        [TestCase]
        public void Render_WhenValid()
        {
            object value = new ProcessDto
            {
                Id = 21323,
                ProcessName = "ProcName",
            };
            
            var sb = new StringBuilder();
            sb.AppendLine("<div>");
            sb.AppendLine("\t<p class=\"Property\"><span class=\"PropertyName\">ProcessID: </span><span class=\"PropertyValue\">{{Value.Id}}</span></p>");
            sb.AppendLine("\t<p class=\"Property\"><span class=\"PropertyName\">ProcessName: </span><span class=\"PropertyValue\">{{Value.ProcessName}}</span></p>");
            sb.AppendLine("</div>");

            var evaluator = new FileTemplateObjectRenderer.ExpressionEvaluator();
            var expected = sb.ToString();
            var val = evaluator.Evaluate(expected, new {Value = value});
            expected = (string) val;

            sb.Clear();
            TextWriter textWriter = new StringWriter(sb);

            IObjectRenderer sut = GetSUT();
            sut.Render(textWriter, value);

            var actual = sb.ToString();
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void Render_WhenInvalid()
        {
            object value = new Parameter();
            
            var sb = new StringBuilder();
            TextWriter textWriter = new StringWriter(sb);

            IObjectRenderer sut = GetSUT();
            sut.Render(textWriter, value);

            var actual = sb.ToString();
            Assert.IsEmpty(actual);
        }

    }
}

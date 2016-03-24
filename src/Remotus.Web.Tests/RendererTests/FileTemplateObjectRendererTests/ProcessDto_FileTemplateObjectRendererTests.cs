using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Remotus.Base;
using Remotus.Web.Rendering;

namespace Remotus.Web.Tests.RendererTests.FileTemplateObjectRendererTests
{
    [TestFixture]
    public class ProcessDto_FileTemplateObjectRendererTests : FileTemplateObjectRendererTestBase
    {
        public override FileTemplateObjectRenderer.IFileTemplate GetFileTemplate()
        {
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RendererTests/FileTemplateObjectRendererTests/Templates");
            var filePath = Path.Combine(baseDir, "ProcessDto.render");
            var template = FileTemplateObjectRenderer.FileTemplate.Load(filePath);
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
        public void Render_WhenValid()
        {
            object value = new ProcessDto
            {
                Id = 21323,
                ProcessName = "ProcName",
            };
            
            var template = GetFileTemplate();
            var rawTemplate = template.RawTemplate.Trim();

            var evaluator = new ExpressionEvaluator();
            var val = evaluator.Resolve(rawTemplate, value);
            var expected = val.Trim();

            var sb = new StringBuilder();
            TextWriter textWriter = new StringWriter(sb);
            IObjectRenderer sut = GetSUT();

            // Act
            var canRender = sut.CanRender(value);
            Assert.AreEqual(true, canRender);
            
            sut.Render(textWriter, value);

            var actual = sb.ToString().Trim();
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public void Render_WhenInvalid()
        {
            object value = new Parameter();

            var template = GetFileTemplate();
            var rawTemplate = template.RawTemplate.Trim();

            var evaluator = new ExpressionEvaluator();
            var val = evaluator.Resolve(rawTemplate, value);
            var expected = val.Trim();

            var sb = new StringBuilder();
            TextWriter textWriter = new StringWriter(sb);
            IObjectRenderer sut = GetSUT();

            // Act
            var canRender = sut.CanRender(value);
            Assert.AreEqual(false, canRender);

            sut.Render(textWriter, value);

            var actual = sb.ToString().Trim();
            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

    }
}

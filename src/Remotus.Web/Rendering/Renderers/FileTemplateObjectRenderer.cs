using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Remotus.Web.Rendering
{
    public class FileTemplateObjectRenderer : IObjectRenderer
    {
        protected readonly IFileTemplate Template;
        protected readonly ExpressionEvaluator Evaluator = new ExpressionEvaluator();

        public FileTemplateObjectRenderer(IFileTemplate fileTemplate)
        {
            if (fileTemplate == null)
                throw new ArgumentNullException(nameof(fileTemplate));
            Template = fileTemplate;
        }

        public bool CanRender(object value)
        {
            var result = ValidateTemplateConditions(value);
            return result;
        }

        public void Render(TextWriter textWriter, object value)
        {
            var resolved = Evaluator.Resolve(Template.RawTemplate, value);
            textWriter.Write(resolved);
        }


        protected virtual bool ValidateTemplateConditions(object value)
        {
            if (Template.Conditions == null)
                return true;

            foreach (var objectRendererCondition in Template.Conditions)
            {
                if (objectRendererCondition == null)
                    continue;
                var res = objectRendererCondition.Validate(value, Evaluator);
                if (!res)
                    return false;
            }
            return true;
        }


        public interface IFileTemplate
        {
            string RawTemplate { get; }
            IObjectRendererCondition[] Conditions { get; }
        }

        public interface IObjectRendererCondition
        {
            string Expression { get; }
            bool Validate(object value, ExpressionEvaluator evaluator);
        }

        protected class ObjectRendererCondition : IObjectRendererCondition
        {
            public ObjectRendererCondition(string expression)
            {
                Expression = expression;
            }

            public string Expression { get; }

            public bool Validate(object value, ExpressionEvaluator evaluator)
            {
                var result = evaluator.Evaluate(Expression, value);
                var cond = (bool) result;
                return cond;
            }
        }

        

        public class FileTemplate : IFileTemplate
        {
            private FileTemplate()
            {

            }

            public string RawTemplate { get; private set; }
            public IObjectRendererCondition[] Conditions { get; private set; }


            public static FileTemplate Load(string filePath)
            {
                filePath = Path.IsPathRooted(filePath)
                    ? filePath
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                var fileLines = File.ReadAllLines(filePath);
                var template = Parse(fileLines);
                return template;
            }

            public static FileTemplate Parse(string[] fileLines)
            {
                var conditions = new List<IObjectRendererCondition>();

                var lineIndex = 0;
                string line = fileLines.ElementAtOrDefault(lineIndex);
                while (!string.IsNullOrWhiteSpace(line))
                {
                    line = line.Trim();
                    if (line.StartsWith("[Cond]", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var expression = line.Substring("[Cond]".Length).Trim();
                        var cond = new ObjectRendererCondition(expression);
                        conditions.Add(cond);
                    }
                    else
                    {

                    }

                    line = fileLines.ElementAt(++lineIndex);
                }

                var rawTemplate = string.Join(Environment.NewLine, fileLines.Skip(lineIndex + 1));

                var template = new FileTemplate();
                template.Conditions = conditions.ToArray();
                template.RawTemplate = rawTemplate;
                return template;
            }
        }
    }
}
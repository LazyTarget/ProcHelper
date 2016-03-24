using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remotus.Web.Rendering
{
    public class FileTemplateObjectRenderer : HtmlObjectRenderer
    {
        protected readonly IFileTemplate Template;
        protected readonly ExpressionEvaluator Evaluator = new ExpressionEvaluator();

        public FileTemplateObjectRenderer(IFileTemplate fileTemplate)
        {
            if (fileTemplate == null)
                throw new ArgumentNullException(nameof(fileTemplate));
            Template = fileTemplate;
        }

        public override bool CanRender(object value)
        {
            var result = ValidateTemplateConditions(value);
            return result;

            //return result = base.CanRender(value);
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
                throw new NotImplementedException();
            }
        }

        public class ExpressionEvaluator
        {
            public object Evaluate(string expression, object reference)
            {
                return null;
            }
        }

        public class FileTemplate : IFileTemplate
        {
            private FileTemplate()
            {
                
            }

            public string RawTemplate { get; private set; }
            public IObjectRendererCondition[] Conditions { get; private set; }


            public static FileTemplate Parse(string fileContents)
            {
                var conditions = new List<IObjectRendererCondition>();

                var lineIndex = 0;
                var lines = fileContents.Split(Environment.NewLine.ToCharArray());
                string line = lines.ElementAtOrDefault(lineIndex);
                while (!string.IsNullOrWhiteSpace(line))
                {
                    line = line.Trim();
                    if (line.StartsWith("[Cond]", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var expression = line.Substring(0, "[Cond]".Length).Trim();
                        var cond = new ObjectRendererCondition(expression);
                        conditions.Add(cond);
                    }
                    else
                    {
                        
                    }

                    line = lines.ElementAt(lineIndex++);
                }

                var rawTemplate = string.Join(Environment.NewLine, lines.Skip(lineIndex + 1));

                var template = new FileTemplate();
                template.Conditions = conditions.ToArray();
                template.RawTemplate = rawTemplate;
                return template;
            }
        }
    }
}
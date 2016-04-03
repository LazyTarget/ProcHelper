using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Remotus.Web.Rendering
{
    public class FileTemplateObjectRenderer : IObjectRenderer
    {
        public readonly IFileTemplate Template;
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
            string FilePath { get; }
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

        

        public class FileTemplate : IFileTemplate, IDisposable
        {
            private FileSystemWatcher _watcher;

            public FileTemplate()
            {

            }

            public string FilePath { get; private set; }
            public string RawTemplate { get; private set; }
            public IObjectRendererCondition[] Conditions { get; private set; }



            private void WatcherOnEvent(object sender, FileSystemEventArgs args)
            {
                Load(args.FullPath, watch: true);
            }

            public void Load(string filePath, bool watch = true)
            {
                filePath = Path.IsPathRooted(filePath)
                    ? filePath
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
                if (!File.Exists(filePath))
                {
                    return;
                }

                var fileLines = new List<string>();
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        fileLines.Add(line);
                    }
                }
                FilePath = filePath;
                LoadFile(fileLines.ToArray());


                _watcher?.Dispose();
                if (watch)
                {
                    var dir = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileName(filePath);
                    _watcher = new FileSystemWatcher(dir, fileName);
                    _watcher.Changed += WatcherOnEvent;
                    _watcher.EnableRaisingEvents = true;
                }
            }


            private void LoadFile(string[] fileLines)
            {
                if (fileLines == null)
                {
                    Conditions = new IObjectRendererCondition[0];
                    RawTemplate = null;
                    return;
                }
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
                
                Conditions = conditions.ToArray();
                RawTemplate = rawTemplate;
            }

            public void Dispose()
            {
                _watcher?.Dispose();
                _watcher = null;
            }
        }
    }
}
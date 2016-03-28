using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remotus.Base.Scripting
{
    public class ScriptExecutor
    {
        public IExecutionContext Context { get; set; }

        public async Task<IResponseBase> Execute(Script script)
        {
            var results = new List<IResponseBase>();
            IResponseBase result = DefaultResponseBase.Create<IEnumerable<IResponseBase>>(results);

            try
            {
                foreach (var task in script.Tasks)
                {
                    var res = await task.Execute(Context);
                    results.Add(res);
                }
            }
            catch (Exception ex)
            {
                result = DefaultResponseBase.CreateError(DefaultError.FromException(ex));
            }
            return result;
        }
    }
}
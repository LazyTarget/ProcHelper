using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class GetProcessesFunction : IFunctionDescriptor, IFunction<IList<ProcessDto>>
    {
        private IProcessFinder _processFinder;

        public GetProcessesFunction()
        {
            _processFinder = new ProcessFinder();
        }

        public string Name => nameof(GetProcessesFunction);

        public IParameterCollection GetParameters()
        {
            var res = new ParameterCollection();
            res[ParameterKeys.ProcessName] = new Parameter
            {
                Name = ParameterKeys.ProcessName,
                Required = false,
                Type = typeof(string),
                Value = null,
            };
            return res;
        }

        public IFunction Instantiate()
        {
            return this;
        }

        async Task<IFunctionResult> IFunction.Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            var result = await Execute(context, arguments);
            return result;
        }

        public async Task<IFunctionResult<IList<ProcessDto>>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                IEnumerable<ProcessDto> enumerable;
                var processName = arguments?.Parameters.GetParamValue<string>(ParameterKeys.ProcessName);
                if (!string.IsNullOrEmpty(processName))
                    enumerable = _processFinder.GetProcessesByName(processName);
                else
                    enumerable = _processFinder.GetProcesses();

                var list = enumerable.ToList();
                var result = new FunctionResult<IList<ProcessDto>>
                {
                    Arguments = arguments,
                    Result = list,
                };
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<IList<ProcessDto>>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public static class ParameterKeys
        {
            public const string ProcessID = "ProcessID";
            public const string ProcessName = "ProcessName";
        }

        public void Dispose()
        {
            
        }
    }
}

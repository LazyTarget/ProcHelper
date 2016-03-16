using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl;
using FullCtrl.Base;

namespace Remotus.Plugins.Process
{
    public class GetProcessFunction : IFunctionDescriptor, IFunction<ProcessDto>
    {
        private IProcessFinder _processFinder;

        public GetProcessFunction()
        {
            _processFinder = new ProcessFinder();
        }

        public string Name => nameof(GetProcessFunction);

        public IParameterCollection GetParameters()
        {
            var res = new ParameterCollection();
            res[ParameterKeys.ProcessID] = new Parameter
            {
                Name = ParameterKeys.ProcessID,
                Required = true,
                Type = typeof(int),
                Value = null,
            };
            //res[ParameterKeys.ProcessName] = new Parameter
            //{
            //    Name = ParameterKeys.ProcessName,
            //    Required = false,
            //    Type = typeof(string),
            //    Value = null,
            //};
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

        public async Task<IFunctionResult<ProcessDto>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var processID = arguments.Parameters.GetParamValue<int>(ParameterKeys.ProcessID);
                var proc = _processFinder.GetProcess(processID);
                
                var result = new FunctionResult<ProcessDto>
                {
                    Arguments = arguments,
                    Result = proc,
                };
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<ProcessDto>();
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

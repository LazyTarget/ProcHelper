using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class KillProcessFunction : IFunction<ProcessDto>
    {
        private IProcessHelper _processHelper;

        public KillProcessFunction()
        {
            _processHelper = new ProcessHelper();
        }

        public IFunctionDescriptor GetDescriptor()
        {
            return new Descriptor();
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
                var proc = _processHelper.KillProcess(processID);
                
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


        public class Descriptor : IFunctionDescriptor
        {
            public string Name => nameof(KillProcessFunction);

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
                return res;
            }

            public IFunction Instantiate()
            {
                return new KillProcessFunction();
            }
        }


        public static class ParameterKeys
        {
            public const string ProcessID = "ProcessID";
        }

        public void Dispose()
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class StartProcessFunction : IFunctionDescriptor, IFunction<ProcessDto>
    {
        private static System.Diagnostics.Process _proc;
        private IProcessHelper _processHelper;

        public StartProcessFunction()
        {
            _processHelper = new ProcessHelper();
        }

        public string Name => nameof(StartProcessFunction);

        public IParameterCollection GetParameters()
        {
            var res = new ParameterCollection();
            res[ParameterKeys.FileName] = new Parameter
            {
                Name = ParameterKeys.FileName,
                Required = true,
                Type = typeof(string),
                Value = null,
            };
            res[ParameterKeys.Arguments] = new Parameter
            {
                Name = ParameterKeys.Arguments,
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

        public async Task<IFunctionResult<ProcessDto>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var fileName = arguments.Parameters.GetParamValue<string>(ParameterKeys.FileName);
                var procArgs = arguments.Parameters.GetParamValue<string>(ParameterKeys.Arguments);
                var res = _processHelper.StartProcess(fileName, procArgs);
                var proc = res.GetBase();
                _proc = proc;
                proc.EnableRaisingEvents = true;
                proc.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
                {
                    context?.Logger?.Info("ProcMsg: " + args.Data);
                };
                
                var result = new FunctionResult<ProcessDto>
                {
                    Arguments = arguments,
                    Result = res,
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
            public const string FileName = "FileName";
            public const string Arguments = "Arguments";
        }

        public void Dispose()
        {
            
        }
    }
}

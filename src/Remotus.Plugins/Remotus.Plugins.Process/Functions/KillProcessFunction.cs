using System;
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
                //var processID = arguments.Parameters.GetParamValue<int>(ParameterKeys.ProcessID);
                var processID = arguments?.Parameters.GetOrDefault<int>(ParameterKeys.ProcessID)?.Value ?? -1;
                if (processID < 0)
                {
                    throw new ArgumentException($"Invalid ProcessID '{processID}'", nameof(arguments));
                }
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
            public string ID => "1B6FC85B-E86D-43F4-B16D-C99BC3810F8C";
            public string Name => nameof(KillProcessFunction);
            public string Version => "1.0.0.0";

            IParameterCollection IFunctionDescriptor.GetParameters()
            {
                return GetParameters();
            }

            public Parameters GetParameters()
            {
                var res = new Parameters();
                return res;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<ProcessDto> Instantiate()
            {
                return new KillProcessFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                ProcessID = new Parameter<int>
                {
                    Name = ParameterKeys.ProcessID,
                    Required = true,
                    Type = typeof(int),
                    Value = default(int)
                };
            }

            public IParameter<int> ProcessID
            {
                get { return this.GetOrDefault<int>(ParameterKeys.ProcessID); }
                private set { this[ParameterKeys.ProcessID] = value; }
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

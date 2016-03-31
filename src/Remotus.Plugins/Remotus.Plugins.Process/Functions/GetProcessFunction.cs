using System;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class GetProcessFunction : IFunction<ProcessDto>
    {
        private IProcessFinder _processFinder;

        public GetProcessFunction()
        {
            _processFinder = new ProcessFinder();
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

                var res = _processFinder.GetProcess(processID);
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


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "7EA18317-D894-4145-A704-7E8197EC65F6";
            public string Name => "Get process";
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
                return new GetProcessFunction();
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

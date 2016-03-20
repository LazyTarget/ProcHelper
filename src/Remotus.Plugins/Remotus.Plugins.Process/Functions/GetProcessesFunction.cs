using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class GetProcessesFunction : IFunction<IList<ProcessDto>>
    {
        private IProcessFinder _processFinder;

        public GetProcessesFunction()
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

        public async Task<IFunctionResult<IList<ProcessDto>>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                IEnumerable<ProcessDto> enumerable;
                var processName = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.ProcessName)?.Value;
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


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "10924BB2-09BF-4C5C-A120-2D7649C36D70";
            public string Name => nameof(GetProcessesFunction);
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

            public IFunction<IList<ProcessDto>> Instantiate()
            {
                return new GetProcessesFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                ProcessName = new Parameter<string>
                {
                    Name = ParameterKeys.ProcessName,
                    Required = false,
                    Type = typeof(string),
                    Value = null,
                };
            }

            public IParameter<string> ProcessName
            {
                get { return this.GetOrDefault<string>(ParameterKeys.ProcessName); }
                private set { this[ParameterKeys.ProcessName] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string ProcessName = "ProcessName";
        }

        public void Dispose()
        {
            
        }
    }
}

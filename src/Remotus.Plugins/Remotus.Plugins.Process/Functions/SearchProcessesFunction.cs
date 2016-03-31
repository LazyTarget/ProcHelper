using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class SearchProcessesFunction : IFunction<IList<ProcessDto>>
    {
        private IProcessFinder _processFinder;

        public SearchProcessesFunction()
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
                List<ProcessDto> list;
                var query = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.Query)?.Value;
                if (!string.IsNullOrEmpty(query))
                {
                    list = new List<ProcessDto>();
                    List<ProcessDto> all = null;

                    var parts = query.Split(new[] { ',', ';' });
                    foreach (var p in parts)
                    {
                        var q = p.Trim();
                        var exact = q.StartsWith("\"") && q.EndsWith("\"");
                        if (exact)
                            list.AddRange(_processFinder.GetProcessesByName(q.Trim('"')));
                        else
                        {
                            if (all == null)
                                all = _processFinder.GetProcesses().ToList();
                            var matches = all.Where(x =>
                                                    x.ProcessName.IndexOf(q, StringComparison.InvariantCultureIgnoreCase) >= 0)
                                             .ToList();
                            list.AddRange(matches);
                        }
                    }
                }
                else
                    list = _processFinder.GetProcesses().ToList();
                
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
            public string ID => "3362B21A-3235-4F64-94C7-AB3A7A31BA08";
            public string Name => "Search processes";
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
                return new SearchProcessesFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                Query = new Parameter<string>
                {
                    Name = ParameterKeys.Query,
                    Required = false,
                    Type = typeof(string),
                    Value = null,
                };
            }

            public IParameter<string> Query
            {
                get { return this.GetOrDefault<string>(ParameterKeys.Query); }
                private set { this[ParameterKeys.Query] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string Query = "Query";
        }

        public void Dispose()
        {
            
        }
    }
}

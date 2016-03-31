using System;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Process
{
    public class StartProcessFunction : IFunction<ProcessDto>
    {
        private IProcessHelper _processHelper;

        public StartProcessFunction()
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
                var fileName = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.FileName)?.Value;
                var procArgs = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.Arguments)?.Value;
                var workingDirectory = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.WorkingDirectory)?.Value;
                var redirectStOutput = arguments?.Parameters.GetOrDefault<bool>(ParameterKeys.RedirectStOutput)?.Value ?? false;
                Credentials credentials = null;

                var res = _processHelper.StartProcess(fileName, procArgs, workingDirectory, redirectStOutput, credentials);
                //var proc = res.GetBase();
                //_proc = proc;
                //proc.EnableRaisingEvents = true;
                //proc.OutputDataReceived += delegate(object sender, DataReceivedEventArgs args)
                //{
                //    context?.Logger?.Info("ProcMsg: " + args.Data);
                //};
                
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
            public string ID => "EF39CE7D-EE2D-47FE-AB76-50D185AAB0FB";
            public string Name => "Start process";
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
                return new StartProcessFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                FileName = new Parameter<string>
                {
                    Name = ParameterKeys.FileName,
                    Required = true,
                    Type = typeof(string),
                    Value = null,
                };
                Arguments = new Parameter<string>
                {
                    Name = ParameterKeys.Arguments,
                    Required = false,
                    Type = typeof(string),
                    Value = null,
                };
                WorkingDirectory = new Parameter<string>
                {
                    Name = ParameterKeys.WorkingDirectory,
                    Required = false,
                    Type = typeof(string),
                    Value = null,
                };
                RedirectStOutput = new Parameter<bool>
                {
                    Name = ParameterKeys.RedirectStOutput,
                    Required = false,
                    Type = typeof(bool),
                    Value = false,
                };
            }

            public IParameter<string> FileName
            {
                get { return this.GetOrDefault<string>(ParameterKeys.FileName); }
                private set { this[ParameterKeys.FileName] = value; }
            }

            public IParameter<string> Arguments
            {
                get { return this.GetOrDefault<string>(ParameterKeys.Arguments); }
                private set { this[ParameterKeys.Arguments] = value; }
            }

            public IParameter<string> WorkingDirectory
            {
                get { return this.GetOrDefault<string>(ParameterKeys.WorkingDirectory); }
                private set { this[ParameterKeys.WorkingDirectory] = value; }
            }

            public IParameter<bool> RedirectStOutput
            {
                get { return this.GetOrDefault<bool>(ParameterKeys.RedirectStOutput); }
                private set { this[ParameterKeys.RedirectStOutput] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string FileName = "FileName";
            public const string Arguments = "Arguments";
            public const string WorkingDirectory = "WorkingDirectory";
            public const string RedirectStOutput = "RedirectStOutput";
        }

        public void Dispose()
        {
            
        }
    }
}

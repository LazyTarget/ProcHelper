using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput.Native;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class WriteTextFunction : IFunction, IFunction<KeyResponse>
    {
        public WriteTextFunction()
        {
            
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

        public async Task<IFunctionResult<KeyResponse>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var text = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.Text)?.Value;

                // Work
                KeyboardInputPlugin.InputSimulator.Keyboard.TextEntry(text);

                KeyResponse res = null;

                var result = new FunctionResult<KeyResponse>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<KeyResponse>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "AACEB5FA-C958-4830-B882-EC9011A3F82B";
            public string Name => "Write text";
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

            public IFunction<KeyResponse> Instantiate()
            {
                return new WriteTextFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                VirtualKeyCode = new Parameter<string>
                {
                    Name = ParameterKeys.Text,
                    Required = true,
                    Type = typeof(string),
                    Value = null,
                };
            }

            public IParameter<string> VirtualKeyCode
            {
                get { return this.GetOrDefault<string>(ParameterKeys.Text); }
                private set { this[ParameterKeys.Text] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string Text = "Text";
        }

        public void Dispose()
        {
            
        }
    }
}

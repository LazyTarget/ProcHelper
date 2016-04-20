using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput.Native;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class InvokeKeyDownFunction : IFunction, IFunction<KeyResponse>
    {
        public InvokeKeyDownFunction()
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
                var virtualKeyCode = arguments?.Parameters.GetOrDefault<VirtualKeyCode>(ParameterKeys.VirtualKeyCode)?.Value;
                if (!virtualKeyCode.HasValue)
                {
                    throw new ArgumentException("Invalid VirtualKeyCode", nameof(arguments));
                }

                // Work
                KeyboardInputPlugin.InputSimulator.Keyboard.KeyDown(virtualKeyCode.Value);

                var res = KeyboardInputPlugin.GetKeyInfo(virtualKeyCode.Value);

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
            public string ID => "F7F52CD2-E29B-4459-8FCC-6E707CBA7235";
            public string Name => "Invoke key down";
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
                return new InvokeKeyDownFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                VirtualKeyCode = new Parameter<VirtualKeyCode>
                {
                    Name = ParameterKeys.VirtualKeyCode,
                    Required = false,
                    Type = typeof(VirtualKeyCode),
                    Value = default(VirtualKeyCode),
                };
            }

            public IParameter<VirtualKeyCode> VirtualKeyCode
            {
                get { return this.GetOrDefault<VirtualKeyCode>(ParameterKeys.VirtualKeyCode); }
                private set { this[ParameterKeys.VirtualKeyCode] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string VirtualKeyCode = "VirtualKeyCode";
        }

        public void Dispose()
        {
            
        }
    }
}

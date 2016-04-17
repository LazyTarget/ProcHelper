using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput.Native;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class GetKeyInfoFunction : IFunction, IFunction<KeyResponse>
    {
        public GetKeyInfoFunction()
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

                var isDown = KeyboardInputPlugin.InputSimulator.InputDeviceState.IsKeyDown(virtualKeyCode.Value);

                var res = new KeyResponse
                {
                    VirtualKeyCode = virtualKeyCode.Value,
                    IsDown = isDown,
                };

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
            public string ID => "22287AD3-56A5-49A1-B6CC-99BE9947A832";
            public string Name => "Get key info";
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
                return new GetKeyInfoFunction();
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

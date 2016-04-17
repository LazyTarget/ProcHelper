using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class MoveMouseByFunction : IFunction, IFunction<MousePosition>
    {
        public MoveMouseByFunction()
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

        public async Task<IFunctionResult<MousePosition>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var deltaX = arguments?.Parameters.GetOrDefault<int>(ParameterKeys.DeltaX)?.Value;
                var deltaY = arguments?.Parameters.GetOrDefault<int>(ParameterKeys.DeltaY)?.Value;

                MouseInputPlugin.InputSimulator.Mouse.MoveMouseBy(deltaX.GetValueOrDefault(), deltaY.GetValueOrDefault());
                var point = Win32.GetCursorPosition();
                var res = new MousePosition
                {
                    Position = point,
                };

                var result = new FunctionResult<MousePosition>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<MousePosition>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "827BD2B9-BADB-4E91-A688-DA5A8C693E95";
            public string Name => "Move mouse by";
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

            public IFunction<MousePosition> Instantiate()
            {
                return new MoveMouseByFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                DeltaX = new Parameter<int>
                {
                    Name = ParameterKeys.DeltaX,
                    Required = false,
                    Type = typeof(int),
                    Value = default(int),
                };
                DeltaY = new Parameter<int>
                {
                    Name = ParameterKeys.DeltaY,
                    Required = false,
                    Type = typeof(int),
                    Value = default(int),
                };
            }

            public IParameter<int> DeltaX
            {
                get { return this.GetOrDefault<int>(ParameterKeys.DeltaX); }
                private set { this[ParameterKeys.DeltaX] = value; }
            }

            public IParameter<int> DeltaY
            {
                get { return this.GetOrDefault<int>(ParameterKeys.DeltaY); }
                private set { this[ParameterKeys.DeltaY] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string DeltaX = "DeltaX";
            public const string DeltaY = "DeltaY";
        }

        public void Dispose()
        {
            
        }
    }
}

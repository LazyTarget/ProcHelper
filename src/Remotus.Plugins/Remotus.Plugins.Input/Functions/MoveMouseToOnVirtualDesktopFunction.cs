using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class MoveMouseToOnVirtualDesktopFunction : IFunction, IFunction<MousePosition>
    {
        public MoveMouseToOnVirtualDesktopFunction()
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
                var absoluteX = arguments?.Parameters.GetOrDefault<double>(ParameterKeys.AbsoluteX)?.Value;
                var absoluteY = arguments?.Parameters.GetOrDefault<double>(ParameterKeys.AbsoluteY)?.Value;

                MouseInputPlugin.InputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(absoluteX.GetValueOrDefault(), absoluteY.GetValueOrDefault());
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
            public string ID => "4B373E67-2E4E-4482-8DAF-5179FE4EA4FF";
            public string Name => "Move mouse to (virtual desktop)";
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
                return new MoveMouseToOnVirtualDesktopFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                AbsoluteX = new Parameter<double>
                {
                    Name = ParameterKeys.AbsoluteX,
                    Required = false,
                    Type = typeof(double),
                    Value = default(double),
                };
                AbsoluteY = new Parameter<double>
                {
                    Name = ParameterKeys.AbsoluteY,
                    Required = false,
                    Type = typeof(double),
                    Value = default(double),
                };
            }

            public IParameter<double> AbsoluteX
            {
                get { return this.GetOrDefault<double>(ParameterKeys.AbsoluteX); }
                private set { this[ParameterKeys.AbsoluteX] = value; }
            }

            public IParameter<double> AbsoluteY
            {
                get { return this.GetOrDefault<double>(ParameterKeys.AbsoluteY); }
                private set { this[ParameterKeys.AbsoluteY] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string AbsoluteX = "AbsoluteX";
            public const string AbsoluteY = "AbsoluteY";
        }

        public void Dispose()
        {
            
        }
    }
}

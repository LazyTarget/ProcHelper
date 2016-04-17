using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Input
{
    public class GetMousePositionFunction : IFunction, IFunction<MousePosition>
    {
        public GetMousePositionFunction()
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
            public string ID => "8C872066-0770-4DF6-B357-8D95AA0C8AA2";
            public string Name => "Get mouse position";
            public string Version => "1.0.0.0";

            public IParameterCollection GetParameters()
            {
                IParameterCollection res = null;
                return res;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<MousePosition> Instantiate()
            {
                return new GetMousePositionFunction();
            }
        }

        public void Dispose()
        {

        }
    }
}

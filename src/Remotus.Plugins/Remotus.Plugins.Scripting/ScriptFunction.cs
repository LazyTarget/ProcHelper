using System;
using System.Threading.Tasks;
using Remotus.Base;
using Remotus.Base.Scripting;

namespace Remotus.Plugins.Scripting
{
    public class ScriptFunction : IFunction
    {
        private readonly ScriptFunctionDescriptor _descriptor;
        private readonly ScriptExecutor _executor;

        public ScriptFunction(ScriptFunctionDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            _descriptor = descriptor;
            _executor = new ScriptExecutor();
        }

        public IFunctionDescriptor GetDescriptor()
        {
            return _descriptor;
        }

        public async Task<IFunctionResult> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            _executor.Context = context;

            var response = await _executor.Execute(_descriptor.Script);
            var result = new FunctionResult
            {
                Arguments = arguments,
                Error = response?.Error,
                Result = response?.Result,
                ResultType = response?.ResultType,
            };
            return result;
        }

        public void Dispose()
        {
            
        }
    }
}
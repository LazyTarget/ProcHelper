using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface IFunction : IDisposable
    {
        IFunctionDescriptor GetDescriptor();
        Task<IFunctionResult> Execute(IExecutionContext context, IFunctionArguments arguments);
    }

    public interface IFunction<TResult> : IFunction
    {
        new Task<IFunctionResult<TResult>> Execute(IExecutionContext context, IFunctionArguments arguments);
    }
}

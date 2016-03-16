using System;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IFunction : IDisposable
    {
        Task<IFunctionResult> Execute(IExecutionContext context, IFunctionArguments arguments);
    }

    public interface IFunction<TResult> : IFunction
    {
        new Task<IFunctionResult<TResult>> Execute(IExecutionContext context, IFunctionArguments arguments);
    }
}

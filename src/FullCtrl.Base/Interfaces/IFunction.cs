using System;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IFunction : IDisposable
    {
        Task<IFunctionResult> Execute(IExecutionContext context, IFunctionArguments arguments);
    }
}

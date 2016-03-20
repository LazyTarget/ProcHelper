using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface ICommand : IDisposable
    {
        ICommandDescriptor GetDescriptor();
        Task<ICommandResult> Execute(IExecutionContext context);
    }
}

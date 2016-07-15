using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface IServicePlugin : IService, IPlugin
    {
        Task Init(IExecutionContext context);
    }
}

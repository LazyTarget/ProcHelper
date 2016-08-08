using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface ICustomHubAgent : IHubAgent
    {
        Task InvokeCustom(IHubMessage message);
    }
}
using System.Threading.Tasks;

namespace Remotus.Base.Interfaces.Net
{
    public interface ICustomHubAgent : IHubAgent
    {
        Task InvokeCustom(IHubMessage message);
    }
}
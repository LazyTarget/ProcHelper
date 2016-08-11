using System.Threading.Tasks;
using Remotus.Base.Net;

namespace Remotus.Base.Interfaces.Net
{
    public interface ICustomHubAgent : IHubAgent
    {
        Task InvokeCustom(CustomHubMessage message);
    }
}
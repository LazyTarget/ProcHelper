using System.Collections.Generic;

namespace Remotus.Base
{
    public interface IHubDescriptor // : IComponentDescriptor
    {
        string HubName { get;}
        IEnumerable<HubTrigger> GetTriggers();
        IEnumerable<HubAction> GetActions();
    }
}
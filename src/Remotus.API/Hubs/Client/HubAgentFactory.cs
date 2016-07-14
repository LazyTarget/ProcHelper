using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.API.Hubs.Client
{
    public class HubAgentFactory : IHubAgentFactory
    {
        private ApiConfig _apiConfig;

        public HubAgentFactory()
        {
            _apiConfig = ServiceInstance.LoadApiConfig();
        }

        public IHubAgent Create(string[] hubs)
        {
            throw new NotImplementedException();
        }
    }
}

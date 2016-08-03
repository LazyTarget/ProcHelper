using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Remotus.Base;
using Remotus.Base.Net;

namespace Remotus.Core.Net.Client
{
    public class HubAgentManager : IHubAgentManager
    {
        private readonly IHubConnector _hubConnector;
        private readonly IDictionary<string, IHubAgent> _hubs;

        public HubAgentManager(IHubConnector hubConnector, IEnumerable<IHubAgent> hubs)
        {
            if (hubConnector == null)
                throw new ArgumentNullException(nameof(hubConnector));
            if (hubs == null)
                throw new ArgumentNullException(nameof(hubs));
            _hubConnector = hubConnector;
            _hubs = hubs.ToDictionary(x => x.HubName);
        }
        
        public bool Connected           => _hubConnector?.State == ConnectionState.Connected;

        public IHubConnector Connector  => _hubConnector;


        public IReadOnlyDictionary<string, IHubAgent> GetHubs()
        {
            var result = new ReadOnlyDictionary<string, IHubAgent>(_hubs);
            return result;
        }

        public IHubAgent GetHub(string hubName)
        {
            IHubAgent hub;
            if (_hubs.TryGetValue(hubName, out hub))
                return hub;
            return null;
        }



        public void Dispose()
        {
            _hubConnector?.Disconnect();
        }
    }
}

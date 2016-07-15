﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Remotus.Base;

namespace Remotus.Core.Net.Client
{
    public class HubAgentManager : IHubAgentManager
    {
        private readonly HubConnection _hubConnection;
        private readonly IDictionary<string, IHubAgent> _hubs;

        public HubAgentManager(HubConnection hubConnection, IEnumerable<IHubAgent> hubs)
        {
            if (hubConnection == null)
                throw new ArgumentNullException(nameof(hubConnection));
            if (hubs == null)
                throw new ArgumentNullException(nameof(hubs));
            _hubConnection = hubConnection;
            _hubs = hubs.ToDictionary(x => x.HubName);
        }
        
        public bool Connected => _hubConnection?.State == ConnectionState.Connected;

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

        public async Task Connect()
        {
            var connection = _hubConnection;
            if (connection != null)
            {
                await connection.Start();
            }
        }

        public void Disconnect()
        {
            var error = new Exception("My custom exc. Closing hub connection...");
            _hubConnection.Stop(error);
        }


        public void Dispose()
        {
            Disconnect();
        }
    }
}
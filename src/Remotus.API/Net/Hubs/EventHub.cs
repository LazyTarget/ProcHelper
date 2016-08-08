using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.API.Net.Hubs
{
    public class EventHub : Hub
    {
        private static readonly HashSet<string> _connections = new HashSet<string>();
        
        public EventHub()
        {
            
        }


        public void Send(string channelName, string eventName, string json)
        {
            Clients.All.onEvent(channelName, eventName, json, DateTime.UtcNow);
        }

        public void Discover()
        {
            foreach (var connection in _connections)
            {
                if (connection == Context.ConnectionId)
                    continue;
                Clients.Caller.onEvent(connection, "some eventName", "I am here... @" + connection, DateTime.UtcNow);
            }
        }

        public override Task OnConnected()
        {
            var version = Context.QueryString["hub-version"];
            if (version != "1.0")
            {
                // ...
                Clients.Caller.notifyWrongVersion();

                // able to deny connection??
            }

            
            _connections.Add(Context.ConnectionId);
            HubServer.Instance.ConnectionManager.OnConnected(this);
            Debug.WriteLine("EventHub::OnConnected() Instance count: {0}", _connections.Count);

            Clients.Others.onEvent(Context.ConnectionId, "onConnected", "Client has connected");

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            _connections.Add(Context.ConnectionId);
            HubServer.Instance.ConnectionManager.OnReconnected(this);
            Debug.WriteLine("EventHub::OnReconnected() Instance count: {0}", _connections.Count);

            Clients.Others.onEvent(Context.ConnectionId, "onReconnected", "Client has reconnected");
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(Context.ConnectionId);
            HubServer.Instance.ConnectionManager.OnDisconnected(this, stopCalled);
            Debug.WriteLine("EventHub::OnDisconnected() Instance count: {0}", _connections.Count);

            Clients.Others.onEvent(Context.ConnectionId, "onDisconnected", "Client has disconnected");

            return base.OnDisconnected(stopCalled);
        }

    }
}
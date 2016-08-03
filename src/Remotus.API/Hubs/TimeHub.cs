using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.API.Hubs
{
    public class TimeHub : Hub
    {
        private static readonly HashSet<string> _connections = new HashSet<string>();
        private static readonly ExecuteLoop _loop = new ExecuteLoop();

        public TimeHub()
        {
            
        }

        
        private void StartLoop()
        {
            ThreadPool.QueueUserWorkItem((sender) =>
                    Debug.WriteLine($"EventHub::Loop exited: {_loop.Execute(this)}"));
        }


        public override Task OnConnected()
        {
            if (_connections.Count == 0)
            {
                StartLoop();
            }

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

            if (_connections.Count <= 0 && _loop.Executing)
            {
                if (!_loop.CancellationToken.IsCancellationRequested)
                    _loop.CancellationToken.Cancel();
            }


            Clients.Others.onEvent(Context.ConnectionId, "onDisconnected", "Client has disconnected");

            return base.OnDisconnected(stopCalled);
        }



        public class ExecuteLoop
        {
            private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);
            private CancellationTokenSource _cancellationTokenSource;

            public ExecuteLoop()
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            public CancellationTokenSource CancellationToken { get { return _cancellationTokenSource; } }

            public bool Executing { get; private set; }

            public int Execute(TimeHub hub)
            {
                try
                {
                    while (Executing)
                    {
                        if (!_cancellationTokenSource.IsCancellationRequested)
                            _cancellationTokenSource.Cancel();
                        _resetEvent.WaitOne();
                    }
                    
                    _cancellationTokenSource = new CancellationTokenSource();
                    var loopCount = 0;
                    while (true)
                    {
                        Executing = true;
                        loopCount++;
                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            return 1;
                        }

                        Debug.WriteLine("ExecuteLoop::" + loopCount);
                        //hub.Clients.All.onTick(DateTime.UtcNow);
                        GlobalHost.ConnectionManager.GetHubContext<TimeHub>().Clients.All.onTick(DateTime.UtcNow);


                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        _resetEvent.Set();
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    return -1;
                }
                finally
                {
                    Executing = false;
                    _resetEvent.Set();
                }
            }

        }

    }
}
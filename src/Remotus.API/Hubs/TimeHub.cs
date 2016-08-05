using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.API.Hubs
{
    public class TimeHub : HubBase
    {
        private static readonly ExecuteLoop _loop = new ExecuteLoop();

        public TimeHub()
        {

        }

        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;


        private void StartLoop()
        {
            if (_loop.Executing)
                return;
            ThreadPool.QueueUserWorkItem((sender) =>
                    Debug.WriteLine($"EventHub::Loop exited: {_loop.Execute(this)}"));
        }


        public override async Task OnConnected()
        {
            await base.OnConnected();


            var version = Context.QueryString["hub-version"];
            if (version != "1.0")
            {
                // ...
                Clients.Caller.notifyWrongVersion();

                // able to deny connection??
            }
            
            Debug.WriteLine($"TimeHub::OnConnected(): {Context.ConnectionId}");

            Clients.Others.OnEvent(Context.ConnectionId, "onConnected", "Client has connected");


            if (HubServer.Instance.ClientManager.GetClients().Any(x => x.Hubs.Any(h => h == "TimeHub")))
            {
                StartLoop();
            }
        }


        public override async Task OnReconnected()
        {
            await base.OnReconnected();
            
            Debug.WriteLine($"TimeHub::OnReconnected(): {Context.ConnectionId}");

            Clients.Others.OnEvent(Context.ConnectionId, "onReconnected", "Client has reconnected");


            if (HubServer.Instance.ClientManager.GetClients().Any(x => x.Hubs.Any(h => h == "TimeHub")))
            {
                StartLoop();
            }
        }


        public override async Task OnDisconnected(bool stopCalled)
        {
            await base.OnDisconnected(stopCalled);
            
            Debug.WriteLine($"TimeHub::OnDisconnected(): {Context.ConnectionId} ({stopCalled})");

            Clients.Others.OnEvent(Context.ConnectionId, "onDisconnected", "Client has disconnected");


            if (!HubServer.Instance.ClientManager.GetClients().Any(x => x.Hubs.Any(h => h == "TimeHub")) && _loop.Executing)
            {
                if (!_loop.CancellationToken.IsCancellationRequested)
                    _loop.CancellationToken.Cancel();
            }
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

                        Debug.WriteLine("ExecuteLoop(TimeHub)::" + loopCount);
                        //hub.Clients.All.onTick(DateTime.UtcNow);
                        GlobalHost.ConnectionManager.GetHubContext<TimeHub>().Clients.All.OnTick(DateTime.UtcNow);


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
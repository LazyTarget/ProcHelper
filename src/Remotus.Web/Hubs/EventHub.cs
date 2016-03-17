using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.Web.Hubs
{
    public class EventHub : Hub
    {
        private static int _instanceCount;
        private static readonly ExecuteLoop _loop = new ExecuteLoop();

        public EventHub()
        {
            
        }


        public void Send(string channelName, string eventName, string json)
        {
            Clients.All.onEvent(channelName, eventName, json);
        }

        

        public override Task OnConnected()
        {
            if (_instanceCount == 0)
            {
                ThreadPool.QueueUserWorkItem((sender) =>
                    Debug.WriteLine($"EventHub::Loop exited: {_loop.Execute(this)}"));
            }

            _instanceCount++;
            Debug.WriteLine("EventHub::OnConnected() Instance count: " + _instanceCount);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Debug.WriteLine("EventHub::OnReconnected() Instance count: " + _instanceCount);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _instanceCount--;
            Debug.WriteLine("EventHub::OnDisconnected() Instance count: " + _instanceCount);
            if (_instanceCount <= 0)
            {
                _loop.CancellationToken.Cancel();
            }

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

            public int Execute(EventHub hub)
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
                        hub.Send(hub.GetHashCode().ToString(), "ads", "{ name: 'peter' }");
                        Debug.WriteLine("Sending message...");


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
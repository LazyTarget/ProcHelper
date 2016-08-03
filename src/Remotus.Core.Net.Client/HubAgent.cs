using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Models.Hub;

namespace Remotus.Core.Net.Client
{
    public class HubAgent : IHubAgent
    {
        private readonly HubConnection _hubConnection;
        private readonly IMessageCache _messageCache;
        private readonly IHubProxy _hubProxy;
        private bool _isDisposing;
        private readonly object _processQueueLock = new object();

        public HubAgent(string hubName, HubConnection hubConnection, IMessageCache messageCache)
        {
            if (string.IsNullOrWhiteSpace(hubName))
                throw new ArgumentNullException(nameof(hubName));
            if (hubConnection == null)
                throw new ArgumentNullException(nameof(hubConnection));
            HubName = hubName;
            _messageCache = messageCache;
            _hubConnection = hubConnection;
            _hubProxy = _hubConnection.CreateHubProxy(HubName);

            _hubConnection.StateChanged += HubConnection_OnStateChanged;
        }


        public string HubName { get; }

        public bool Connected => _hubConnection?.State == ConnectionState.Connected;


        public Task Invoke(IHubMessage message)
        {
            Task task;
            if (!Connected)
            {
                if (_messageCache != null && message.Queuable)
                {
                    var request = new HubRequest
                    {
                        HubName = HubName,
                        ConnectionId = _hubConnection?.ConnectionId,
                        //AgentId = _hubConnection.GetAgentId(),
                        Message = message,
                    };
                    _messageCache.Enqueue(request);
                }

                task = Task.Factory.StartNew((msg) =>
                {
                    if (message.Queuable)
                        throw new Exception("Could not send message, not connected to Hub. Queueing message...");
                    else
                        throw new Exception("Could not send message, not connected to Hub");
                }, message);
                return task;
            }

            task = _hubProxy.Invoke(message.Method, args: message.Args.ToArray());
            return task;
        }


        public IHubSubscription Subscribe(string eventName)
        {
            var subscription = new HubSubscription();
            subscription.HubName = HubName;
            subscription.EventName = eventName;

            Action<IList<JToken>> HubProxySub_OnReceived = list =>
            {
                subscription.Invoke(list);
            };

            var sub = _hubProxy.Subscribe(eventName);
            sub.Received += HubProxySub_OnReceived;

            // todo: able to unsubscribe via IDisposable

            return subscription;
        }


        public async Task Connect()
        {
            var connection = _hubConnection;
            if (connection != null)
            {
                try
                {
                    await connection.Start();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public bool EnsureReconnecting()
        {
            var result = false;
            try
            {
                if (_hubConnection != null)
                    result = _hubConnection.EnsureReconnecting();
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public void Disconnect()
        {
            var error = new Exception("My custom exc. Closing hub connection...");
            try
            {
                _hubConnection?.Stop(error);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void HubConnection_OnStateChanged(StateChange stateChange)
        {
            if (_isDisposing)
                return;

            if (stateChange.NewState == ConnectionState.Connected)
            {
                // Re-gained connection
                ThreadPool.QueueUserWorkItem(ProcessSendQueue);
            }
        }


        private void ProcessSendQueue(object state)
        {
            var sendCount = 0;
            var failed = new List<HubRequest>();
            
            if (!Connected)
            {
                //StartReconnectionTimer();
                return;
            }
            if (_isDisposing)
            {
                return;
            }

            lock (_processQueueLock)
            {
                //var messageCount = _messageQueue.Count;   // todo?
                var messageCount = 1;
                if (messageCount > 0)
                {
                    //LogMessage(LogLevel.Verbose, "Proccessing message queue, message count: {0}", _messageQueue.Count);

                    HubRequest req;
                    while (Connected && _messageCache.TryDequeue(out req))
                    {
                        try
                        {
                            if (req.HubName != HubName)
                            {
                                // todo: Dequeued, will never run?
                                continue;
                            }

                            var task = Invoke(req.Message);
                            task.Wait();

                            sendCount++;
                        }
                        catch (Exception ex)
                        {
                            //LogMessage(LogLevel.Error, "Error sending message data to hub. Error: {0}", ex.Message);
                            //LogException(ex);

                            if (!(ex is NotImplementedException || ex is NotSupportedException))
                                failed.Add(req);
                        }
                        finally
                        {

                        }
                    }

                    if (failed.Any())
                    {
                        //LogMessage(LogLevel.Verbose, "Re-queuing failed messages ({0}/{1})", failed.Count, messageCount);
                        failed.ForEach(item => _messageCache.Enqueue(item));

                        // todo: timer to retry?
                    }
                    else
                    {
                        //LogMessage(LogLevel.Verbose, "Successfully sent all messages (count: {0})", messageCount);

                    }
                }
                else
                {

                }
            }

            //if (sentMessages > 0)
            //{
            //    LogMessage(LogLevel.Debug, "Invoking messageQueueTrigger.Set() after sending: {0} messages", sentMessages);
            //    _messageQueueTrigger.Set();
            //}
            //LogMessage(LogLevel.Debug, "ProcessSendQueue() --END--");
        }


        public void Dispose()
        {
            _isDisposing = true;
        }
    }
}

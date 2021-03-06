﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Interfaces.Net;
using Remotus.Base.Net;
using ConnectionState = Remotus.Base.Interfaces.Net.ConnectionState;

namespace Remotus.Core.Net.Client
{
    public class HubAgent : IHubAgent
    {
        protected static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);

        private readonly IHubConnector _hubConnector;
        private readonly IMessageCache _messageCache;
        protected readonly IHubProxy _hubProxy;
        private bool _isDisposing;
        private readonly object _processQueueLock = new object();
        private readonly IDictionary<string, HubSubscription> _subscriptions;


        public HubAgent(string hubName, IHubProxy hubProxy, IHubConnector hubConnector, IMessageCache messageCache)
        {
            if (string.IsNullOrWhiteSpace(hubName))
                throw new ArgumentNullException(nameof(hubName));
            if (hubConnector == null)
                throw new ArgumentNullException(nameof(hubConnector));
            HubName = hubName;
            _messageCache = messageCache;
            _hubProxy = hubProxy;
            _subscriptions = new Dictionary<string, HubSubscription>();
            
            _hubConnector = hubConnector;
            _hubConnector.StateChanged -= HubConnection_OnStateChanged;
            _hubConnector.StateChanged += HubConnection_OnStateChanged;
        }


        public string HubName { get; }

        protected bool Connected        => _hubConnector.Connected;

        public IHubConnector Connector  => _hubConnector;

        
        protected virtual void HubConnection_OnStateChanged(object sender, HubConnectionStateChange stateChange)
        {
            if (_isDisposing)
                return;
            
            //if (stateChange.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            if (stateChange.NewState == ConnectionState.Connected)
            {
                // Re-gained connection
                ThreadPool.QueueUserWorkItem(ProcessSendQueue);
            }
        }


        public virtual Task Invoke(IHubMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            Task task;
            if (!Connected)
            {
                if (_messageCache != null && message.Queuable)
                {
                    _log.Info($"HubAgent:Invoke() Not connected. Queuing message...");
                    var request = new HubRequest
                    {
                        HubName = HubName,
                        ConnectionId = _hubConnector?.ConnectionId,
                        //AgentId = _hubConnection.GetAgentId(),
                        Message = message,
                        TimeQueued = DateTime.UtcNow,
                        TimeSent = null,
                    };
                    _messageCache.Enqueue(request);
                }
                else
                {
                    if (message.Queuable)
                        _log.Warn($"HubAgent:Invoke() Not connected. Message could not be queued to undefined messageQueue");
                    else
                        _log.Warn($"HubAgent:Invoke() Not connected. Message will not be queued");
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

            object[] args = message.Args?.ToArray();
            task = InvokeMethod(message.Method, args);
            return task;
        }


        protected virtual Task InvokeMethod(string method, object[] args)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException(nameof(method));

            var task = _hubProxy.Invoke(method, args: args);
            return task;
        }

        
        public virtual IHubSubscription Observe(string eventName)
        {
            lock (_subscriptions)
            {
                HubSubscription subscription;
                if (!_subscriptions.TryGetValue(eventName, out subscription) || subscription == null)
                {
                    var observable = _hubProxy.Observe(eventName);
                    subscription = new HubSubscription(observable);
                    subscription.HubName = HubName;
                    subscription.EventName = eventName;
                    _subscriptions.Add(subscription.EventName, subscription);
                }
                return subscription;
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
                var messageCount = _messageCache.Any() ? 1 : 0;
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
                            req.TimeSent = DateTime.UtcNow;
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

                    _log.Info($"HubAgent:ProcessSendQueue() Success: {sendCount}");
                    _log.Info($"HubAgent:ProcessSendQueue() Failed: {failed.Count}");
                    if (failed.Any())
                    {
                        //LogMessage(LogLevel.Verbose, "Re-queuing failed messages ({0}/{1})", failed.Count, messageCount);
                        failed.ForEach(item => _messageCache.Enqueue(item));

                        // Timer to retry fail queue
                        ThreadPool.QueueUserWorkItem(delegate(object o)
                        {
                            Thread.Sleep(10 * 1000);
                            ProcessSendQueue(state: "retry-failed");
                        });
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
            if (_hubConnector != null)
            {
                _hubConnector.StateChanged -= HubConnection_OnStateChanged;
                //_hubConnector.Dispose();
                //_hubConnector = null;
            }

            _isDisposing = true;
        }
    }
}

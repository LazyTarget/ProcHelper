using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace Remotus.API.Hubs.Client
{
    public class ClientHubManager
    {
        private readonly IDictionary<string, IHubProxy> _hubProxies;
        private readonly HubConnection _connection;
        private readonly ConcurrentQueue<HubRequest> _messageQueue;
        private bool _enableQueueing;

        public ClientHubManager(HubConnection connection)
        {
            _hubProxies = new Dictionary<string, IHubProxy>();
            _messageQueue = new ConcurrentQueue<HubRequest>();
            _enableQueueing = true;
            _connection = connection;
            _connection.StateChanged += Connection_OnStateChanged;
        }


        public bool Connected
        {
            get { return _connection.State == ConnectionState.Connected; }
        }


        public virtual IHubProxy GetHubProxy(string hubName)
        {
            if (string.IsNullOrWhiteSpace(hubName))
                throw new ArgumentNullException(nameof(hubName));

            var hubProxy = _hubProxies.ContainsKey(hubName)
                               ? _hubProxies[hubName]
                               : null;
            return hubProxy;
        }


        protected virtual Subscription Subscribe(string hubName, string eventName)
        {
            if (string.IsNullOrWhiteSpace(hubName))
                throw new ArgumentNullException(nameof(hubName));
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));

            var hubProxy = GetHubProxy(hubName);
            if (hubProxy == null)
                throw new InvalidOperationException($"Could not get hub proxy '{hubName}'");
            var subscription = hubProxy.Subscribe(eventName);
            return subscription;
        }


        public Task Invoke(HubMessage message)
        {
            Task task;
            if (!Connected)
            {
                QueueMessage(message);

                task = Task.Factory.StartNew((msg) =>
                {
                    throw new Exception("Could not send message, not connected to Hub. Queueing message...");
                }, message);
                return task;
            }

            var hubProxy = GetHubProxy(message.Hub);
            if (hubProxy == null)
                throw new InvalidOperationException($"Could not get hub proxy '{message.Hub}'");
            task = hubProxy.Invoke(message.Method, message.Args);
            return task;
        }


        public Task<TResult> Invoke<TResult>(HubMessage message)
        {
            Task<TResult> task;
            if (!Connected)
            {
                QueueMessage(message);

                task = Task<TResult>.Factory.StartNew((msg) =>
                {
                    throw new Exception("Could not send message, not connected to Hub. Queueing message...");
                }, message);
                return task;
            }

            var hubProxy = GetHubProxy(message.Hub);
            if (hubProxy == null)
                throw new InvalidOperationException($"Could not get hub proxy '{message.Hub}'");
            task = hubProxy.Invoke<TResult>(message.Method, message.Args);
            return task;
        }


        //public async Task<TResult> Invoke<TResult, TProgress>(HubMessage message, Action<TProgress> onProgress)
        //{
        //    var hubProxy = GetHubProxy(message.Hub);
        //    if (hubProxy == null)
        //        throw new InvalidOperationException($"Could not get hub proxy '{message.Hub}'");
        //    var result = await hubProxy.Invoke<TResult, TProgress>(message.Method, onProgress, message.Args);
        //    return result;
        //}



        private void QueueMessage(HubMessage message)
        {
            var request = new HubRequest();
            request.Message = message;

            if (_enableQueueing && message.Queuable)
            {
                _messageQueue.Enqueue(request);
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

            var messageCount = _messageQueue.Count;
            if (messageCount > 0)
            {
                //LogMessage(LogLevel.Verbose, "Proccessing message queue, message count: {0}", _messageQueue.Count);

                HubRequest req;
                while (_messageQueue.TryDequeue(out req))
                {
                    try
                    {
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
                    failed.ForEach(_messageQueue.Enqueue);

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

            //if (sentMessages > 0)
            //{
            //    LogMessage(LogLevel.Debug, "Invoking messageQueueTrigger.Set() after sending: {0} messages", sentMessages);
            //    _messageQueueTrigger.Set();
            //}
            //LogMessage(LogLevel.Debug, "ProcessSendQueue() --END--");
        }


        private void Connection_OnStateChanged(StateChange stateChange)
        {
            if (stateChange.NewState == ConnectionState.Connected)
            {
                ThreadPool.QueueUserWorkItem(ProcessSendQueue);
            }
        }

    }
}
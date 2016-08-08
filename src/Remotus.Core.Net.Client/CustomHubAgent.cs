using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Models.Hub;

namespace Remotus.Core.Net.Client
{
    public class CustomHubAgent : HubAgent, ICustomHubAgent
    {
        //private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);
        
        public CustomHubAgent(string hubName, IHubProxy hubProxy, IHubConnector hubConnector, IMessageCache messageCache)
            : base(hubName, hubProxy, hubConnector, messageCache)
        {
        }

        protected string CustomHubIdentifier => $"CustomHub_{HubName}";


        public override Task Invoke(IHubMessage message)
        {
            var msg = message;
            var task = base.Invoke(msg);
            return task;
        }

        public Task InvokeCustom(IHubMessage message)
        {
            var args = message?.Args?.Count > 0
                ? new object[] { message.Args.ToArray() }
                : new object[0];

            var inner = new ExternalHubMessage();
            inner.HubName = HubName;
            inner.Method = message?.Method;
            inner.Args = args;
            inner.Queuable = message?.Queuable ?? false;

            var msg = new ExternalHubMessage();
            msg.HubName = "CustomHub";
            msg.Method = "InvokeCustom";
            //msg.Args = args;
            msg.Args = new object[] { inner };
            msg.Queuable = inner.Queuable;

            var task = base.Invoke(msg);
            return task;
        }


        public override IHubSubscription Subscribe(string eventName)
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


            Task task = null;
            try
            {
                var msg = new HubMessage
                {
                    Method = "AddToGroup",
                    Args = new object[] { CustomHubIdentifier },
                    Queuable = true,
                };
                var timeout = TimeSpan.FromSeconds(15);
                task = Invoke(msg);
                task.Wait(timeout);
            }
            catch (Exception ex)
            {
                
            }

            return subscription;
        }

    }
}

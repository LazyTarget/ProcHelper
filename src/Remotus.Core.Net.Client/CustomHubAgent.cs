using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Models.Hub;
using Remotus.Base.Models.Payloads;
using Remotus.Base.Net;

namespace Remotus.Core.Net.Client
{
    public class CustomHubAgent : HubAgent
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
            if (!(msg is ExternalHubMessage))
            {
                var args = message?.Args?.Count > 0
                    ? new object[] { message.Args.ToArray() }
                    : new object[0];

                var innerMsg = new ExternalHubMessage
                {
                    HubName = HubName,
                    Method = "InvokeCustom",
                    Args = args,
                    Queuable = msg.Queuable,
                    //AgentId = 
                };

                args = new object[] { innerMsg };
                msg = new ExternalHubMessage
                {
                    HubName = HubName,
                    Method = "InvokeCustom",
                    Args = args,
                    Queuable = msg.Queuable,
                    //AgentId = 
                };
            }

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

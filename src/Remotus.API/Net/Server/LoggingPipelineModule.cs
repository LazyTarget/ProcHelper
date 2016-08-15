using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Remotus.Base;

namespace Remotus.API.Net.Server
{
    public class LoggingPipelineModule : HubPipelineModule
    {
        private static ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod().DeclaringType.FullName);


        #region Connectivity

        protected override bool OnBeforeConnect(IHub hub)
        {
            var res = base.OnBeforeConnect(hub);
            return res;
        }

        protected override void OnAfterConnect(IHub hub)
        {
            base.OnAfterConnect(hub);
        }

        protected override bool OnBeforeReconnect(IHub hub)
        {
            var res = base.OnBeforeReconnect(hub);
            return res;
        }

        protected override void OnAfterReconnect(IHub hub)
        {
            base.OnAfterReconnect(hub);
        }

        protected override bool OnBeforeDisconnect(IHub hub, bool stopCalled)
        {
            var res = base.OnBeforeDisconnect(hub, stopCalled);
            return res;
        }

        protected override void OnAfterDisconnect(IHub hub, bool stopCalled)
        {
            base.OnAfterDisconnect(hub, stopCalled);
        }

        #endregion


        #region Security

        protected override bool OnBeforeAuthorizeConnect(HubDescriptor hubDescriptor, IRequest request)
        {
            var res = base.OnBeforeAuthorizeConnect(hubDescriptor, request);
            return res;
        }

        #endregion


        #region Traffic

        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            _log.Debug(() => $"=> Invoking '{context.MethodDescriptor.Name}' on hub '{context.MethodDescriptor.Hub.Name}'");
            var res = base.OnBeforeIncoming(context);
            return res;
        }

        protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
        {
            var res = base.OnAfterIncoming(result, context);
            return res;
        }

        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            _log.Debug(() => $"<= Invoking '{context.Invocation.Method}' on client hub '{context.Invocation.Hub}'");
            return base.OnBeforeOutgoing(context);
        }

        protected override void OnAfterOutgoing(IHubOutgoingInvokerContext context)
        {
            base.OnAfterOutgoing(context);
        }

        #endregion

    }
}

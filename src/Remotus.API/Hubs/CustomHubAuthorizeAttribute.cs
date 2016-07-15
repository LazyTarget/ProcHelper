using System.IO;
using System.Security.Principal;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Remotus.API.Models;

namespace Remotus.API.Hubs
{
    public class CustomHubAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            var handshake = request.GetHandshake();
            var result = ValidateHandshake(handshake);
            return result;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var handshake = hubIncomingInvokerContext.Hub.Context.Request.GetHandshake();
            var result = ValidateHandshake(handshake);
            return result;
        }

        protected override bool UserAuthorized(IPrincipal user)
        {
            var result = base.UserAuthorized(user);
            return result;
        }

        protected virtual bool ValidateHandshake(HubHandshake handshake)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(handshake?.AgentId))
            {
                result = true;
            }
            else
            {
                
            }
            return result;
        }

    }
}

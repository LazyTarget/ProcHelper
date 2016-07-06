using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using Remotus.API.Models;

namespace Remotus.API.Components
{
    public class CustomHubAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            var customSerializerSettings = new CustomJsonSerializerSettings();
            var serializer = JsonSerializer.Create(customSerializerSettings.Settings);

            var s = request.Headers["App-Handshake"];
            var textReader = new StringReader(s);
            var jsonReader = new JsonTextReader(textReader);
            var handshake = serializer.Deserialize<HubHandshake>(jsonReader);

            var result = ValidateHandshake(handshake);
            return result;
        }

        public override bool AuthorizeHubMethodInvocation(IHubIncomingInvokerContext hubIncomingInvokerContext, bool appliesToMethod)
        {
            var customSerializerSettings = new CustomJsonSerializerSettings();
            var serializer = JsonSerializer.Create(customSerializerSettings.Settings);

            var s = hubIncomingInvokerContext.Hub.Context.Request.Headers["App-Handshake"];
            var textReader = new StringReader(s);
            var jsonReader = new JsonTextReader(textReader);
            var handshake = serializer.Deserialize<HubHandshake>(jsonReader);

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
            if (!string.IsNullOrEmpty(handshake?.ClientKey))
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

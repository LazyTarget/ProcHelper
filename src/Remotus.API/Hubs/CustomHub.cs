using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.CSharp.RuntimeBinder;
using Remotus.Base.Models.Hub;

namespace Remotus.API.Hubs
{
    public class CustomHub : HubBase
    {
        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;

        

        public void InvokeCustom(ExternalHubMessage message)
        {
            try
            {
                var customHubIdentifier = $"CustomHub_{message.HubName}";
                var g = Clients.Group(customHubIdentifier);


                var dyn1 = (DynamicObject)g;
                var m = g.GetType()?.GetMethod(message.Method);
                var res = m?.Invoke(g, message.Args);

                var methodBinder = g;

                object result;
                var dyn = (DynamicObject)g;
                var r = dyn.TryInvoke(methodBinder, message.Args, out result);

                var r2 = dyn.TryInvokeMember(methodBinder, message.Args, out result);
                //var param = new Type[] {typeof (ExternalHubMessage)};
                var param = message.Args?.Select(x => x?.GetType()).ToArray() ?? new Type[0];
                var d = new DynamicMethod(message.Method, null, param);
                var s = d.Invoke(g, message.Args);
            }
            catch (RuntimeBinderException ex)
            {

            }
            catch (Exception ex)
            {
                
            }
        }



        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}

using System.Linq;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Remotus.Base;
using Remotus.Base.Net;
using Remotus.API.Net.Client;

namespace Remotus.API.Net.Server
{
    public class HubRecipePipelineModule : HubPipelineModule
    {
        #region Traffic

        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            var res = base.OnBeforeIncoming(context);
            return res;
        }

        protected override object OnAfterIncoming(object result, IHubIncomingInvokerContext context)
        {
            var res = base.OnAfterIncoming(result, context);

            
            var hubName = context.MethodDescriptor.Hub.Name;
            var methodName = context.MethodDescriptor.Name;
            var args = context.Args?.ToArray();
            if (hubName == "CustomHub" && methodName == "InvokeCustom")
            {
                var inner = args?.OfType<CustomHubMessage>().FirstOrDefault();
                if (inner != null)
                {
                    hubName = inner.HubName;
                    methodName = inner.Method;
                    args = inner.Args;
                }
            }


            var triggers = HubServer.Instance.RecipeRepository.GetTriggers();
            var actions = HubServer.Instance.RecipeRepository.GetActions();

            // Find recipes
            var recipes = HubServer.Instance.RecipeRepository.GetRecipes();
            var matchingRecipes = recipes.Where(x =>
            {
                if (x.Trigger.HubName == hubName)
                {
                    if (x.Trigger.EventName == methodName)
                    {
                        return true;
                    }
                }
                return false;
            }).ToArray();


            // Invoke recipes
            if (matchingRecipes.Length > 0)
            {
                args = args ?? new object[0];
                foreach (var recipe in matchingRecipes)
                {
                    IExecutionContext executionContext = new ExecutionContext
                    {
                        //ClientInfo = _clientInfo,
                        Logger = new TraceLogger(),
                        Remotus = new Remotus.API.v1.FullCtrlAPI(),
                        //SignalR =  // todo: !
                        HubAgentFactory = new HubAgentFactory(),
                    };

                    var task = recipe.Action.Invoke(executionContext, args);
                    //task.TryWaitAsync();
                    task.TryWait();
                }
            }


            return res;
        }

        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            return base.OnBeforeOutgoing(context);
        }

        protected override void OnAfterOutgoing(IHubOutgoingInvokerContext context)
        {
            base.OnAfterOutgoing(context);
        }

        #endregion

    }
}

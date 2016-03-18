using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lux;
using Remotus.API.v1;
using Remotus.Base;
using Remotus.Web.Models;

namespace Remotus.Web.Controllers
{
    public class FunctionController : Controller
    {
        public async Task<ActionResult> Plugin(string clientID, string pluginName)
        {
            var model = await GetPluginViewModel(clientID, pluginName);
            return View("Plugin", model);
        }


        public async Task<PluginViewModel> GetPluginViewModel(string clientID, string pluginName)
        {
            var resp = await GetFunctionPlugins(clientID);
            resp.EnsureSuccess("Error getting function plugins");
            var plugin = resp.Result?.FirstOrDefault(x => x.Name == pluginName);
            if (plugin == null)
                throw new Exception($"Plugin '{pluginName}' not found");

            var model = new PluginViewModel();
            model.PluginName = pluginName;
            model.Functions = new FunctionViewModel[0];
            
            var functions = plugin.GetFunctions().ToList();
            foreach (var functionDescriptor in functions)
            {
                model.Functions = model.Functions.Concat(new[] {GetFunctionViewModel(functionDescriptor)}).ToArray();
            }
            return model;
        }

        public FunctionViewModel GetFunctionViewModel(IFunctionDescriptor functionDescriptor)
        {
            var model = new FunctionViewModel();
            model.FunctionName = functionDescriptor.Name;
            model.ParameterCollection = functionDescriptor.GetParameters();
            return model;
        }


        public async Task<ActionResult> Execute(string clientID, string pluginName, string functionName)
        {
            var serializer = new CustomJsonSerializer();

            try
            {
                var resp = await GetFunctionPlugins(clientID);
                resp.EnsureSuccess("Error getting function plugins");
                var plugin = resp.Result?.FirstOrDefault(x => x.Name == pluginName);
                if (plugin == null)
                    throw new Exception($"Plugin '{pluginName}' not found");

                var functionDescriptor = plugin.GetFunctions()?.FirstOrDefault(x => x.Name == functionName);
                if (functionDescriptor == null)
                    throw new Exception($"Function '{functionName}' not found");

                var converter = new Converter();
                var parameters = functionDescriptor.GetParameters();
                parameters = parameters ?? new ParameterCollection();

                foreach (var param in parameters)
                {
                    var str = Request.Form["Param_" + param.Key];
                    if (str != null)
                    {
                        var val = converter.Convert(str, param.Value.Type);
                        param.Value.Value = val;
                    }
                }

                var arg = new FunctionArguments();
                arg.Parameters = parameters;

                //var function = functionDescriptor.Instantiate();
                //var result = await function.Execute(arg);

                var response = await ExecuteFunction(clientID, pluginName, functionName, arg);
                response.EnsureSuccess();
                //var actionResult = new JsonResult<IResponseBase<IFunctionResult>>(response, settings, Encoding.UTF8, request);
                
                var json = serializer.Serialize(response);
                var actionResult = new ContentResult
                {
                    Content = json,
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8,
                };
                return actionResult;
            }
            catch (Exception ex)
            {
                var response = DefaultResponseBase.CreateError<IFunctionResult>(DefaultError.FromException(ex));
                //var actionResult = new JsonResult<IResponseBase<IFunctionResult>>(response, settings, Encoding.UTF8, request);
                //return actionResult;

                var json = serializer.Serialize(response);
                var actionResult = new ContentResult
                {
                    Content = json,
                    ContentType = "application/json",
                    ContentEncoding = Encoding.UTF8,
                };
                return actionResult;
            }
        }


        

        private async Task<IResponseBase<IEnumerable<IPlugin>>> GetPlugins(string clientID)
        {
            var api = new FullCtrlAPI();

            IResponseBase<IEnumerable<IPlugin>> response =
                clientID != null
                    ? await api.GetRemotePlugins(clientID)
                    : await api.GetLocalPlugins();
            return response;
        }

        private async Task<IResponseBase<IEnumerable<IFunctionPlugin>>> GetFunctionPlugins(string clientID)
        {
            var api = new FullCtrlAPI();

            IResponseBase<IEnumerable<IFunctionPlugin>> response =
                clientID != null
                    ? await api.GetRemoteFunctionPlugins(clientID)
                    : await api.GetLocalFunctionPlugins();
            return response;
        }

        private async Task<IResponseBase<IEnumerable<IServicePlugin>>> GetServicePlugins(string clientID)
        {
            var api = new FullCtrlAPI();

            IResponseBase<IEnumerable<IServicePlugin>> response =
                clientID != null
                    ? await api.GetRemoteServicePlugins(clientID)
                    : await api.GetLocalServicePlugins();
            return response;
        }



        private async Task<IResponseBase<IFunctionResult>> ExecuteFunction(string clientID, string pluginName, string functionName, IFunctionArguments arguments)
        {
            var api = new FullCtrlAPI();

            IResponseBase<IFunctionResult> response =
                clientID != null
                    ? await api.ExecuteRemoteFunction(clientID, pluginName, functionName, arguments)
                    : await api.ExecuteLocalFunction(pluginName, functionName, arguments);
            return response;
        }

    }
}
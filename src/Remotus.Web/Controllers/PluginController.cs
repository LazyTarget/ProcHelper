using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Mvc;
using Lux;
using Remotus.API.v1;
using Remotus.Base;
using Remotus.Web.Models;

namespace Remotus.Web.Controllers
{
    public class PluginController : Controller
    {
        public async Task<ActionResult> Index(string clientID, string pluginID)
        {
            var actionResult = await ViewPlugin(clientID, pluginID);
            return actionResult;
        }


        [NonAction]
        public async Task<ActionResult> ViewPlugin(string clientID, string pluginID)
        {
            var model = await GetPluginViewModel(clientID, pluginID);
            return View("Plugin", model);
        }


        public async Task<PluginViewModel> GetPluginViewModel(string clientID, string pluginID)
        {
            var resp = await GetFunctionPlugins(clientID);
            resp.EnsureSuccess("Error getting function plugins");

            var plugin = resp.Result?.FirstOrDefault(
                    x => string.Equals(x.ID, pluginID, StringComparison.InvariantCultureIgnoreCase));
            if (plugin == null)
                throw new Exception($"Plugin '{pluginID}' not found");

            var model = new PluginViewModel();
            model.PluginID = plugin.ID;
            model.PluginName = plugin.Name;
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
            model.FunctionID = functionDescriptor.ID;
            model.FunctionName = functionDescriptor.Name;
            model.ParameterCollection = functionDescriptor.GetParameters();
            return model;
        }


        public async Task<FormattedContentResult<IResponseBase<IFunctionResult>>> Execute(string clientID, string pluginID, string functionID)
        {
            var serializer = new CustomJsonSerializer();

            try
            {
                var resp = await GetFunctionPlugins(clientID);
                resp.EnsureSuccess("Error getting function plugins");

                var plugin = resp.Result?.FirstOrDefault(
                            x => string.Equals(x.ID, pluginID, StringComparison.InvariantCultureIgnoreCase));
                if (plugin == null)
                    throw new Exception($"Plugin '{pluginID}' not found");

                var functionDescriptor = plugin.GetFunctions()?.FirstOrDefault(
                            x => string.Equals(x.ID, functionID, StringComparison.InvariantCultureIgnoreCase));
                if (functionDescriptor == null)
                    throw new Exception($"Function '{functionID}' not found");

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

                var response = await ExecuteFunction(clientID, pluginID, functionID, arg);
                response.EnsureSuccess();
                //var actionResult = new JsonResult<IResponseBase<IFunctionResult>>(response, settings, Encoding.UTF8, request);
                
                //var json = serializer.Serialize(response);
                //var actionResult = new ContentResult
                //{
                //    Content = json,
                //    ContentType = "application/json",
                //    ContentEncoding = Encoding.UTF8,
                //};
                //return actionResult;

                MediaTypeFormatter formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = serializer.GetSerializerSettings(),
                };
                MediaTypeHeaderValue mediaType = formatter.SupportedMediaTypes.FirstOrDefault();
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(Request.HttpMethod), Request.Url);
                var actionResult = new FormattedContentResult<IResponseBase<IFunctionResult>>(HttpStatusCode.OK, response, formatter, mediaType, request);
                return actionResult;
            }
            catch (Exception ex)
            {
                var response = DefaultResponseBase.CreateError<IFunctionResult>(DefaultError.FromException(ex));
                //var actionResult = new JsonResult<IResponseBase<IFunctionResult>>(response, settings, Encoding.UTF8, request);
                //return actionResult;

                //var json = serializer.Serialize(response);
                //var actionResult = new ContentResult
                //{
                //    Content = json,
                //    ContentType = "application/json",
                //    ContentEncoding = Encoding.UTF8,
                //};
                //return actionResult;

                MediaTypeFormatter formatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = serializer.GetSerializerSettings(),
                };
                MediaTypeHeaderValue mediaType = formatter.SupportedMediaTypes.FirstOrDefault();
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(Request.HttpMethod), Request.Url);
                var actionResult = new FormattedContentResult<IResponseBase<IFunctionResult>>(HttpStatusCode.OK, response, formatter, mediaType, request);
                return actionResult;
            }
        }


        public async Task<ActionResult> ExecutePartial(string clientID, string pluginID, string functionID)
        {
            var result = await Execute(clientID, pluginID, functionID);
            var model = result?.Content;
            return PartialView("_FunctionResult", model);
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



        private async Task<IResponseBase<IFunctionResult>> ExecuteFunction(string clientID, string pluginID, string functionID, IFunctionArguments arguments)
        {
            var api = new FullCtrlAPI();

            IResponseBase<IFunctionResult> response =
                clientID != null
                    ? await api.ExecuteRemoteFunction(clientID, pluginID, functionID, arguments)
                    : await api.ExecuteLocalFunction(pluginID, functionID, arguments);
            return response;
        }

    }
}
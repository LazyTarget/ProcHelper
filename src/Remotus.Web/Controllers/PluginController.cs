using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Remotus.Web.Rendering;

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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IResponseBase<IFunctionResult> response;
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

                response = await ExecuteFunction(clientID, pluginID, functionID, arg);
                response.EnsureSuccess();
            }
            catch (Exception ex)
            {
                response = DefaultResponseBase.CreateError<IFunctionResult>(DefaultError.FromException(ex));
            }
            finally
            {
                stopwatch.Stop();
            }

            var requestGuid = Guid.NewGuid();
            if (_requests.Count >= 5)
            {
                _requests.Remove(_requests.Keys.First());
            }
            _requests[requestGuid] = response;


            var metadata = response as IResponseMetadata;
            if (metadata != null)
            {
                metadata.Metadata["RequestGuid"] = requestGuid;
                metadata.Metadata["ExecutionTime"] = stopwatch.Elapsed;
            }

            MediaTypeFormatter formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = serializer.GetSerializerSettings(),
            };
            MediaTypeHeaderValue mediaType = formatter.SupportedMediaTypes.FirstOrDefault();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(Request.HttpMethod), Request.Url);
            var actionResult = new FormattedContentResult<IResponseBase<IFunctionResult>>(HttpStatusCode.OK, response, formatter, mediaType, request);
            return actionResult;
        }


        private static Dictionary<Guid, IResponseBase<IFunctionResult>> _requests = new Dictionary<Guid, IResponseBase<IFunctionResult>>(5);
        private static Dictionary<Guid, IObjectRenderer> _renderers = new Dictionary<Guid, IObjectRenderer>
        {
            { Guid.Parse("37E3938B-9A56-4214-B12C-2BB94A90A5BE"), new HtmlObjectRenderer() },
            { Guid.Parse("B34793F1-EDBC-4A86-BD28-EC6CD7F97237"), HtmlObjectRenderer.Default },
            { Guid.Parse("C781DE9B-56C3-45E1-B1B7-A0D77E640B9D"), new JsonObjectRenderer() },
        };


        public async Task<ActionResult> ExecutePartial(string clientID, string pluginID, string functionID)
        {
            var result = await Execute(clientID, pluginID, functionID);
            var model = result?.Content;
            return PartialView("_FunctionResult", model);
        }

        [HttpPost]
        public async Task<ActionResult> FormatResult(Guid? requestGuid, Guid? formatterGuid)
        {
            IObjectRenderer renderer = formatterGuid.HasValue && _renderers.ContainsKey(formatterGuid.Value) ? _renderers[formatterGuid.Value] : null;
            IResponseBase<IFunctionResult> resp = requestGuid.HasValue && _requests.ContainsKey(requestGuid.Value) ? _requests[requestGuid.Value] : null;
            object value = resp?.Result?.Result;

            var sb = new StringBuilder();
            try
            {
                if (renderer != null)
                {
                    var reference = new Lux.Model.ObjectModel();
                    reference.DefineProperty("Value", null, value, true);
                    reference.DefineProperty("Renderer", typeof(IObjectRenderer), renderer, true);

                    using (var textWriter = new StringWriter(sb))
                    {
                        renderer.Render(textWriter, reference);
                    }
                }
                else
                {
                    var str = value?.ToString();
                    sb.Append(str);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            var html = sb.ToString();

            var actionResult = new ContentResult();
            actionResult.Content = html;
            actionResult.ContentType = "text/html";
            return actionResult;
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
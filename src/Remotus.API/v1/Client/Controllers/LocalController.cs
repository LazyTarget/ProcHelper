using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web.Http;
using Lux.Extensions;
using Newtonsoft.Json;
using Remotus.API.v1.Models;
using Remotus.Base;

namespace Remotus.API.v1.Client.Controllers
{
    [ControllerCategory("Client")]
    public class LocalController : ApiController
    {
        protected ResponseFactory ResponseFactory => new ResponseFactory();


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/local/plugins")]
        public async Task<IResponseBaseActionResult> GetPlugins()
        {
            IEnumerable<IPlugin> result = null;
            try
            {
                result = Program.Service?.Client?.GetPlugins();
                
                var response = ResponseFactory.CreateResponse<IEnumerable<IPlugin>>(this, result: result);
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
            catch (Exception ex)
            {
                var response = ResponseFactory.CreateResponse<IEnumerable<IPlugin>>(this, error: DefaultError.FromException(ex));
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
        }

        
        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/local/plugins/function")]
        public async Task<IResponseBaseActionResult> GetFunctionPlugins()
        {
            IEnumerable<IFunctionPlugin> result = null;
            try
            {
                result = Program.Service?.Client?.GetPlugins().OfType<IFunctionPlugin>();

                var response = ResponseFactory.CreateResponse<IEnumerable<IFunctionPlugin>>(this, result: result);
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
            catch (Exception ex)
            {
                var response = ResponseFactory.CreateResponse<IEnumerable<IFunctionPlugin>>(this, error: DefaultError.FromException(ex));
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/local/plugins/service")]
        public async Task<IResponseBaseActionResult> GetServicePlugins()
        {
            IEnumerable<IServicePlugin> result = null;
            try
            {
                result = Program.Service?.Client?.GetPlugins().OfType<IServicePlugin>();

                var response = ResponseFactory.CreateResponse<IEnumerable<IServicePlugin>>(this, result: result);
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
            catch (Exception ex)
            {
                var response = ResponseFactory.CreateResponse<IEnumerable<IServicePlugin>>(this, error: DefaultError.FromException(ex));
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
        }



        [HttpPost, HttpPut]
        [Route("api/v1/local/execute/function")]
        public async Task<IResponseBaseActionResult> ExecuteFunction(string pluginName, string functionName)
        {
            try
            {
                var pluginResponse = await GetFunctionPlugins();
                if (pluginResponse?.Response?.Error != null && !pluginResponse.Response.Error.Handled)
                {
                    var response = ResponseFactory.CreateResponse<IFunctionResult>(this, error: pluginResponse.Response.Error);
                    var actionResult = ResponseFactory.CreateActionResult(this, response);
                    return actionResult;
                }

                var plugin = pluginResponse?.Response?.Result?.CastAs<IEnumerable<IFunctionPlugin>>().FirstOrDefault(x => x.Name == pluginName);
                if (plugin == null)
                {
                    throw new Exception($"Plugin '{pluginName}' not found");
                }
                
                if (plugin is IFunctionPlugin)
                {
                    var functionPlugin = (IFunctionPlugin)plugin;
                    var functions = functionPlugin.GetFunctions();
                    var functionDescriptor = functions.FirstOrDefault(x => x.Name == functionName);
                    if (functionDescriptor == null)
                    {
                        throw new Exception($"Function '{functionName}' not found");
                    }
                    
                    var parameters = functionDescriptor.GetParameters();
                    foreach (var param in parameters)
                    {
                        //var str = Request.Form["Param_" + param.Key];
                        //if (str != null)
                        //{
                        //    var val = Converter.Convert(str, param.Value.Type);
                        //    param.Value.Value = val;
                        //}
                    }

                    IFunctionArguments arg = new FunctionArguments();
                    arg.Parameters = parameters;


                    var serializer = Configuration.Formatters.JsonFormatter.CreateJsonSerializer();
                    var json = await Request.Content.ReadAsStringAsync();
                    var sentArgs = serializer.Deserialize<IFunctionArguments>(new JsonTextReader(new StringReader(json)));
                    arg = sentArgs;
                    // todo: decide whether base arguments on what the descriptor says

                    
                    var function = functionDescriptor.Instantiate();
                    var result = await ExecuteFunction(function, arg);
                    
                    var response = ResponseFactory.CreateResponse<IFunctionResult>(this, result: result);
                    var actionResult = ResponseFactory.CreateActionResult(this, response);
                    return actionResult;
                }
                else
                {
                    throw new Exception($"Plugin '{functionName}' is not a function plugin");
                }
            }
            catch (Exception ex)
            {
                var response = ResponseFactory.CreateResponse<IFunctionResult>(this, error: DefaultError.FromException(ex));
                var actionResult = ResponseFactory.CreateActionResult(this, response);
                return actionResult;
            }
        }


        private async Task<IFunctionResult> ExecuteFunction(IFunction function, IFunctionArguments arguments)
        {
            try
            {
                IExecutionContext context = new ExecutionContext
                {
                    ClientInfo = Program.Service?.Client?.ClientInfo,
                    Logger = new TraceLogger(),
                };

                var result = await function.Execute(context, arguments);
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }
        
    }
}

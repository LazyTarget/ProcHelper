using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using FullCtrl.API.Data;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1.Controllers
{
    public class LocalController : BaseController
    {
        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/local/plugins")]
        public async Task<IResponseBase<IEnumerable<IPlugin>>> GetPlugins()
        {
            IEnumerable<IPlugin> result = null;
            try
            {
                var database = new PluginLoader();

                result = await database.GetPlugins();
                var response = CreateResponse(result);
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<IEnumerable<IPlugin>>(DefaultError.FromException(ex));
                return response;
            }
        }


        [HttpPost, HttpPut]
        [Route("api/v1/local/execute/function")]
        public async Task<IResponseBase<IFunctionResult>> ExecuteFunction(string pluginName, string functionName)
        {
            try
            {
                var pluginResponse = await GetPlugins();
                if (pluginResponse?.Error != null && !pluginResponse.Error.Handled)
                {
                    var response = CreateError<IFunctionResult>(pluginResponse.Error);
                    return response;
                }

                var plugin = pluginResponse?.Result?.FirstOrDefault(x => x.Name == pluginName);
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

                    
                    var function = functionDescriptor.Instantiate();
                    var result = await function.Execute(arg);

                    var response = CreateResponse(result);
                    return response;
                }
                else
                {
                    throw new Exception($"Plugin '{functionName}' is not a function plugin");
                }
            }
            catch (Exception ex)
            {
                var response = CreateError<IFunctionResult>(DefaultError.FromException(ex));
                return response;
            }
        }
        
    }
}

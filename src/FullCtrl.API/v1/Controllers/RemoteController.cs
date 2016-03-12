using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class RemoteController : BaseController
    {
        protected v1.FullCtrlAPI GetLocalApi(string clientID)
        {
            // todo: get client info from db?
            IClientInfo clientInfo = new ClientInfo
            {
                ClientID = clientID,
                ApiAddress = new Uri($"http://{Environment.MachineName}:9000/api/v1/"),
            };

            var localApi = new v1.FullCtrlAPI();
            if (clientInfo?.ApiAddress != null)
                localApi.BaseUri = clientInfo.ApiAddress;
            else
                throw new InvalidOperationException($"No api address defined for client '{clientID}'");
            return localApi;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/remote/plugins")]
        public async Task<IResponseBase<IEnumerable<IPlugin>>> GetPlugins(string clientID)
        {
            try
            {
                var localApi = GetLocalApi(clientID);
                var response = await localApi.GetLocalPlugins();
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<IEnumerable<IPlugin>>(DefaultError.FromException(ex));
                return response;
            }
        }


        [HttpPost, HttpPut]
        [Route("api/v1/remote/execute/function")]
        public async Task<IResponseBase<IFunctionResult>> ExecuteFunction(string clientID, string pluginName, string functionName, [FromBody] string body, IFunctionArguments arguments)
        {
            try
            {
                var localApi = GetLocalApi(clientID);
                var response = await localApi.ExecuteLocalFunction(pluginName, functionName, arguments);
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<IFunctionResult>(DefaultError.FromException(ex));
                return response;
            }
        }


    }
}

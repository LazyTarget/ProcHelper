using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FullCtrl.API.Models;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class RemoteController : BaseController
    {
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


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/remote/plugins/function")]
        [Route("api/v1/remote/plugins/function/{clientID}")]
        public async Task<IResponseBase<IEnumerable<IFunctionPlugin>>> GetFunctionPlugins(string clientID)
        {
            try
            {
                var localApi = GetLocalApi(clientID);
                var response = await localApi.GetLocalFunctionPlugins();
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<IEnumerable<IFunctionPlugin>>(DefaultError.FromException(ex));
                return response;
            }
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/remote/plugins")]
        public async Task<IResponseBase<IEnumerable<IServicePlugin>>> GetServicePlugins(string clientID)
        {
            try
            {
                var localApi = GetLocalApi(clientID);
                var response = await localApi.GetLocalServicePlugins();
                return response;
            }
            catch (Exception ex)
            {
                var response = CreateError<IEnumerable<IServicePlugin>>(DefaultError.FromException(ex));
                return response;
            }
        }



        [HttpPost, HttpPut]
        [Route("api/v1/remote/execute/function")]
        public async Task<IResponseBase<IFunctionResult>> ExecuteFunction(string clientID, string pluginName, string functionName, IFunctionArguments arguments)
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



        
        protected IClientInfo LoadClientInfo(string clientID)
        {
            // todo: get client info from db?
            IClientInfo clientInfo = new ClientInfo
            {
                ClientID = clientID,
                ApiAddress = new Uri($"http://{Environment.MachineName}:9000/api/v1/"),
            };
            return clientInfo;
        }


        protected v1.FullCtrlAPI GetLocalApi(string clientID)
        {
            var factory = new ApiFactory();
            var clientInfo = LoadClientInfo(clientID);
            var localApi = factory.Create_V1(clientInfo);
            return localApi;
        }
    }
}

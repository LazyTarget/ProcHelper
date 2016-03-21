using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;
using RestSharp;

namespace Remotus.API.v1
{
    public partial class FullCtrlAPI
    {
        public async Task<IResponseBase<IEnumerable<IPlugin>>> GetRemotePlugins(string clientID)
        {
            var uri = new Uri("remote/plugins", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);
            request.AddQueryParameter("clientID", clientID);

            var response = await GetResponse<IEnumerable<IPlugin>>(request);
            return response;
        }
        
        public async Task<IResponseBase<IEnumerable<IFunctionPlugin>>> GetRemoteFunctionPlugins(string clientID)
        {
            var uri = new Uri("remote/plugins/function", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);
            request.AddQueryParameter("clientID", clientID);

            var response = await GetResponse<IEnumerable<IFunctionPlugin>>(request);
            return response;
        }
        
        public async Task<IResponseBase<IEnumerable<IServicePlugin>>> GetRemoteServicePlugins(string clientID)
        {
            var uri = new Uri("remote/plugins/service", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);
            request.AddQueryParameter("clientID", clientID);

            var response = await GetResponse<IEnumerable<IServicePlugin>>(request);
            return response;
        }


        public async Task<IResponseBase<IFunctionResult>> ExecuteRemoteFunction(string clientID, string pluginID, string functionID, IFunctionArguments arguments)
        {
            var uri = new Uri("remote/execute/function", UriKind.Relative);
            var request = BuildRequest(uri, Method.POST);
            request.AddQueryParameter("clientID", clientID);
            request.AddQueryParameter("pluginID", pluginID);
            request.AddQueryParameter("functionID", functionID);
            request.AddJsonBody(arguments);

            var response = await GetResponse<IFunctionResult>(request);
            return response;
        }

    }
}

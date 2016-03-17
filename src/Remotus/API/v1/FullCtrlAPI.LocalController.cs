using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;
using RestSharp;

namespace Remotus.API.v1
{
    public partial class FullCtrlAPI
    {
        public async Task<IResponseBase<IEnumerable<IPlugin>>> GetLocalPlugins()
        {
            var uri = new Uri("local/plugins", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);

            var response = await GetResponse<IEnumerable<IPlugin>>(request);
            return response;
        }
        
        public async Task<IResponseBase<IEnumerable<IFunctionPlugin>>> GetLocalFunctionPlugins()
        {
            var uri = new Uri("local/plugins/function", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);

            var response = await GetResponse<IEnumerable<IFunctionPlugin>>(request);
            return response;
        }
        
        public async Task<IResponseBase<IEnumerable<IServicePlugin>>> GetLocalServicePlugins()
        {
            var uri = new Uri("local/plugins/service", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);

            var response = await GetResponse<IEnumerable<IServicePlugin>>(request);
            return response;
        }


        public async Task<IResponseBase<IFunctionResult>> ExecuteLocalFunction(string pluginName, string functionName, IFunctionArguments arguments)
        {
            var uri = new Uri("local/execute/function", UriKind.Relative);
            var request = BuildRequest(uri, Method.POST);
            request.AddQueryParameter("pluginName", pluginName);
            request.AddQueryParameter("functionName", functionName);
            request.AddJsonBody(arguments);

            var response = await GetResponse<IFunctionResult>(request);
            return response;
        }

    }
}

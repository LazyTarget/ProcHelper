using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FullCtrl.Base;
using RestSharp;

namespace FullCtrl.API.v1
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

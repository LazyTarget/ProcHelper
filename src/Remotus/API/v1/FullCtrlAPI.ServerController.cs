using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;
using RestSharp;

namespace Remotus.API.v1
{
    public partial class FullCtrlAPI
    {
        public async Task<IResponseBase<IEnumerable<IClientInfo>>> GetClients()
        {
            var uri = new Uri("server/clients", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);

            var response = await GetResponse<IEnumerable<IClientInfo>>(request);
            return response;
        }
        
        public async Task<IResponseBase<object>> RegisterClient(string clientID, Uri apiAddress, int apiVersion)
        {
            var uri = new Uri("server/clients/register", UriKind.Relative);
            var request = BuildRequest(uri, Method.POST);
            request.AddQueryParameter("clientID", clientID);
            request.AddQueryParameter("apiAddress", apiAddress.ToString());
            request.AddQueryParameter("apiVersion", apiVersion.ToString());

            var response = await GetResponse<object>(request);
            return response;
        }

    }
}

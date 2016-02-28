using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FullCtrl.Base;
using FullCtrl.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace FullCtrl
{
    public class ProcessAPI : IProcessAPI
    {
        public ProcessAPI()
        {
            BaseUri = new Uri("http://localhost:9000/api/v1/");
        }

        public Uri BaseUri { get; set; }


        protected virtual IRestClient GetClient()
        {
            var jsonSerializer = new CustomJsonSerializer();
            jsonSerializer.Container.Bind(typeof(IResponseBase), typeof(DefaultResponseBase<>));
            jsonSerializer.Container.Bind(typeof(IResponseBase<>), typeof(DefaultResponseBase<>));
            jsonSerializer.Container.Bind(typeof(IError), typeof(DefaultError));
            jsonSerializer.Container.Bind(typeof(ILink), typeof(DefaultLink));
            jsonSerializer.Container.Bind(typeof(IProcessDto), typeof(ProcessDto));

            var client = new RestClient(BaseUri);
            client.AddHandler("application/json", jsonSerializer);
            client.AddHandler("text/json", jsonSerializer);
            client.AddHandler("text/x-json", jsonSerializer);
            client.AddHandler("*+json", jsonSerializer);
            client.AddHandler("*", jsonSerializer);

            //client.AddDefaultHeader("Accept", "application/json");
            return client;
        }

        protected virtual IRestRequest BuildRequest(Uri resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("accept", "application/json");
            return request;
        }

        protected async Task<IResponseBase<TResult>> GetResponse<TResult>(IRestRequest request)
        {
            IResponseBase<TResult> response;
            try
            {
                var client = GetClient();
                var httpResponse = await client.ExecuteTaskAsync<IResponseBase<TResult>>(request);
                response = httpResponse.Data;
            }
            catch (Exception ex)
            {
                response = new DefaultResponseBase<TResult>
                {
                    Error = DefaultError.FromException(ex),
                };
            }
            return response;
        }
        
        public async Task<IResponseBase<IProcessDto>> Get(int pid)
        {
            //var uri = new Uri(BaseUri, "process/get/" + pid);
            //var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var uri = new Uri("process/get/" + pid, UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);

            var response = await GetResponse<IProcessDto>(request);
            return response;
        }

        public async Task<IResponseBase<IEnumerable<IProcessDto>>> List()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResponseBase<IEnumerable<IProcessDto>>> ListByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResponseBase<StartProcessResponse>> Start(StartProcessRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResponseBase<KillProcessResponse>> Kill(KillProcessRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;
using RestSharp;

namespace Remotus.API
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
            IResponseBase<TResult> response = null;
            try
            {
                var client = GetClient();
                var httpResponse = await client.ExecuteTaskAsync<IResponseBase<TResult>>(request);
                response = httpResponse.Data;

                if (httpResponse.ResponseStatus == ResponseStatus.Error)
                {
                    var error = DefaultError.FromException(httpResponse.ErrorException);
                    if (!string.IsNullOrEmpty(httpResponse.ErrorMessage))
                        error.ErrorMessage = httpResponse.ErrorMessage;
                    response = new DefaultResponseBase<TResult>
                    {
                        Error = error,
                        Result = response != null ? response.Result : default(TResult),
                    };
                }
            }
            catch (Exception ex)
            {
                response = new DefaultResponseBase<TResult>
                {
                    Error = DefaultError.FromException(ex),
                    Result = response != null ? response.Result : default(TResult),
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
            var uri = new Uri("process/list", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);
            var response = await GetResponse<IEnumerable<IProcessDto>>(request);
            return response;
        }

        public async Task<IResponseBase<IEnumerable<IProcessDto>>> ListByName(string name)
        {
            var uri = new Uri("process/list/{name}", UriKind.Relative);
            var request = BuildRequest(uri, Method.GET);
            request.AddQueryParameter("name", name);
            var response = await GetResponse<IEnumerable<IProcessDto>>(request);
            return response;
        }

        public async Task<IResponseBase<StartProcessResponse>> Start(StartProcessRequest request)
        {
            throw new System.NotImplementedException();
        }
        
        public async Task<IResponseBase<IProcessDto>> SwitchToMainWindow(int processID)
        {
            var uri = new Uri("process/switchto/" + processID, UriKind.Relative);
            var request = BuildRequest(uri, Method.POST);
            request.AddQueryParameter("processID", processID.ToString());

            var response = await GetResponse<IProcessDto>(request);
            return response;

        }

        public async Task<IResponseBase<KillProcessResponse>> Kill(KillProcessRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
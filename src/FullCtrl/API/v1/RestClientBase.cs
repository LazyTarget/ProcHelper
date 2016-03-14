using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FullCtrl.Base;
using RestSharp;

namespace FullCtrl.API.v1
{
    public abstract class RestClientBase
    {
        protected RestClientBase()
        {
            BaseUri = new Uri("http://localhost:9000/api/v1/");
        }

        public Uri BaseUri { get; set; }


        protected virtual CustomJsonSerializer GetJsonSerializer()
        {
            var serializer = new CustomJsonSerializer();
            //serializer.Container.Bind(typeof(IResponseBase), typeof(DefaultResponseBase<>));
            //serializer.Container.Bind(typeof(IResponseBase<>), typeof(DefaultResponseBase<>));
            //serializer.Container.Bind(typeof(IError), typeof(DefaultError));
            //serializer.Container.Bind(typeof(ILink), typeof(DefaultLink));
            //serializer.Container.Bind(typeof(IProcessDto), typeof(ProcessDto));
            //serializer.Container.Bind(typeof(IParameter), typeof(Base.Parameter));
            //serializer.Container.Bind(typeof(IParameterCollection), typeof(ParameterCollection));
            //serializer.Container.Bind(typeof(IFunctionResult), typeof(FunctionResult));
            //serializer.Container.Bind(typeof(IFunctionArguments), typeof(FunctionArguments));
            //serializer.Container.Bind(typeof(IFunctionDescriptor), typeof(FunctionDescriptor));
            //serializer.Container.Bind(typeof(IPlugin), typeof(FunctionPluginDescriptor));
            //serializer.Container.Bind(typeof(IFunctionPlugin), typeof(FunctionPluginDescriptor));
            return serializer;
        }

        protected virtual IRestClient GetClient()
        {
            var jsonSerializer = GetJsonSerializer();

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
            request.JsonSerializer = GetJsonSerializer();

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

                var isSuccessCode = ((int) httpResponse.StatusCode >= 200) &&
                                    ((int) httpResponse.StatusCode <= 299);
                if (httpResponse.ErrorException != null)
                {
                    var result = httpResponse.Data != null ? httpResponse.Data.Result : default(TResult);
                    var error = DefaultError.FromException(httpResponse.ErrorException);
                    if (httpResponse.ErrorMessage != null)
                        error.ErrorMessage = httpResponse.ErrorMessage;
                    response = DefaultResponseBase.Create(result, error);
                }
                else if (!isSuccessCode)
                {
                    var result = httpResponse.Data != null ? httpResponse.Data.Result : default(TResult);
                    var error = new DefaultError
                    {
                        ErrorMessage = $"Rest request [{httpResponse.Request.Method} \"{httpResponse.ResponseUri}\" " +
                                       $"resulted in an error status code: {(int) httpResponse.StatusCode} \"{httpResponse.StatusDescription}\"",
                    };
                    response = DefaultResponseBase.Create(result, error);
                }
                else
                {

                }
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

    }
}

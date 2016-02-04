using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using FullCtrl.Base;
using FullCtrl.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FullCtrl
{
    public class ProcessAPI : IProcessAPI
    {
        public ProcessAPI()
        {
            BaseUri = new Uri("http://localhost:9000/api/v1");
        }

        public Uri BaseUri { get; set; }


        protected virtual HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1));
            client.BaseAddress = BaseUri;
            return client;
        }

        protected virtual JsonSerializer GetSerializer()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };
            settings.Converters.Add(new StringEnumConverter());

            var serializer = JsonSerializer.Create(settings);
            return serializer;
        }

        protected virtual TResult Deserialize<TResult>(string json)
        {
            TextReader textReader = new StringReader(json);
            JsonReader jsonReader = new JsonTextReader(textReader);

            var serializer = GetSerializer();
            var result = serializer.Deserialize<TResult>(jsonReader);
            return result;
        }

        protected async Task<IResponseBase<TResult>> GetResponse<TResult>(HttpRequestMessage request)
        {
            IResponseBase<TResult> response;
            try
            {
                var client = GetClient();
                var httpResponse = await client.SendAsync(request);
                var content = await httpResponse.Content.ReadAsStringAsync();
                var result = Deserialize<TResult>(content);
                
                response = new DefaultResponseBase<TResult>
                {
                    Result = result,
                };
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
            var uri = new Uri(BaseUri, "process/get/" + pid);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

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
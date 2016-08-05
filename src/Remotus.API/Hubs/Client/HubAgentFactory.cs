using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Remotus.API.Data;
using Remotus.API.Models;
using Remotus.Base;
using Remotus.Core.Net.Client;

namespace Remotus.API.Hubs.Client
{
    public class HubAgentFactory : IHubAgentFactory
    {
        private ApiConfig _apiConfig;

        public HubAgentFactory()
        {
            _apiConfig = ServiceInstance.LoadApiConfig();
            Host = _apiConfig.ServerApiAddress.Host;
            Port = _apiConfig.ServerApiAddress.Port;
        }

        public string Host { get; set; }
        public int Port { get; set; }

        
        public virtual IHubAgent Create(string hubName, ICredentials credentials, IDictionary<string, string> queryString = null)
        {
            var connection = CreateConnection(credentials, queryString);
            var connector = new HubConnector(connection);
            var hubProxy = connection.CreateHubProxy(hubName);
            var hubAgent = CreateHubAgent(hubName, hubProxy, connector);
            return hubAgent;
        }


        public virtual IHubAgentManager Create(IEnumerable<string> hubNames, ICredentials credentials, IDictionary<string, string> queryString = null)
        {
            var connection = CreateConnection(credentials);
            var connector = new HubConnector(connection);

            var result = new List<IHubAgent>();
            foreach (var hubName in hubNames)
            {
                var hubProxy = connection.CreateHubProxy(hubName);
                var hubAgent = CreateHubAgent(hubName, hubProxy, connector);
                result.Add(hubAgent);
            }
            var manager = new HubAgentManager(connector, result);
            return manager;
        }


        protected virtual HubHandshake CreateHandshake()
        {
            var handshake = new HubHandshake();
            handshake.MachineName = Environment.MachineName;
            //handshake.ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            handshake.UserName = Environment.UserName;
            handshake.UserDomainName = Environment.UserDomainName;
            handshake.AgentId = "qSdhjkZ672zzz";
            handshake.Address = new Uri("http://localhost:9000");   //_apiConfig.ClientApiAddress;
            return handshake;
        }


        protected virtual Base.Payloads.AuthCredentials CreateAuthCredentials(ICredentials credentials)
        {
            var host = Host;
            var port = Port;
            var url = $"http://{host}:{port}/signalr";
            var uri = new Uri(url);
            var creds = credentials?.GetCredential(uri, "");

            Base.Payloads.AuthCredentials authObj;
            authObj = new Base.Payloads.AuthCredentials
            {
                //UserId = "fjskDhsucC",
                UserName = Environment.UserName,
                Domain = Environment.UserDomainName,
            };
            authObj = Base.Payloads.AuthCredentials.Create(creds);
            return authObj;
        }


        protected virtual HubConnection CreateConnection(ICredentials credentials, IDictionary<string, string> queryString = null)
        {
            _apiConfig = _apiConfig ?? ServiceInstance.LoadApiConfig();

            //var host = _apiConfig.ServerApiAddress.Host;
            //var port = _apiConfig.ServerApiAddress.Port;
            var host = Host;
            var port = Port;
            var url = $"http://{host}:{port}/signalr";
            var uri = new Uri(url);

            queryString = queryString ?? new Dictionary<string, string>();
            queryString["hub-version"] = "1.0";
            queryString["machine-name"] = Environment.MachineName;
            queryString["username"] = Environment.UserName;
            queryString["UserDomainName"] = Environment.UserDomainName;


            var customJsonSerializerSettings = new CustomJsonSerializerSettings();
            var jsonSerializerSettings = customJsonSerializerSettings.Settings;
            var jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
            jsonSerializer.Formatting = Formatting.None;

            var connection = new HubConnection(url, queryString);
            connection.JsonSerializer = jsonSerializer;
            connection.CookieContainer = connection.CookieContainer ?? new CookieContainer();


            // Handshake
            var handshake = CreateHandshake();
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            jsonSerializer.Serialize(stringWriter, handshake);
            var handshakeJson = stringBuilder.ToString();
            connection.Headers["App-Handshake"] = handshakeJson;


            // Credentials
            var authObj = CreateAuthCredentials(credentials);
            if (authObj != null)
            {
                stringBuilder = new StringBuilder();
                stringWriter = new StringWriter(stringBuilder);
                jsonSerializer.Serialize(stringWriter, authObj);
                var authJson = stringBuilder.ToString();
                authJson = HttpUtility.UrlEncode(authJson);

                var authCookie = new Cookie("auth", authJson, "/", host);
                connection.CookieContainer.Add(authCookie);
            }

            return connection;
        }


        private IHubAgent CreateHubAgent(string hubName, IHubProxy hubProxy, IHubConnector connector)
        {
            var messageCache = new MessageMemoryCache();        // todo: use Dependency Resolver
            var hubAgent = new HubAgent(hubName, hubProxy, connector, messageCache);
            return hubAgent;
        }

    }
}

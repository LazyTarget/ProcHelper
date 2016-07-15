using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        }


        public IHubAgent Create(string hubName, ICredentials credentials)
        {
            var host = _apiConfig.ServerApiAddress.Host;
            var port = _apiConfig.ServerApiAddress.Port;
            var url = $"http://{host}:{port}/signalr";

            var queryString = new Dictionary<string, string>();
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
            var handshake = new HubHandshake();
            handshake.MachineName = Environment.MachineName;
            handshake.ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            handshake.UserName = Environment.UserName;
            handshake.UserDomainName = Environment.UserDomainName;
            handshake.ClientVersion = "1.0";
            handshake.ClientKey = "qSdhjkZ672zzz";
            handshake.Address = new Uri("http://localhost:9000");

            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            jsonSerializer.Serialize(stringWriter, handshake);
            var handshakeJson = stringBuilder.ToString();
            connection.Headers["App-Handshake"] = handshakeJson;


            var messageCache = new MessageMemoryCache();        // todo: use Dependency Resolver
            var hubAgent = new HubAgent(hubName, connection, messageCache);

            try
            {
                connection.Start().Wait();
                // To be done later?
            }
            catch (Exception ex)
            {
                
            }

            return hubAgent;
        }
    }
}

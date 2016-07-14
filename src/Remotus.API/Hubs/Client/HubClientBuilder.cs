using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Remotus.API.Data;
using Remotus.API.Models;
using Remotus.Base.Interfaces;
using Remotus.Base.Models.Hub;

namespace Remotus.API.Hubs.Client
{
    public class HubClientBuilder
    {
        public ClientHubManager Create<TQueue>(string host, int port, TQueue queue)
            where TQueue : IQueueEx<HubRequest>
        {
            var queryString = new Dictionary<string, string>();
            queryString["client-hub-manager-version"] = "1.0";
            
            var connectionBuilder = new HubConnectionBuilder();
            var connection = connectionBuilder.Default(host, port, queryString);

            var manager = new ClientHubManager(connection, queue);
            return manager;
        }


    }

    public class HubConnectionBuilder
    {
        public HubConnection Default(string host, int port, IDictionary<string, string> queryString = null)
        {
            var url = $"http://{host}:{port}/signalr";

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

            return connection;
        }

    }
}

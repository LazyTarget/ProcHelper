using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Remotus.API.Models;

namespace Remotus.API.Hubs
{
    public static class HubExtensions
    {
        public static HubHandshake GetHandshake(this IRequest request)
        {
            var customSerializerSettings = new CustomJsonSerializerSettings();
            var serializer = JsonSerializer.Create(customSerializerSettings.Settings);

            var s = request.Headers["App-Handshake"];
            var textReader = new StringReader(s);
            var jsonReader = new JsonTextReader(textReader);
            var handshake = serializer.Deserialize<HubHandshake>(jsonReader);
            return handshake;
        }
    }
}

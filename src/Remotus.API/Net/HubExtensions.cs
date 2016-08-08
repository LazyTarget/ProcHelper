using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Remotus.API.Models;
using Remotus.Base;

namespace Remotus.API.Net
{
    public static class HubExtensions
    {
        public static HubHandshake GetHandshake(this IRequest request)
        {
            var customSerializerSettings = new CustomJsonSerializerSettings();
            var serializer = JsonSerializer.Create(customSerializerSettings.Settings);

            var s = request.Headers["App-Handshake"];
            var handshake = !string.IsNullOrWhiteSpace(s)
                ? serializer.DeserializeJson<HubHandshake>(s)
                : null;
            return handshake;
        }
    }
}

using System.IO;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remotus.API.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            // todo: more security!
            // todo: decode/decrypt, etc.

            var cookie = request.Cookies["auth"];
            var json = cookie.Value;
            json = HttpUtility.UrlDecode(json);

            JsonSerializer serializer = request.Environment.ContainsKey("app.serializer")
                                            ? request.Environment["app.serializer"] as JsonSerializer
                                            : null;
            serializer = serializer ?? new JsonSerializer();
            var stringReader = new StringReader(json);
            var reader = new JsonTextReader(stringReader);
            var jObj = serializer.Deserialize<JObject>(reader);
            var userId = jObj.Property("UserId")?.Value?.ToObject<string>();
            return userId;
        }
    }
}

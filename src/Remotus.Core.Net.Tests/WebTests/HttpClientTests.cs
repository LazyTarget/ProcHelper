using System;
using NUnit.Framework;
using Remotus.Core.Net;
using Remotus.Base;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remotus.Core.Net.Tests.WebTests
{
    [TestFixture]
    public class HttpClientTests
    {
        [TestCase]
        public void Test1()
        {
            var log = new DebugLogger();
            var client = new WebPageRequester(log);

            var gamertag = "OvercastPit537";
            var url = "https://account.xbox.com/en-us/XboxLiveUser/GetOnlineStatus?gamertag=" + gamertag;
            var cookies = new CookieContainer();
            var response = client.Get(url, ref cookies);

            var json = JsonConvert.DeserializeObject<JObject>(response);
            Assert.IsTrue(json.Property("result")?.Value?.ToObject<bool>());
        }
    }
}

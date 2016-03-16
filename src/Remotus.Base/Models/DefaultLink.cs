using System;
using Newtonsoft.Json;

namespace FullCtrl.Base
{
    public class DefaultLink : ILink
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "relative")]
        public string Relative { get; set; }


        public Uri ToUri()
        {
            var uri = new Uri(Href);
            return uri;
        }


        public static DefaultLink FromUri(Uri uri)
        {
            var link = new DefaultLink
            {
                Href = uri.AbsoluteUri,
                Relative = uri.AbsolutePath,
            };
            return link;
        }
    }
}

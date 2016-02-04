using System;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1.Models
{
    public class Link : ILink
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


        public static Link FromUri(Uri uri)
        {
            var link = new Link
            {
                Href = uri.AbsoluteUri,
                Relative = uri.AbsolutePath,
            };
            return link;
        }
    }
}

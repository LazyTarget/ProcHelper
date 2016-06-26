using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux;
using Lux.Interfaces;
using Lux.Xml;

namespace Remotus.API
{
    public class ApiConfig : Lux.Config.Xml.XmlConfigBase
    {
        public ApiConfig()
        {
            Clients = new List<ClientEndpointConfig>();
        }

        public bool Enabled { get; set; }
        public Uri ServerApiAddress { get; set; }
        public Uri ClientApiAddress { get; set; }
        public List<string> Urls { get; set; }
        public List<ClientEndpointConfig> Clients { get; set; }

        public override void Configure(XElement element)
        {
            IConverter converter = new Converter();

            var elem = element.Elements("property").FirstOrDefault(x => x.GetAttributeValue("name") == nameof(Enabled));
            if (elem != null)
            {
                var str = elem.GetAttributeValue("value") ?? elem.Value;
                Enabled = converter.Convert<bool>(str);
            }

            elem = element.Elements("property").FirstOrDefault(x => x.GetAttributeValue("name") == nameof(ClientApiAddress));
            if (elem != null)
            {
                var str = elem.GetAttributeValue("value") ?? elem.Value;
                ClientApiAddress = converter.Convert<Uri>(str);
            }

            elem = element.Elements("property").FirstOrDefault(x => x.GetAttributeValue("name") == nameof(ServerApiAddress));
            if (elem != null)
            {
                var str = elem.GetAttributeValue("value") ?? elem.Value;
                ServerApiAddress = converter.Convert<Uri>(str);
            }

            elem = element.Elements("urls").FirstOrDefault();
            if (elem != null)
            {
                Urls = Urls ?? new List<string>();
                Urls.Clear();

                var elems = elem.Elements("url");
                foreach (var e in elems)
                {
                    var url = e.GetAttributeValue("value") ?? e.Value;
                    Urls.Add(url);
                }
            }

            elem = element.Elements("clients").FirstOrDefault();
            if (elem != null)
            {
                Clients = Clients ?? new List<ClientEndpointConfig>();
                Clients.Clear();

                var elems = elem.Elements("client");
                foreach (var e in elems)
                {
                    var client = new ClientEndpointConfig();
                    client.Configure(e);
                    Clients.Add(client);
                }
            }
        }

        public override void Export(XElement element)
        {
            
        }
    }
}

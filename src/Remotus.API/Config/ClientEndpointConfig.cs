using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux;
using Lux.Interfaces;
using Lux.Xml;

namespace Remotus.API
{
    public class ClientEndpointConfig : Lux.Config.Xml.XmlConfigBase
    {
        public bool Enabled { get; set; }
        public Uri Address { get; set; }

        public override void Configure(XElement element)
        {
            IConverter converter = new Converter();

            var elem = element.Elements("property").FirstOrDefault(x => x.GetAttributeValue("name") == nameof(Enabled));
            if (elem != null)
            {
                var str = elem.GetAttributeValue("value") ?? elem.Value;
                Enabled = converter.Convert<bool>(str);
            }

            elem = element.Elements("property").FirstOrDefault(x => x.GetAttributeValue("name") == nameof(Address));
            if (elem != null)
            {
                var str = elem.GetAttributeValue("value") ?? elem.Value;
                Address = converter.Convert<Uri>(str);
            }
        }

        public override void Export(XElement element)
        {
            
        }
    }
}

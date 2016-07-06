using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remotus.API.Models
{
    public class HubHandshake
    {
        public string ClientVersion { get; set; }
        public string ClientKey { get; set; }
        public string ApplicationVersion { get; set; }
        public string InstallationID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string UserDomainName { get; set; }
        public Uri Address { get; set; }
    }
}

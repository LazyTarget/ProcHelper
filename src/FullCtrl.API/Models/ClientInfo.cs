using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl.Base;

namespace FullCtrl.API.Models
{
    public class ClientInfo : IClientInfo
    {
        public ClientInfo()
        {
            
        }

        public string ClientID { get; set; }
        public int ApiVersion { get; set; }
        public Uri ApiAddress { get; set; }
        public IServerInfo ServerInfo { get; set; }


        [Obsolete]
        public async Task<IEnumerable<IPlugin>> GetPlugins()
        {
            var factory = new ApiFactory();
            var api = factory.Create_V1(this);
            var response = await api.GetLocalPlugins();
            response.EnsureSuccess();

            var result = response?.Result?.AsEnumerable();
            return result;
        }
    }
}

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
        public Uri ApiAddress { get; set; }


        public async Task<IEnumerable<IPlugin>> GetPlugins()
        {
            var api = API.v1.FullCtrlAPI.FromClientInfo(this);
            var response = await api.GetLocalPlugins();
            response.EnsureSuccess();

            var result = response?.Result?.AsEnumerable();
            return result;
        }
    }
}

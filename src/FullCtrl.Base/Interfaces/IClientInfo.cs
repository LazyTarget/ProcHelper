using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IClientInfo
    {
        string ClientID { get; }
        Uri ApiAddress { get; }

        Task<IEnumerable<IPlugin>> GetPlugins();
    }
}

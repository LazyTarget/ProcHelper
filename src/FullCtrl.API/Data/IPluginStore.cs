using System.Collections.Generic;
using System.Threading.Tasks;
using FullCtrl.Base;

namespace FullCtrl.API.Data
{
    public interface IPluginStore
    {
        Task<IEnumerable<IPlugin>> GetPlugins();
    }
}
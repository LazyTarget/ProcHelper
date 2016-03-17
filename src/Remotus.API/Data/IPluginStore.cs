using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.API.Data
{
    public interface IPluginStore
    {
        Task<IEnumerable<IPlugin>> GetPlugins();
    }
}
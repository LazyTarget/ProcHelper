using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface IRemotusAPI
    {
        Task<IResponseBase<IFunctionResult>> ExecuteLocalFunction(string pluginID, string functionID, IFunctionArguments arguments);
        Task<IResponseBase<IFunctionResult>> ExecuteRemoteFunction(string clientID, string pluginID, string functionID, IFunctionArguments arguments);
    }
}

using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    [Obsolete]
    public interface IPowershellAPI
    {
        Task<IResponseBase<PowershellResponse>> RunFile(string fileName, bool redirectStandardOutput = true);
        Task<IResponseBase<PowershellResponse>> RunQuery(string query, bool redirectStandardOutput = true);
    }
}

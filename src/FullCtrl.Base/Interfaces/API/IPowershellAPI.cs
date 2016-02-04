using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IPowershellAPI
    {
        Task<IResponseBase<PowershellResponse>> RunFile(string fileName, bool redirectStandardOutput = true);
        Task<IResponseBase<PowershellResponse>> RunQuery(string query, bool redirectStandardOutput = true);
    }
}

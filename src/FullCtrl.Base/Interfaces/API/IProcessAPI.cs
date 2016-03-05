using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IProcessAPI
    {
        Task<IResponseBase<IProcessDto>> Get(int pid);
        Task<IResponseBase<IEnumerable<IProcessDto>>> List();
        Task<IResponseBase<IEnumerable<IProcessDto>>> ListByName(string name);
        Task<IResponseBase<StartProcessResponse>> Start(StartProcessRequest request);
        Task<IResponseBase<IProcessDto>> SwitchToMainWindow(int processID);
        Task<IResponseBase<KillProcessResponse>> Kill(KillProcessRequest request);
    }
}

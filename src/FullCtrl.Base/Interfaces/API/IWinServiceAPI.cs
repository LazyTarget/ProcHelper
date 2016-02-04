using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IWinServiceAPI
    {
        Task<IResponseBase<WinServiceDto>> Get(string serviceName);
        Task<IResponseBase<IEnumerable<WinServiceDto>>> List();
        Task<IResponseBase<IEnumerable<WinServiceDto>>> ListByName(string serviceName);
        Task<IResponseBase<IEnumerable<WinServiceDto>>> FindByName(string serviceName);
        Task<IResponseBase<IEnumerable<WinServiceDto>>> FindByStatus(ServiceControllerStatus status);
        Task<IResponseBase<WinServiceDto>> Start(string serviceName, params string[] arguments);
        Task<IResponseBase<WinServiceDto>> Pause(string serviceName);
        Task<IResponseBase<WinServiceDto>> Continue(string serviceName);
        Task<IResponseBase<WinServiceDto>> Stop(string serviceName);
    }
}

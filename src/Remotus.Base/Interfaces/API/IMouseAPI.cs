using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    [Obsolete]
    public interface IMouseAPI
    {
        Task<IResponseBase<MouseInfo>> GetInfo();
        Task<IResponseBase<MouseInfo>> MoveMouseBy(MoveMouseBy request);
        Task<IResponseBase<MouseInfo>> MoveMouseTo(MoveMouseTo request);
        Task<IResponseBase<MouseInfo>> MoveMouseToPositionOnVirtualDesktop(MoveMouseToPositionOnVirtualDesktop request);
    }
}

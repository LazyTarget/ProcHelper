using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    [Obsolete]
    public interface IDeviceControllerAPI
    {
        Task<IResponseBase<object>> TakeScreenshot();
    }
}

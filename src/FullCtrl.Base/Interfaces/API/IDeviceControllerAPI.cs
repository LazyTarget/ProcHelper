using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IDeviceControllerAPI
    {
        Task<IResponseBase<object>> TakeScreenshot();
    }
}

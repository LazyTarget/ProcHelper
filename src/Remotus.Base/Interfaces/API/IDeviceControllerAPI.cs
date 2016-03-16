using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    [Obsolete]
    public interface IDeviceControllerAPI
    {
        Task<IResponseBase<object>> TakeScreenshot();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IFullCtrlAPI
    {
        IProcessAPI Process { get; }
        IWinServiceAPI WinService { get; }
        IPowershellAPI Powershell { get; }
        IMouseAPI Mouse { get; }
        IKeyboardAPI Keyboard { get; }
        IAudioControllerAPI AudioController { get; }
        IDeviceControllerAPI DeviceController { get; }


        Task<IEnumerable<IPlugin>> GetPlugins(string clientID);
        Task<IFunctionResult> ExecuteFunction(string clientID, string pluginName, IFunctionArguments arguments);
    }
}

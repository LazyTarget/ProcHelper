using FullCtrl.Base;

namespace FullCtrl
{
    public class FullCtrlAPI : IFullCtrlAPI
    {
        public FullCtrlAPI()
        {
            Process = new ProcessAPI();
            //WinService = new WinServiceAPI();
            //Powershell = new PowershellAPI();
            //Mouse = new MouseAPI();
            //Keyboard = new KeyboardAPI();
            AudioController = new AudioControllerAPI();
        }

        public IProcessAPI Process { get; }
        public IWinServiceAPI WinService { get; }
        public IPowershellAPI Powershell { get; }
        public IMouseAPI Mouse { get; }
        public IKeyboardAPI Keyboard { get; }
        public IAudioControllerAPI AudioController { get; }
    }
}

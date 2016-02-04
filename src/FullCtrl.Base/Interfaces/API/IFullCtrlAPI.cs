namespace FullCtrl.Base
{
    public interface IFullCtrlAPI
    {
        IProcessAPI Process { get; }
        IWinServiceAPI WinService { get; }
        IPowershellAPI Powershell { get; }
        IMouseAPI Mouse { get; }
        IKeyboardAPI Keyboard { get; }
    }
}

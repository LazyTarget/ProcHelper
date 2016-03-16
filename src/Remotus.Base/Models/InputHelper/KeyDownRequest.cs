using WindowsInput.Native;

namespace FullCtrl.Base
{
    public class KeyDownRequest : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

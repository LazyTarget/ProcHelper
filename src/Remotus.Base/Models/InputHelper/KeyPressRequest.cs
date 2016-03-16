using WindowsInput.Native;

namespace FullCtrl.Base
{
    public class KeyPressRequest : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

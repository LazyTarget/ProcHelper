using WindowsInput.Native;

namespace FullCtrl.Base
{
    public class KeyUpRequest : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

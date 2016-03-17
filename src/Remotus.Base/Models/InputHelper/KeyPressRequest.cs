using WindowsInput.Native;

namespace Remotus.Base
{
    public class KeyPressRequest : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

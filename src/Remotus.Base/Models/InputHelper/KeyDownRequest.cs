using WindowsInput.Native;

namespace Remotus.Base
{
    public class KeyDownRequest : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

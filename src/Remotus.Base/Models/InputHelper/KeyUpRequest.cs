using WindowsInput.Native;

namespace Remotus.Base
{
    public class KeyUpRequest : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

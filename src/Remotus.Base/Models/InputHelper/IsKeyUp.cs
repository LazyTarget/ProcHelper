using WindowsInput.Native;

namespace Remotus.Base
{
    public class IsKeyUp : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

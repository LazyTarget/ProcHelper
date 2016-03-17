using WindowsInput.Native;

namespace Remotus.Base
{
    public class IsKeyDown : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

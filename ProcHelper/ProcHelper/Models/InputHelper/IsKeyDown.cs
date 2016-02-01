using WindowsInput.Native;

namespace ProcHelper
{
    public class IsKeyDown : ServiceStack.IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

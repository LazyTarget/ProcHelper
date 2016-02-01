using WindowsInput.Native;

namespace ProcHelper
{
    public class IsKeyUp : ServiceStack.IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

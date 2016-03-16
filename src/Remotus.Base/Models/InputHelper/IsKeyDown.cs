using WindowsInput.Native;

namespace FullCtrl.Base
{
    public class IsKeyDown : IReturn<KeyResponse>
    {
        public VirtualKeyCode VirtualKeyCode { get; set; }
    }
}

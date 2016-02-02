using WindowsInput.Native;

namespace FullCtrl.Base
{
    public interface IInputHelper
    {
        // Mouse
        Point GetMousePosition();
        void MoveMouseBy(int pixelDeltaX, int pixelDeltaY);
        void MoveMouseTo(int absoluteX, int absoluteY);
        void MoveMouseToPositionOnVirtualDesktop(int absoluteX, int absoluteY);

        // Keyboard
        bool IsKeyDown(VirtualKeyCode keyCode);
        bool IsKeyUp(VirtualKeyCode keyCode);
        bool IsTogglingKeyInEffect(VirtualKeyCode keyCode);
        void KeyDown(VirtualKeyCode keyCode);
        void KeyUp(VirtualKeyCode keyCode);
        void KeyPress(VirtualKeyCode keyCode);
        void WriteTextEntry(string text);
    }
}

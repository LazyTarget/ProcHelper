using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;
using FullCtrl.Base;

namespace FullCtrl
{
    public class InputHelper : IInputHelper
    {
        private InputSimulator _inputSimulator;

        public InputHelper()
        {
            _inputSimulator = new InputSimulator();
        }


        public Point GetMousePosition()
        {
            var pos = Win32.GetCursorPosition();
            return pos;
        }

        public void MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
        {
            _inputSimulator.Mouse.MoveMouseBy(pixelDeltaX, pixelDeltaY);
        }

        public void MoveMouseTo(int absoluteX, int absoluteY)
        {
            _inputSimulator.Mouse.MoveMouseTo(absoluteX, absoluteY);
        }

        public void MoveMouseToPositionOnVirtualDesktop(int absoluteX, int absoluteY)
        {
            _inputSimulator.Mouse.MoveMouseToPositionOnVirtualDesktop(absoluteX, absoluteY);
        }



        public bool IsKeyDown(VirtualKeyCode keyCode)
        {
            var res = _inputSimulator.InputDeviceState.IsKeyDown(keyCode);
            return res;
        }

        public bool IsKeyUp(VirtualKeyCode keyCode)
        {
            var res = _inputSimulator.InputDeviceState.IsKeyUp(keyCode);
            return res;
        }

        public bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            var res = _inputSimulator.InputDeviceState.IsTogglingKeyInEffect(keyCode);
            return res;
        }


        public void KeyDown(VirtualKeyCode keyCode)
        {
            _inputSimulator.Keyboard.KeyDown(keyCode);
        }

        public void KeyUp(VirtualKeyCode keyCode)
        {
            _inputSimulator.Keyboard.KeyUp(keyCode);
        }

        public void KeyPress(VirtualKeyCode keyCode)
        {
            _inputSimulator.Keyboard.KeyPress(keyCode);
        }

        public void WriteTextEntry(string text)
        {
            _inputSimulator.Keyboard.TextEntry(text);
        }



        private class Win32
        {
            /// <summary>
            /// Struct representing a point.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct POINT
            {
                public int X;
                public int Y;

                public static implicit operator Point(POINT point)
                {
                    return new Point(point.X, point.Y);
                }
            }

            /// <summary>
            /// Retrieves the cursor's position, in screen coordinates.
            /// </summary>
            /// <see>See MSDN documentation for further information.</see>
            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out POINT lpPoint);

            public static Point GetCursorPosition()
            {
                POINT lpPoint;
                GetCursorPos(out lpPoint);
                //bool success = User32.GetCursorPos(out lpPoint);
                // if (!success)

                return lpPoint;
            }
        }

    }
}

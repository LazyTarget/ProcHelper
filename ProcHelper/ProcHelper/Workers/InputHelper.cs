using System.Runtime.InteropServices;
using WindowsInput;

namespace ProcHelper
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

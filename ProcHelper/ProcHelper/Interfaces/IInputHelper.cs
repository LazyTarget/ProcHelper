namespace ProcHelper
{
    public interface IInputHelper
    {
        Point GetMousePosition();
        void MoveMouseBy(int pixelDeltaX, int pixelDeltaY);
        void MoveMouseTo(int absoluteX, int absoluteY);
        void MoveMouseToPositionOnVirtualDesktop(int absoluteX, int absoluteY);
    }
}

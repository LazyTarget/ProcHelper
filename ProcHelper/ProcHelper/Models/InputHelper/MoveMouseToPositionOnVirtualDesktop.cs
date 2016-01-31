namespace ProcHelper
{
    public class MoveMouseToPositionOnVirtualDesktop : ServiceStack.IReturn<MoveMouseResponse>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

namespace Remotus.Base
{
    public class MoveMouseToPositionOnVirtualDesktop : IReturn<MoveMouseResponse>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

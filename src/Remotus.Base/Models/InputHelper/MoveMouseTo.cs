namespace Remotus.Base
{
    public class MoveMouseTo : IReturn<MoveMouseResponse>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

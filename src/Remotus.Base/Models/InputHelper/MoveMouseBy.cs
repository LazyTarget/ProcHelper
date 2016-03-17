namespace Remotus.Base
{
    public class MoveMouseBy : IReturn<MoveMouseResponse>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

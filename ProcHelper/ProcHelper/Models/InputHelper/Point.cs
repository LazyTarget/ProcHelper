namespace ProcHelper
{
    public class Point
    {
        public Point()
        {

        }

        public Point(int x, int y)
            : this()
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}

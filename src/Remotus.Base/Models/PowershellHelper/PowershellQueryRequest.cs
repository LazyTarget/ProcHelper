namespace Remotus.Base
{
    public class PowershellQueryRequest : IReturn<PowershellResponse>
    {
        public string Query { get; set; }

        public bool RedirectStandardOutput { get; set; }
    }
}

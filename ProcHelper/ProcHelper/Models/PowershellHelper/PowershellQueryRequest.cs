namespace ProcHelper
{
    public class PowershellQueryRequest : ServiceStack.IReturn<PowershellResponse>
    {
        public string Query { get; set; }

        public bool RedirectStandardOutput { get; set; }
    }
}

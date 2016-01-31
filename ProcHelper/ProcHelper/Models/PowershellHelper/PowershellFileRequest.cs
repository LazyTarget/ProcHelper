namespace ProcHelper
{
    public class PowershellFileRequest : ServiceStack.IReturn<PowershellResponse>
    {
        public string File { get; set; }

        public bool RedirectStandardOutput { get; set; }
    }
}

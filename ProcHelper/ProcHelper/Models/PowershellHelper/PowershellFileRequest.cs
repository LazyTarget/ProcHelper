namespace ProcHelper
{
    public class PowershellFileRequest : ServiceStack.IReturn<PowershellResponse>
    {
        public string FileName { get; set; }

        public bool RedirectStandardOutput { get; set; }
    }
}

namespace Remotus.Base
{
    public class PowershellFileRequest : IReturn<PowershellResponse>
    {
        public string FileName { get; set; }

        public bool RedirectStandardOutput { get; set; }
    }
}

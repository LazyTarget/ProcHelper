namespace ProcHelper
{
    public class PowershellResponse
    {
        public ServiceStack.IReturn<PowershellResponse> Request { get; set; }

        public string StandardOutput { get; set; }

        public string StandardError { get; set; }

    }
}

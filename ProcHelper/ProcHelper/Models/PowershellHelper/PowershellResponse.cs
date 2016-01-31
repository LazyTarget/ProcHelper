namespace ProcHelper
{
    public class PowershellResponse
    {
        public ServiceStack.IReturn<PowershellResponse> Request { get; set; }

        public ProcessDto ProcessInfo { get; set; }

        //public string StandardOutput
        //{
        //    get { return ProcessInfo?.StandardOutput; }
        //}

        //public string StandardError
        //{
        //    get { return ProcessInfo?.StandardError; }
        //}

    }
}

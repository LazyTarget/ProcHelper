namespace FullCtrl.Base
{
    public class PowershellResponse
    {
        public IReturn<PowershellResponse> Request { get; set; }

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

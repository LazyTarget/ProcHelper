namespace ProcHelper
{
    public class StartWinServiceRequest : ServiceStack.IReturn<StartWinServiceResponse>
    {
        public string ServiceName { get; set; }

        public string Arguments { get; set; }
    }
}
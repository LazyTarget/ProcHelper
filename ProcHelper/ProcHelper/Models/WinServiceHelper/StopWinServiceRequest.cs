namespace ProcHelper
{
    public class StopWinServiceRequest : ServiceStack.IReturn<StopWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}
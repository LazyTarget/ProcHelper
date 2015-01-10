namespace ProcHelper
{
    public class ContinueWinServiceRequest : ServiceStack.IReturn<ContinueWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}
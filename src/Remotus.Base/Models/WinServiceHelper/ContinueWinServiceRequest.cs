namespace Remotus.Base
{
    public class ContinueWinServiceRequest : IReturn<ContinueWinServiceResponse>
    {
        public string ServiceName { get; set; }
    }
}
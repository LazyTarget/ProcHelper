namespace Remotus.Base
{
    public class GetWinServicesRequest : IReturn<WinServicesResponse>
    {
        public string Name { get; set; }
    }
}
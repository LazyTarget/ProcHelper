namespace ProcHelper
{
    public class GetWinServicesRequest : ServiceStack.IReturn<WinServicesResponse>
    {
        public string Name { get; set; }
    }
}
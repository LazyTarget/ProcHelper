namespace FullCtrl.Base
{
    public class GetWinServicesRequest : IReturn<WinServicesResponse>
    {
        public string Name { get; set; }
    }
}
namespace FullCtrl.Base
{
    public interface IPowershellHelper
    {
        PowershellResponse RunFile(PowershellFileRequest request);
        PowershellResponse RunQuery(PowershellQueryRequest request);
    }
}

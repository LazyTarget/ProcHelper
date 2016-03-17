namespace Remotus.Base
{
    public interface IPowershellHelper
    {
        PowershellResponse RunFile(PowershellFileRequest request);
        PowershellResponse RunQuery(PowershellQueryRequest request);
    }
}

namespace ProcHelper
{
    public interface IPowershellHelper
    {
        PowershellResponse RunQuery(PowershellQueryRequest request);
    }
}

namespace FullCtrl.Base
{
    public interface IExecutionContext
    {
        IClientInfo ClientInfo { get; }
        ILog Logger { get; }
    }
}

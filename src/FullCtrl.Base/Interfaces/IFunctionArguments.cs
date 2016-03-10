namespace FullCtrl.Base
{
    public interface IFunctionArguments
    {
        IParameterCollection Parameters { get; set; }
        IRemoteConfiguration RemoteConfig { get; set; }
    }
}
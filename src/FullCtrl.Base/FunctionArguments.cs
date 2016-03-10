namespace FullCtrl.Base
{
    public class FunctionArguments : IFunctionArguments
    {
        public FunctionArguments()
        {
            Parameters = new ParameterCollection();
        }

        public IParameterCollection Parameters { get; set; }
        public IRemoteConfiguration RemoteConfig { get; set; }
    }
}
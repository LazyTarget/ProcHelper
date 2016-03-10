namespace FullCtrl.Base
{
    public interface IFunctionDescriptor
    {
        string Name { get; }
        // todo: more metadata, such as logo

        bool CanExecuteRemotely { get; }

        IParameterCollection GetParameters();
        IFunction Instantiate();
    }
}
namespace Remotus.Base
{
    public class CommandResult : ICommandResult
    {
        //public IFunctionArguments Arguments { get; set; }
        public IError Error { get; set; }
        public object Result { get; set; }

        // todo:
    }
}

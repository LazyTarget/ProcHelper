using System.Threading.Tasks;

namespace Remotus.Base.Scripting
{
    public abstract class ScriptTaskBase
    {
        public abstract string Name { get; }
        public abstract Task<IResponseBase> Execute(IExecutionContext context);
    }
}
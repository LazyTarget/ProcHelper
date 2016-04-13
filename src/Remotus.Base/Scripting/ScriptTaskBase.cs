using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Remotus.Base.Scripting
{
    [Serializable]
    [XmlInclude(typeof(ExecuteFunctionScriptTask))]
    public abstract class ScriptTaskBase
    {
        public abstract string Name { get; }
        public abstract Task<IResponseBase> Execute(IExecutionContext context, IParameterCollection parameterCollection);
    }
}
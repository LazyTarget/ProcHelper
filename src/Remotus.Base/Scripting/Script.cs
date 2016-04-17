using System.Xml.Serialization;

namespace Remotus.Base.Scripting
{
    [XmlInclude(typeof(ExecuteFunctionScriptTask))]
    public class Script : IComponentDescriptor
    {
        public Script()
        {
            
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        [XmlArray(nameof(Tasks), IsNullable = true)]
        [XmlArrayItem("ExecuteFunctionTask", typeof(ExecuteFunctionScriptTask))]
        [XmlArrayItem("Task", typeof(ScriptTaskBase))]
        //public ScriptTaskBase[] Tasks { get; set; }
        public ExecuteFunctionScriptTask[] Tasks { get; set; }
    }
}
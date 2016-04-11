namespace Remotus.Base.Scripting
{
    public class Script : IComponentDescriptor
    {
        public Script()
        {
            
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public ScriptTaskBase[] Tasks { get; set; }
    }
}
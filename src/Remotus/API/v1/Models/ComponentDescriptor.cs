using System;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ComponentDescriptor : IComponentDescriptor
    {
        public ComponentDescriptor()
        {
            
        }

        public ComponentDescriptor(IComponentDescriptor descriptor)
            : this()
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            ID = descriptor.ID;
            Name = descriptor.Name;
            Version = descriptor.Version;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}

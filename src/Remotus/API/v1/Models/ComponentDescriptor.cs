using System;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ComponentDescriptor : IComponentDescriptor
    {
        public ComponentDescriptor()
        {
            
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }


        public static ComponentDescriptor Create(IComponentDescriptor descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor));
            var result = new ComponentDescriptor();
            result.ID = descriptor.ID;
            result.Name = descriptor.Name;
            result.Version = descriptor.Version;
            return result;
        }
    }
}

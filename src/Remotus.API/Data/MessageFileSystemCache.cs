using System;
using Remotus.Base;
using Remotus.Base.Models.Hub;

namespace Remotus.API.Data
{
    [Obsolete("Not yet implemented")]
    public class MessageFileSystemCache : IMessageCache
    {
        public MessageFileSystemCache(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; private set; }


        public void Enqueue(HubRequest item)
        {
            // todo: serialize to file...

            throw new NotImplementedException();
        }

        public bool TryDequeue(out HubRequest item)
        {
            // todo: deserialize file, also removing it...

            throw new NotImplementedException();
        }
    }
}
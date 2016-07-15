using System;
using Remotus.Base;

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


        public void Enqueue(IHubMessage item)
        {
            // todo: serialize to file...

            throw new NotImplementedException();
        }

        public bool TryDequeue(out IHubMessage item)
        {
            // todo: deserialize file, also removing it...

            throw new NotImplementedException();
        }
    }
}
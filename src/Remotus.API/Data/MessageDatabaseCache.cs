using System;
using Remotus.Base;

namespace Remotus.API.Data
{
    [Obsolete("Not yet implemented")]
    public class MessageDatabaseCache : IMessageCache
    {
        public MessageDatabaseCache()
        {

        }


        public void Enqueue(IHubMessage item)
        {
            // todo: serialize and store in some database...

            throw new NotImplementedException();
        }

        public bool TryDequeue(out IHubMessage item)
        {
            // todo: read, deserialize and remove from database...

            throw new NotImplementedException();
        }
    }
}
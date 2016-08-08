using System;
using Remotus.Base;
using Remotus.Base.Models.Hub;

namespace Remotus.API.Data
{
    [Obsolete("Not yet implemented")]
    public class MessageDatabaseCache : IMessageCache
    {
        public MessageDatabaseCache()
        {

        }


        public bool Any()
        {
            throw new NotImplementedException();
        }

        public void Enqueue(HubRequest item)
        {
            // todo: serialize and store in some database...

            throw new NotImplementedException();
        }

        public bool TryDequeue(out HubRequest item)
        {
            // todo: read, deserialize and remove from database...

            throw new NotImplementedException();
        }
    }
}
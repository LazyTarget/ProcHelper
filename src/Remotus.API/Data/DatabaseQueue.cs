using System;
using Remotus.Base;

namespace Remotus.API.Data
{
    [Obsolete("Not yet implemented")]
    public class DatabaseQueue<T> : IQueueEx<T>
    {
        public DatabaseQueue()
        {

        }


        public void Enqueue(T item)
        {
            // todo: serialize and store in some database...

            throw new NotImplementedException();
        }

        public bool TryDequeue(out T item)
        {
            // todo: read, deserialize and remove from database...

            throw new NotImplementedException();
        }
    }
}
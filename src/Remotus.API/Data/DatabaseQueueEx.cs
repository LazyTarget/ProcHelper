using System;

namespace Remotus.API.Data
{
    [Obsolete("Not yet implemented")]
    public class DatabaseQueueEx<T> : IQueueEx<T>
    {
        public DatabaseQueueEx()
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
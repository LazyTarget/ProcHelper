using System;
using Remotus.Base;

namespace Remotus.API.Data
{
    [Obsolete("Not yet implemented")]
    public class FileSystemQueue<T> : IQueueEx<T>
    {
        public FileSystemQueue(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; private set; }


        public void Enqueue(T item)
        {
            // todo: serialize to file...

            throw new NotImplementedException();
        }

        public bool TryDequeue(out T item)
        {
            // todo: deserialize file, also removing it...

            throw new NotImplementedException();
        }
    }
}
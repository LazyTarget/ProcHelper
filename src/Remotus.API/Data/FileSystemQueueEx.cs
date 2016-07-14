using System;

namespace Remotus.API.Data
{
    public class FileSystemQueueEx<T> : IQueueEx<T>
    {
        public FileSystemQueueEx(string directory)
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
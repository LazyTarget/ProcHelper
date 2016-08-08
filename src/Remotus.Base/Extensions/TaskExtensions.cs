using System;
using System.Threading;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public static class TaskExtensions
    {
        private static TimeSpan _defaultTimeout = TimeSpan.FromSeconds(15);

        public static void TryWait(this Task task)
        {
            TryWait(task, _defaultTimeout);
        }

        public static void TryWait(this Task task, TimeSpan timeout)
        {
            try
            {
                if (timeout >= TimeSpan.Zero)
                    task?.Wait(timeout);
                else
                    task?.Wait();
            }
            catch (Exception ex)
            {

            }
        }



        public static void TryWaitAsync(this Task task)
        {
            TryWaitAsync(task, _defaultTimeout);
        }

        public static void TryWaitAsync(this Task task, TimeSpan timeout)
        {
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                TryWait(task, timeout);
            });
        }

    }
}
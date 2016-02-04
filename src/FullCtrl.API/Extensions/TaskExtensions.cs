using System.Threading.Tasks;

namespace FullCtrl.API
{
    public static class TaskExtensions
    {
        public static TResult WaitForResult<TResult>(this Task<TResult> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}

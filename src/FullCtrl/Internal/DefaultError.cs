using System;
using FullCtrl.Base;

namespace FullCtrl.Internal
{
    public class DefaultError : IError
    {
        public DefaultError(Exception exception)
            : this(exception.Message)
        {
            Exception = exception;
        }

        public DefaultError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public Exception Exception { get; private set; }


        public static DefaultError FromException(Exception exception)
        {
            var error = new DefaultError(exception);
            return error;
        }
    }
}

using System;

namespace FullCtrl.Base
{
    public class DefaultError : IError
    {
        public DefaultError()
        {

        }

        public string ErrorMessage { get; set; }

        public Exception Exception { get; set; }


        public static DefaultError FromException(Exception exception)
        {
            var error = new DefaultError();
            error.Exception = exception;
            error.ErrorMessage = exception.Message;
            return error;
        }
    }
}

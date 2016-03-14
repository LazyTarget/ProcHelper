using System;

namespace FullCtrl.Base
{
    public class DefaultError : IError
    {
        public DefaultError()
        {

        }

        public DefaultError(Exception exception)
            : this()
        {
            Exception = exception;
            ErrorMessage = exception?.Message;
        }

        public DefaultError(string errorMessage)
            : this()
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }

        public bool Handled { get; set; }

        public Exception Exception { get; set; }

        public void Throw()
        {
            if (Exception != null)
                throw Exception;
            throw new Exception(ErrorMessage);
        }


        public static DefaultError FromException(Exception exception)
        {
            var error = new DefaultError();
            error.Exception = exception;
            error.ErrorMessage = exception.Message;
            return error;
        }

        public static DefaultError FromException(Exception exception, string errorMessage)
        {
            var error = new DefaultError();
            error.Exception = exception;
            error.ErrorMessage = errorMessage ?? exception?.Message;
            return error;
        }
    }
}

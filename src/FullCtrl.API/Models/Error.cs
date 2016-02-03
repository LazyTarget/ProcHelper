using System;
using FullCtrl.API.Interfaces;

namespace FullCtrl.API.Models
{
    public class Error : IError
    {
        public Error(Exception exception)
            : this(exception.Message)
        {
            Exception = exception;
        }

        public Error(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public Exception Exception { get; private set; }


        public static Error FromException(Exception exception)
        {
            var error = new Error(exception);
            return error;
        }
    }
}

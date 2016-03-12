using System;
using FullCtrl.API.Interfaces;
using FullCtrl.Base;

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

        public bool Handled { get; set; }

        public Exception Exception { get; private set; }

        public void Throw()
        {
            if (Exception != null)
                throw Exception;
            throw new Exception(ErrorMessage);
        }


        public static Error FromException(Exception exception)
        {
            var error = new Error(exception);
            return error;
        }
    }
}

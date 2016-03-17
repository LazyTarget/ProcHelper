using System;
using System.Linq.Expressions;
using Lux;
using Lux.Interfaces;

namespace Remotus.Base
{
    public static class Extensions
    {
        public static TProp TryGetProp<TObj, TProp>(this TObj obj, Expression<Func<TObj, TProp>> lamda)
        {
            try
            {
                var value = lamda.Compile().Invoke(obj);
                return value;
            }
            catch (Exception ex)
            {
                return default(TProp);
            }
        }



        public static TValue GetParamValue<TValue>(this IParameterCollection collection, string parameterName)
        {
            var result = default(TValue);
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName), "Invalid parameter name");
            }

            try
            {
                if (collection != null && collection.ContainsKey(parameterName))
                {
                    var param = collection[parameterName];
                    if (param != null)
                    {
                        IConverter converter = new Converter();
                        result = converter.Convert<TValue>(param.Value);
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                
                }
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }


        public static IParameter SetParamValue(this IParameterCollection collection, string parameterName, object value)
        {
            IParameter result = null;
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName), "Invalid parameter name");
            }

            try
            {
                if (collection != null && collection.ContainsKey(parameterName))
                {
                    result = collection[parameterName];
                    if (result != null)
                    {
                        var type = value?.GetType();
                        if (type != null && result.Type != null)
                        {
                            if (type != result.Type)
                            {
                                IConverter converter = new Converter();
                                value = converter.Convert(value, result.Type);
                            }
                        }

                        result.Value = value;
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }



        public static void ThrowIfNullResponse(this IResponseBase response)
        {
            if (response == null)
                throw new Exception("Request resulted in a null response");
        }

        public static void ThrowIfNullResult(this IResponseBase response)
        {
            if (response?.Result == null)
                throw new Exception("Request resulted in a null response result");
        }

        public static void ThrowIfError(this IResponseBase response)
        {
            if (response?.Error != null)
                response.Error.Throw();
        }

        public static void EnsureSuccess(this IResponseBase response)
        {
            try
            {
                response.ThrowIfError();
                response.ThrowIfNullResponse();
                response.ThrowIfNullResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
using System;
using System.Linq.Expressions;

namespace FullCtrl.Base
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

    }
}
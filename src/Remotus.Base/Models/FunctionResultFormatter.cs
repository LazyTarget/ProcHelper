using System;

namespace Remotus.Base
{
    public class FunctionResultFormatter
    {
        public virtual ResultObject Format(object value)
        {
            try
            {
                var result = new ResultObject(value);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class ResultObject : Lux.Model.MirrorObjectModel
        {
            public ResultObject(object instance)
                : base(instance)
            {

            }
        }
    }
}

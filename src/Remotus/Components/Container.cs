using System;
using System.Collections.Generic;

namespace Remotus
{
    public class Container : IContainer
    {
        public static readonly Container Default = new Container();


        public readonly IDictionary<Type, Type> _mappings = new Dictionary<Type, Type>();


        public virtual void Bind(Type objectType, Type targetType)
        {
            _mappings[objectType] = targetType;
        }

        public virtual Type ResolveType(Type objectType)
        {
            var type = objectType;
            if (_mappings.ContainsKey(objectType))
                type = _mappings[objectType];
            return type;
        }


        public virtual object Resolve(Type objectType)
        {
            var targetType = ResolveType(objectType);
            var result = Activator.CreateInstance(targetType);
            return result;
        }

        public virtual T Resolve<T>()
        {
            var type = typeof (T);
            var obj = Resolve(type);
            var result = (T)obj;
            return result;
        }

        public virtual bool IsBound(Type objectType)
        {
            return _mappings.ContainsKey(objectType);
        }

    }
}
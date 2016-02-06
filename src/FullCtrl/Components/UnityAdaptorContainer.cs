using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace FullCtrl
{
    public class UnityAdaptorContainer : IContainer
    {
        private readonly IUnityContainer _container;

        public UnityAdaptorContainer()
        {
            _container = new UnityContainer();
        }

        public void Bind(Type objectType, Type targetType)
        {
            var injectionMembers = new List<InjectionMember>();
            var constructors = targetType.GetConstructors();
            if (constructors.Length > 1)
            {
                var hasParameterLessCtor = constructors.Any(x => !x.GetParameters().Any());
                if (hasParameterLessCtor)
                    injectionMembers.Add(new InjectionConstructor());
            }
            
            if (injectionMembers.Any())
                _container.RegisterType(objectType, targetType, injectionMembers.ToArray());
            else
                _container.RegisterType(objectType, targetType);
        }

        public bool IsBound(Type objectType)
        {
            //var res = _container.IsRegistered(objectType);
            //if (!res && objectType.IsConstructedGenericType)
            //    res = _container.IsRegistered(objectType.GetGenericTypeDefinition());
            //return res;

            var targetType = ResolveType(objectType);
            var res = targetType != null;
            return res;
        }

        public object Resolve(Type objectType)
        {
            var result = _container.Resolve(objectType);
            return result;
        }

        public Type ResolveType(Type objectType)
        {
            var result = _container.Registrations.Where(x =>
            {
                var res = x.RegisteredType == objectType;
                if (!res && objectType.IsConstructedGenericType)
                    res = x.RegisteredType == objectType.GetGenericTypeDefinition();
                return res;
            }).Select(x => x.MappedToType).FirstOrDefault();
            return result;
        }
    }
}

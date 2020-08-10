using Castle.DynamicProxy;
using DevnotMentor.Api.Utilities.Interceptor.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Interceptor
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type
                .GetCustomAttributes<InterceptionBaseAttribute>(true)
                .ToList();

            var methodAttributes = type
                .GetMethod(method.Name)
                .GetCustomAttributes<InterceptionBaseAttribute>(true)
                .ToList();

            classAttributes.AddRange(methodAttributes);

            return classAttributes.ToArray();
        }
    }
}

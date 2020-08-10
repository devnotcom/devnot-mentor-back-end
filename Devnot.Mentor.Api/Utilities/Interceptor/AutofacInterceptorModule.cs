using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Utilities.Interceptor
{
    public class AutofacInterceptorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(executingAssembly)
                 .AsImplementedInterfaces()
                 .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                 {
                     Selector = new AspectInterceptorSelector()
                 })
                 .SingleInstance();
        }
    }
}

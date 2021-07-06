//using Autofac;
//using Autofac.Extras.DynamicProxy;
//using Castle.DynamicProxy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace DevnotMentor.Api.Utilities.Interceptor
//{
//    public class AutofacInterceptorModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

//            builder.RegisterAssemblyTypes(executingAssembly)
//                .Where(i => !CheckIsRepositoryClass(i.Name))
//                 .AsImplementedInterfaces()
//                 .EnableInterfaceInterceptors(new ProxyGenerationOptions()
//                 {
//                     Selector = new AspectInterceptorSelector()
//                 })
//                 .SingleInstance();
//        }

//        private bool CheckIsRepositoryClass(string name)
//        {
//            return Regex.IsMatch(name, ".*([rR]epository)$");
//        }
//    }
//}

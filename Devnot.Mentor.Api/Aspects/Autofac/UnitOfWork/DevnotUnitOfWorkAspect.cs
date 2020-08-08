using Castle.DynamicProxy;
using DevnotMentor.Api.Common;
using DevnotMentor.Api.Utilities.Interceptor.Autofac;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace DevnotMentor.Api.Aspects.Autofac.UnitOfWork
{
    public class DevnotUnitOfWorkAspect : Interception
    {
        public override void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();

                if (invocation.ReturnValue is Task task && task.Exception != null)
                {
                    if (invocation.Method.ReturnType.IsGenericType)
                    {
                        Type[] argTypes = invocation.Method.ReturnType.GetGenericArguments();

                        var args = new List<object>();

                        foreach (var argType in argTypes)
                        {
                            args.Add(Activator.CreateInstance(argType));
                        }

                        MethodInfo fromResult = typeof(Task).GetMethod("FromResult");

                        MethodInfo genericFromResult = fromResult.MakeGenericMethod(argTypes);

                        invocation.ReturnValue = genericFromResult.Invoke(task, args.ToArray());
                    }

                    throw task.Exception.InnerException;
                }
            }
            catch (Exception ex)
            {
                if (invocation.Method.ReturnType.IsAbstract)
                {
                    throw new InvalidOperationException($"Type {invocation.Method.ReturnType.FullName} may not be abstract. Abstract classes cannot be instantiated.");
                }

                throw ex;
            }
        }
    }
}

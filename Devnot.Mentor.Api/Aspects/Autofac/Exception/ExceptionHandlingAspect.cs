using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories;
using DevnotMentor.Api.Utilities.Interceptor.Autofac;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Aspects.Autofac.Exception
{
    //public class ExceptionHandlingAspect : Interception
    //{
    //    private LoggerRepository loggerRepository;

    //    public ExceptionHandlingAspect()
    //    {
    //    //    loggerRepository = new LoggerRepository(new MentorDBContext());
    //    }

    //    //    public override void OnException(IInvocation invocation, System.Exception e)
    //    //    {
    //    //        loggerRepository.WriteError(e).Wait();

    //    //        CreateDefaultReturnValue(invocation);
    //    //    }

    //    //    private void CreateDefaultReturnValue(IInvocation invocation)
    //    //    {
    //    //        if (invocation.Method.ReturnType.IsAbstract)
    //    //        {
    //    //            throw new InvalidOperationException($"Type {invocation.Method.ReturnType.FullName} may not be abstract. Abstract classes cannot be instantiated.");
    //    //        }

    //    //        if (invocation.Method.ReturnType.IsGenericType)
    //    //        {
    //    //            if (invocation.Method.ReturnType.Namespace == typeof(Task).Namespace)
    //    //            {
    //    //                CreateDefaultReturnGenericTaskValue(invocation);
    //    //            }
    //    //            else
    //    //            {
    //    //                CreateDefaultReturnGenericValue(invocation);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            invocation.ReturnValue = Activator.CreateInstance(invocation.Method.ReturnType);
    //    //        }
    //    //    }

    //    //    private void CreateDefaultReturnGenericValue(IInvocation invocation)
    //    //    {
    //    //        string returnValueFullName = invocation.Method.ReturnType.FullName;

    //    //        Type returnType = Type.GetType(returnValueFullName);

    //    //        invocation.ReturnValue = Activator.CreateInstance(returnType);
    //    //    }

    //    //    private void CreateDefaultReturnGenericTaskValue(IInvocation invocation)
    //    //    {
    //    //        Type[] argTypes = invocation.Method.ReturnType.GetGenericArguments();

    //    //        var args = new List<object>();

    //    //        foreach (var argType in argTypes)
    //    //        {
    //    //            args.Add(Activator.CreateInstance(argType));
    //    //        }

    //    //        MethodInfo fromResult = typeof(Task).GetMethod("FromResult");

    //    //        MethodInfo genericFromResult = fromResult.MakeGenericMethod(argTypes);

    //    //        var task = Task.Run(() => { });

    //    //        invocation.ReturnValue = genericFromResult.Invoke(task, args.ToArray());
    //    //    }
    //    //}
    //}
}

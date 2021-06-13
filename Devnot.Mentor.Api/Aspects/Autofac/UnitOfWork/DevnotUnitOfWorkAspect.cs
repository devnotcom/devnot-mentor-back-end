using Castle.DynamicProxy;
using DevnotMentor.Api.Utilities.Interceptor.Autofac;
using System.Threading.Tasks;
using System.Transactions;

namespace DevnotMentor.Api.Aspects.Autofac.UnitOfWork
{
    public class DevnotUnitOfWorkAspect : Interception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();

                    if (invocation.ReturnValue is Task returnValueTask)
                    {
                        returnValueTask.GetAwaiter().GetResult();
                    }

                    if (invocation.ReturnValue is Task task && task.Exception != null)
                    {
                        throw task.Exception;
                    }

                    transaction.Complete();
                }
                catch (System.Exception ex)
                {
                    transaction.Dispose();

                    throw ex;
                }
            }
        }
    }
}

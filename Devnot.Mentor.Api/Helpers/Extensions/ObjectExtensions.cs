//using DevnotMentor.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtensions
    {
        //MentorDBContext _context = context.HttpContext.RequestServices.GetService(typeof(MentorDBContext)) as MentorDBContext;

        //public static async Task RunInTry<T>(this object o, ResponseModel<T> response, Action action)
        //{
        //    try
        //    {
        //        action();
        //    }
        //    catch (Exception ex)
        //    {
        //        //log.WriteError("Business hatası", ex);
        //        response.Errors.Add("Bilinmeyen bir hata oluştu.");
        //    }
        //}

        public static async Task RunInTryAsync(this object o, Func<Task> action, Func<Exception, Task> exception)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                //log.WriteError("Business hatası", ex);
                await exception(ex);
            }
        }
    }
}

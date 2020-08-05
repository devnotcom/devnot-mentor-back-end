using DevnotMentor.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly MentorDBContext context;

        public Repository(MentorDBContext context)
        {
            this.context = context;
        }

        public T Create(T entity)
        {
            var newEntry = context.Set<T>().Add(entity);
            context.SaveChanges();

            return newEntry.Entity;
        }

        public T Update(T entity)
        {
            var newEntry = context.Set<T>().Update(entity);
            context.SaveChanges();

            return newEntry.Entity;
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> Filter()
        {
            return context.Set<T>();
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            return context.Set<T>().Where(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}

using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DevnotMentor.Api.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        protected MentorDBContext DbContext;

        public BaseRepository(MentorDBContext dbContext)
        {
            DbContext = dbContext;
        }

        public TEntity Create(TEntity entity)
        {
            var newEntry = DbContext.Set<TEntity>().Add(entity);
            DbContext.SaveChanges();

            return newEntry.Entity;
        }

        public void Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            DbContext.SaveChanges();
        }

        public List<TEntity> GetList()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public TEntity Update(TEntity entity)
        {
            var newEntry = DbContext.Set<TEntity>().Update(entity);
            DbContext.SaveChanges();

            return newEntry.Entity;
        }
    }
}

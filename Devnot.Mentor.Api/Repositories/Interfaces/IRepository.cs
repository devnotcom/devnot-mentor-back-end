using System;
using System.Collections.Generic;
using System.Linq;

namespace DevnotMentor.Api.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        List<TEntity> GetList();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public abstract class GenericRepository<T> where T : class
    {
        protected DbContext _entities;
        protected readonly DbSet<T> _dbset;

        public GenericRepository(UnitOfWork UoW)
        {
            _entities = UoW.DbContext;
            _dbset = UoW.DbContext.Set<T>();
        }

        public virtual T GetFirst(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate).FirstOrDefault();
        }
        public virtual T GetFirst()
        {
            return _dbset.FirstOrDefault();
        }
        public virtual bool IsExists(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return GetFirst(predicate) == null ? false : true;
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate).AsEnumerable();
        }

        public virtual EntityEntry<T> Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual EntityEntry<T> Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }
        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            return _dbset.Where(whereCondition).Count();
        }
        public long Count()
        {
            return _dbset.Count();
        }

        public virtual T GetById(object id)
        {
            return _entities.Set<T>().Find(id);
        }

        public T GetLast()
        {

            return _dbset.LastOrDefault();
        }
    }
}

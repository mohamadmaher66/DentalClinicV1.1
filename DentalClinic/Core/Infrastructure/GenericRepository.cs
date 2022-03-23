using AutoMapper;
using DBModels;
using Microsoft.EntityFrameworkCore;
using Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure
{
    public abstract class GenericRepository<E,D> where E : AuditEntity 
    {
        protected DbContext _entities;
        protected readonly DbSet<E> _dbset;
        protected IMapper _mapper;

        public GenericRepository(IUnitOfWork UoW, IMapper mapper)
        {
            _entities = UoW.DbContext;
            _dbset = UoW.DbContext.Set<E>();
            _mapper = mapper;
        }

        public virtual IEnumerable<D> GetAll(GridSettings gridSettings, Expression<Func<E, bool>> predicate)
        {
            IQueryable<E> appointmentAdditionList = _dbset.Where(predicate).AsNoTracking();

            gridSettings.RowsCount = appointmentAdditionList.Count();

            return _mapper.Map<List<D>>(appointmentAdditionList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize).AsNoTracking());
        }

        public virtual D GetById(int id)
        {
            return _mapper.Map<D>(_entities.Set<E>().Find(id));
        }

        public virtual void Add(D dto, int userId)
        {
            E entity = _mapper.Map<E>(dto);
            entity.CreationDate = DateTime.Now;
            entity.CreatedBy = userId;

            _dbset.Add(entity);
        }

        public virtual void Update(D dto, int userId)
        {
            E entity = _mapper.Map<E>(dto);
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedBy = userId;

            //_entities.Entry(entity).State = EntityState.Modified;
            _entities.Entry(entity).Property(m => m.CreatedBy).IsModified = false;
            _entities.Entry(entity).Property(m => m.CreationDate).IsModified = false;

            _dbset.Update(entity);
        }

        public void Delete(D dto)
        {
            _dbset.Remove(_mapper.Map<E>(dto));
        }
    }
}

using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace AppointmentCategoryModule
{
    public class AppointmentCategoryRepository
    {
        private DbContext entities;
        private DbSet<AppointmentCategory> dbset;
        private readonly IMapper _mapper;

        public AppointmentCategoryRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<AppointmentCategory>();
            _mapper = mapper;
        }

        public IEnumerable<AppointmentCategoryDTO> GetAll(GridSettings gridSettings)
        {
            double.TryParse(gridSettings.SearchText, out double price);
            IEnumerable<AppointmentCategory> appointmentCategoryList = dbset.Where(x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                                                                    (x.Name.Contains(gridSettings.SearchText)
                                                                                    || x.Price == price
                                                                                    ));

            gridSettings.RowsCount = appointmentCategoryList.Count();
            return _mapper.Map<List<AppointmentCategoryDTO>>(appointmentCategoryList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }
        public IEnumerable<AppointmentCategoryDTO> GetAllLite()
        {
            return _mapper.Map<List<AppointmentCategoryDTO>>(dbset.OrderBy(m => m.Name));
        }

        public AppointmentCategoryDTO GetById(int appointmentCategoryId)
        {
            return _mapper.Map<AppointmentCategoryDTO>(entities.Set<AppointmentCategory>().AsNoTracking().FirstOrDefault(c => c.Id == appointmentCategoryId));
        }

        public void Add(AppointmentCategoryDTO appointmentCategory, int appointmentCategoryId)
        {
            AppointmentCategory model = _mapper.Map<AppointmentCategory>(appointmentCategory);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = appointmentCategoryId;
            dbset.Add(model);
        }

        public void Update(AppointmentCategoryDTO appointmentCategory, int appointmentCategoryId)
        {
            AppointmentCategory model = _mapper.Map<AppointmentCategory>(appointmentCategory);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = appointmentCategoryId;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }
        public void Delete(AppointmentCategoryDTO appointmentCategory)
        {
            dbset.Remove(_mapper.Map<AppointmentCategory>(appointmentCategory));
        }
    }
}

using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace AppointmentAdditionModule
{
    public class AppointmentAdditionRepository
    {
        private DbContext entities;
        private DbSet<AppointmentAddition> dbset;
        private readonly IMapper _mapper;

        public AppointmentAdditionRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<AppointmentAddition>();
            _mapper = mapper;
        }

        public IEnumerable<AppointmentAdditionDTO> GetAll(GridSettings gridSettings)
        {
            double.TryParse(gridSettings.SearchText, out double price);
            IEnumerable<AppointmentAddition> appointmentAdditionList = dbset.Where(x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                                                                    (x.Name.Contains(gridSettings.SearchText)
                                                                                    || x.Price == price
                                                                                    ));

            gridSettings.RowsCount = appointmentAdditionList.Count();
            return _mapper.Map<List<AppointmentAdditionDTO>>(appointmentAdditionList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }
        public IEnumerable<AppointmentAdditionDTO> GetAllLite()
        {
            return _mapper.Map<List<AppointmentAdditionDTO>>(dbset.OrderBy(m => m.Name));
        }

        public AppointmentAdditionDTO GetById(int appointmentAdditionId)
        {
            return _mapper.Map<AppointmentAdditionDTO>(entities.Set<AppointmentAddition>().AsNoTracking().FirstOrDefault(c => c.Id == appointmentAdditionId));
        }

        public void Add(AppointmentAdditionDTO appointmentAddition, int appointmentAdditionId)
        {
            AppointmentAddition model = _mapper.Map<AppointmentAddition>(appointmentAddition);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = appointmentAdditionId;
            dbset.Add(model);
        }

        public void Update(AppointmentAdditionDTO appointmentAddition, int appointmentAdditionId)
        {
            AppointmentAddition model = _mapper.Map<AppointmentAddition>(appointmentAddition);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = appointmentAdditionId;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }
        public void Delete(AppointmentAdditionDTO appointmentAddition)
        {
            dbset.Remove(_mapper.Map<AppointmentAddition>(appointmentAddition));
        }
    }
}

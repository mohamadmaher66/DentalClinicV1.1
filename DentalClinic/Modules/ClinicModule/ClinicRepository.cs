using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace ClinicModule
{
    public class ClinicRepository
    {
        private DbContext entities;
        private DbSet<Clinic> dbset;
        private readonly IMapper _mapper;

        public ClinicRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<Clinic>();
            _mapper = mapper;
        }

        public IEnumerable<ClinicDTO> GetAll(GridSettings gridSettings)
        {
            IEnumerable<Clinic> clinicList = dbset.Where(x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                    (x.Name.Contains(gridSettings.SearchText)
                                    || x.Address.Contains(gridSettings.SearchText)
                                    ));

            gridSettings.RowsCount = clinicList.Count();
            return _mapper.Map<List<ClinicDTO>>(clinicList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }
        public IEnumerable<ClinicDTO> GetAllLite()
        {
            return _mapper.Map<List<ClinicDTO>>(dbset.OrderBy(m => m.Name));
        }

        public ClinicDTO GetById(int clinicId)
        {
            return _mapper.Map<ClinicDTO>(entities.Set<Clinic>().AsNoTracking().FirstOrDefault(c => c.Id == clinicId));
        }

        public void Add(ClinicDTO clinic, int clinicId)
        {
            Clinic model = _mapper.Map<Clinic>(clinic);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = clinicId;
            dbset.Add(model);
        }

        public void Update(ClinicDTO clinic, int clinicId)
        {
            Clinic model = _mapper.Map<Clinic>(clinic);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = clinicId;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }
        public void Delete(ClinicDTO clinic)
        {
            dbset.Remove(_mapper.Map<Clinic>(clinic));
        }
    }
}

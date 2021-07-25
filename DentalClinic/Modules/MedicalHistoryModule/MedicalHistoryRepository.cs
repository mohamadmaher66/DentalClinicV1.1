using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace MedicalHistoryModule
{
    public class MedicalHistoryRepository
    {
        private DbContext entities;
        private DbSet<MedicalHistory> dbset;
        private readonly IMapper _mapper;

        public MedicalHistoryRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<MedicalHistory>();
            _mapper = mapper;
        }

        public IEnumerable<MedicalHistoryDTO> GetAll(GridSettings gridSettings)
        {
            IEnumerable<MedicalHistory> medicalHistoryList = dbset.Where(x => string.IsNullOrEmpty(gridSettings.SearchText) ? true : x.Name.Contains(gridSettings.SearchText));
            gridSettings.RowsCount = medicalHistoryList.Count();
            return _mapper.Map<List<MedicalHistoryDTO>>(medicalHistoryList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }
        public IEnumerable<MedicalHistoryDTO> GetAllLite()
        {
            return _mapper.Map<List<MedicalHistoryDTO>>(dbset.OrderBy(m => m.Name));
        }

        public MedicalHistoryDTO GetById(int medicalHistoryId)
        {
            return _mapper.Map<MedicalHistoryDTO>(entities.Set<MedicalHistory>().AsNoTracking().FirstOrDefault(c => c.Id == medicalHistoryId));
        }

        public void Add(MedicalHistoryDTO medicalHistory, int userId)
        {
            MedicalHistory model = _mapper.Map<MedicalHistory>(medicalHistory);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = userId;
            dbset.Add(model);
        }

        public void Update(MedicalHistoryDTO medicalHistory, int userId)
        {
            MedicalHistory model = _mapper.Map<MedicalHistory>(medicalHistory);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = userId;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }
        public void Delete(MedicalHistoryDTO medicalHistory)
        {
            dbset.Remove(_mapper.Map<MedicalHistory>(medicalHistory));
        }
    }
}

using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;

namespace PatientModule
{
    public class PatientRepository
    {
        private DbContext entities;
        private DbSet<Patient> dbset;
        private DbSet<PatientMedicalHistory> patientMedicalHistoryDBSet;
        private readonly IMapper _mapper;

        public PatientRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<Patient>();
            patientMedicalHistoryDBSet = UoW.DbContext.Set<PatientMedicalHistory>();
            _mapper = mapper;
        }

        public IEnumerable<PatientDTO> GetAll(GridSettings gridSettings)
        {
            IEnumerable<Patient> patientList = dbset.Where(x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                    (x.FullName.Contains(gridSettings.SearchText)
                                    || x.Address.Contains(gridSettings.SearchText)
                                    || x.Phone.Contains(gridSettings.SearchText)
                                    ));

            gridSettings.RowsCount = patientList.Count();
            return _mapper.Map<List<PatientDTO>>(patientList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }

        public IEnumerable<PatientDTO> GetAllLite()
        {
            return _mapper.Map<List<PatientDTO>>(dbset
                .Include(PMH => PMH.PatientMedicalHistoryList)
                .ThenInclude(MH => MH.MedicalHistory)
                .OrderBy(m => m.FullName));
        }

        public PatientDTO GetById(int patientId)
        {
            return _mapper.Map<PatientDTO>(entities.Set<Patient>()
                .Include(PMH => PMH.PatientMedicalHistoryList)
                .ThenInclude(MH => MH.MedicalHistory)
                .AsNoTracking().FirstOrDefault(c => c.Id == patientId));
        }

        public int Add(PatientDTO patient, int userId)
        {
            Patient model = _mapper.Map<Patient>(patient);
            model.CreationDate = DateTime.Now;
            model.CreatedBy = userId;

            dbset.Add(model);
            entities.SaveChanges();
            return model.Id;
        }

        public void Update(PatientDTO patient, int userId)
        {
            Patient model = _mapper.Map<Patient>(patient);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = userId;

            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }

        public void Delete(PatientDTO patient)
        {
            dbset.Remove(_mapper.Map<Patient>(patient));
        }

        internal void AddPatientMedicalHistoryList(List<MedicalHistoryDTO> medicalHistoryList, int patientId)
        {
            foreach(var medicalHistory in medicalHistoryList)
            {
                patientMedicalHistoryDBSet.Add(new PatientMedicalHistory()
                {
                    MedicalHistoryId = medicalHistory.Id,
                    PatientId = patientId
                });
            }
        }

        internal void RemovePatientMedicalHistoryList(int patientId)
        {
            IEnumerable<PatientMedicalHistory> patientMedicalHistories = patientMedicalHistoryDBSet.Where(p => p.PatientId == patientId);
            if (patientMedicalHistories != null && patientMedicalHistories.Count() > 0)
            {
                patientMedicalHistoryDBSet.RemoveRange(patientMedicalHistories);
            }
        }
    }
}

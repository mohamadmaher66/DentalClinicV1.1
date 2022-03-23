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
    public class PatientRepository : GenericRepository<Patient, PatientDTO>
    {
        public PatientRepository(UnitOfWork UoW, IMapper mapper) : base(UoW, mapper) { }


        public IEnumerable<PatientDTO> GetAllLite()
        {
            return _mapper.Map<List<PatientDTO>>(_dbset
                .Include(PMH => PMH.PatientMedicalHistoryList)
                .ThenInclude(MH => MH.MedicalHistory)
                .AsNoTracking()
                .OrderBy(m => m.FullName));
        }

        public override PatientDTO GetById(int patientId)
        {
            return _mapper.Map<PatientDTO>(_entities.Set<Patient>()
                .Include(PMH => PMH.PatientMedicalHistoryList)
                .ThenInclude(MH => MH.MedicalHistory)
                .AsNoTracking().FirstOrDefault(c => c.Id == patientId));
        }

        public int Add(PatientDTO patient, int userId)
        {
            base.Add(patient, userId);
            _entities.SaveChanges();
            return patient.Id;
        }

        public override void Update(PatientDTO dto, int userId)
        {
            //Patient patient = _mapper.Map<Patient>(dto);

            Patient patientToUpdate = _dbset.Include(p => p.PatientMedicalHistoryList)
                                            .FirstOrDefault(p => p.Id == dto.Id);

            //patientToUpdate = _mapper.Map<Patient>(dto);

            patientToUpdate.Id = dto.Id;
            patientToUpdate.FullName = dto.FullName;
            patientToUpdate.Age = dto.Age;
            patientToUpdate.Address = dto.Address;
            patientToUpdate.Gender = dto.Gender;
            patientToUpdate.Phone = dto.Phone;
            patientToUpdate.ModifiedBy = userId;
            patientToUpdate.ModifiedDate = DateTime.Now;

            _entities.Entry(patientToUpdate).Property(m => m.CreatedBy).IsModified = false;
            _entities.Entry(patientToUpdate).Property(m => m.CreationDate).IsModified = false;

            patientToUpdate.PatientMedicalHistoryList.Clear();

            if (dto.MedicalHistoryList.Count > 0)
            {
                foreach (var item in dto.MedicalHistoryList)
                {
                    patientToUpdate.PatientMedicalHistoryList.Add(new PatientMedicalHistory()
                    {
                        PatientId = patientToUpdate.Id,
                        MedicalHistoryId = item.Id
                    });
                }
            }

            _dbset.Update(patientToUpdate);
        }
        /*public override void Update(PatientDTO dto, int userId)
        {
            Patient patient = _mapper.Map<Patient>(dto);
            //Patient orignialPatient = _dbset.Include(MH => MH.PatientMedicalHistoryList).Single(p => p.Id == dto.Id);
            //orignialPatient = patient;

            patient.ModifiedDate = DateTime.Now;
            patient.ModifiedBy = userId;

            _entities.Entry(patient).Property(m => m.CreatedBy).IsModified = false;
            _entities.Entry(patient).Property(m => m.CreationDate).IsModified = false;

            //patient.PatientMedicalHistoryList.Clear(); 

            foreach (var item in dto.MedicalHistoryList)
            {
                patient.PatientMedicalHistoryList.Add(_mapper.Map<PatientMedicalHistory>(item));
            }

            _dbset.Update(patient);
        }*/

        //public void Update(PatientDTO patient, int userId)
        //{
        //    Patient model = _mapper.Map<Patient>(patient);
        //    List<PatientMedicalHistoryList> patientMedicalHistoryList = patient.MedicalHistoryList.

        //    model.PatientMedicalHistoryList.a
        //    model.ModifiedDate = DateTime.Now;
        //    model.ModifiedBy = userId;

        //    entities.Entry(model).State = EntityState.Modified;
        //    entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
        //    entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        //}

        //public void Delete(PatientDTO patient)
        //{
        //    dbset.Remove(_mapper.Map<Patient>(patient));
        //}

        //internal void AddPatientMedicalHistoryList(List<MedicalHistoryDTO> medicalHistoryList, int patientId)
        //{
        //    foreach(var medicalHistory in medicalHistoryList)
        //    {
        //        patientMedicalHistoryDBSet.Add(new PatientMedicalHistory()
        //        {
        //            MedicalHistoryId = medicalHistory.Id,
        //            PatientId = patientId
        //        });
        //    }
        //}

        //internal void RemovePatientMedicalHistoryList(int patientId)
        //{
        //    IEnumerable<PatientMedicalHistory> patientMedicalHistories = patientMedicalHistoryDBSet.Where(p => p.PatientId == patientId);
        //    if (patientMedicalHistories != null && patientMedicalHistories.Count() > 0)
        //    {
        //        patientMedicalHistoryDBSet.RemoveRange(patientMedicalHistories);
        //    }
        //}
    }
}

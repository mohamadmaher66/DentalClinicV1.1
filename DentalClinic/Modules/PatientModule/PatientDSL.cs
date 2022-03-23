using Infrastructure;
using System;
using System.Linq;
using AppDBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;
using Enums;
using MedicalHistoryModule;

namespace PatientModule
{
    public class PatientDSL
    {
        PatientRepository patientRepository;
        UnitOfWork UoW;
        IMapper mapper;

        public PatientDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            patientRepository = new PatientRepository(UoW, _mapper);
            mapper = _mapper;
        }

        public List<PatientDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                return patientRepository.GetAll(gridSettings, x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                    (x.FullName.Contains(gridSettings.SearchText)
                                    || x.Address.Contains(gridSettings.SearchText)
                                    || x.Phone.Contains(gridSettings.SearchText)
                                    )).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PatientDTO GetById(int patientId)
        {
            try
            {
                return patientRepository.GetById(patientId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PatientDTO> GetAllLite()
        {
            try
            {
                return patientRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(PatientDTO patient, int userId)
        {
            try
            {
                patient.Id = patientRepository.Add(patient, userId);
                //AddPatientMedicalHistoryList(patient.MedicalHistoryList, patient.Id);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(PatientDTO patient, int userId)
        {
            try
            {
                patientRepository.Update(patient, userId);
                //RemovePatientMedicalHistoryList(patient.Id);
                //AddPatientMedicalHistoryList(patient.MedicalHistoryList, patient.Id);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<PatientDTO> Delete(PatientDTO patient, GridSettings gridSettings)
        {
            try
            {
                //RemovePatientMedicalHistoryList(patient.Id);
                patientRepository.Delete(patient);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetAll(gridSettings);
        }

        public List<DetailsList> GetDetailsLists()
        {
            try
            {
                List<MedicalHistoryDTO> medicalHistoryList = new MedicalHistoryDSL(mapper).GetAllLite();
                DetailsList list = new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.MedicalHistory,
                    List = medicalHistoryList
                };
                List<DetailsList> detailsList = new List<DetailsList>();
                detailsList.Add(list);

                return detailsList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void AddPatientMedicalHistoryList(List<MedicalHistoryDTO> medicalHistoryList, int patientId)
        {
            try
            {
                //patientRepository.AddPatientMedicalHistoryList(medicalHistoryList, patientId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void RemovePatientMedicalHistoryList(int patientId)
        {
            try
            {
                //patientRepository.RemovePatientMedicalHistoryList(patientId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

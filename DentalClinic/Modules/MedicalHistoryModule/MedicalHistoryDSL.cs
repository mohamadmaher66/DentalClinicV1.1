using Infrastructure;
using System;
using System.Linq;
using AppDBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;

namespace MedicalHistoryModule
{
    public class MedicalHistoryDSL
    {
        MedicalHistoryRepository medicalHistoryRepository;
        UnitOfWork UoW;

        public MedicalHistoryDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            medicalHistoryRepository = new MedicalHistoryRepository(UoW, _mapper);
        }

        public List<MedicalHistoryDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                return medicalHistoryRepository.GetAll(gridSettings, x => string.IsNullOrEmpty(gridSettings.SearchText) ? true : x.Name.Contains(gridSettings.SearchText)).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<MedicalHistoryDTO> GetAllLite()
        {
            try
            {
                return medicalHistoryRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public MedicalHistoryDTO GetById(int medicalHistoryId)
        {
            try
            {
                return medicalHistoryRepository.GetById(medicalHistoryId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(MedicalHistoryDTO medicalHistory, int userId)
        {
            try
            {
                medicalHistoryRepository.Add(medicalHistory, userId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(MedicalHistoryDTO medicalHistory, int userId)
        {
            try
            {
                medicalHistoryRepository.Update(medicalHistory, userId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<MedicalHistoryDTO> Delete(MedicalHistoryDTO medicalHistory, GridSettings gridSettings)
        {
            try
            {
                medicalHistoryRepository.Delete(medicalHistory);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetAll(gridSettings);
        }
    }
}

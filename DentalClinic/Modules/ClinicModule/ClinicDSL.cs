using Infrastructure;
using System;
using System.Linq;
using AppDBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;

namespace ClinicModule
{
    public class ClinicDSL
    {
        ClinicRepository clinicRepository;
        UnitOfWork UoW;

        public ClinicDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            clinicRepository = new ClinicRepository(UoW, _mapper);
        }

        public List<ClinicDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                return clinicRepository.GetAll(gridSettings, x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                     (x.Name.Contains(gridSettings.SearchText)
                                     || x.Address.Contains(gridSettings.SearchText)
                                     )).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<ClinicDTO> GetAllLite()
        {
            try
            {
                return clinicRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ClinicDTO GetById(int clinicId)
        {
            try
            {
                return clinicRepository.GetById(clinicId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(ClinicDTO clinic, int clinicId)
        {
            try
            {
                clinicRepository.Add(clinic, clinicId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(ClinicDTO clinic, int clinicId)
        {
            try
            {
                clinicRepository.Update(clinic, clinicId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ClinicDTO> Delete(ClinicDTO clinic, GridSettings gridSettings)
        {
            try
            {
                clinicRepository.Delete(clinic);
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

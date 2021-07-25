using Infrastructure;
using System;
using System.Linq;
using DBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;

namespace AppointmentCategoryModule
{
    public class AppointmentCategoryDSL
    {
        AppointmentCategoryRepository appointmentCategoryRepository;
        UnitOfWork UoW;

        public AppointmentCategoryDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            appointmentCategoryRepository = new AppointmentCategoryRepository(UoW, _mapper);
        }

        public List<AppointmentCategoryDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                return appointmentCategoryRepository.GetAll(gridSettings).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentCategoryDTO> GetAllLite()
        {
            try
            {
                return appointmentCategoryRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AppointmentCategoryDTO GetById(int appointmentCategoryId)
        {
            try
            {
                return appointmentCategoryRepository.GetById(appointmentCategoryId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(AppointmentCategoryDTO appointmentCategory, int appointmentCategoryId)
        {
            try
            {
                appointmentCategoryRepository.Add(appointmentCategory, appointmentCategoryId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(AppointmentCategoryDTO appointmentCategory, int appointmentCategoryId)
        {
            try
            {
                appointmentCategoryRepository.Update(appointmentCategory, appointmentCategoryId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AppointmentCategoryDTO> Delete(AppointmentCategoryDTO appointmentCategory, GridSettings gridSettings)
        {
            try
            {
                appointmentCategoryRepository.Delete(appointmentCategory);
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

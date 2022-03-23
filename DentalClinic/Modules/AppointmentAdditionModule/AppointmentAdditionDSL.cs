using Infrastructure;
using System;
using System.Linq;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;
using DBModels;

namespace AppointmentAdditionModule
{
    public class AppointmentAdditionDSL
    {
        AppointmentAdditionRepository appointmentAdditionRepository;
        IUnitOfWork UoW;

        public AppointmentAdditionDSL(IUnitOfWork uow, IMapper _mapper)
        {
            UoW = uow;
            appointmentAdditionRepository = new AppointmentAdditionRepository(UoW, _mapper);
        }

        public List<AppointmentAdditionDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                double.TryParse(gridSettings.SearchText, out double price);
                return appointmentAdditionRepository.GetAll(gridSettings, x => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                                                                    (x.Name.Contains(gridSettings.SearchText)
                                                                                    || x.Price == price
                                                                                    )).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentAdditionDTO> GetAllLite()
        {
            try
            {
                return appointmentAdditionRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AppointmentAdditionDTO GetById(int appointmentAdditionId)
        {
            try
            {
                return appointmentAdditionRepository.GetById(appointmentAdditionId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(AppointmentAdditionDTO appointmentAddition, int appointmentAdditionId)
        {
            try
            {
                appointmentAdditionRepository.Add(appointmentAddition, appointmentAdditionId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(AppointmentAdditionDTO appointmentAddition, int appointmentAdditionId)
        {
            try
            {
                appointmentAdditionRepository.Update(appointmentAddition, appointmentAdditionId);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AppointmentAdditionDTO> Delete(AppointmentAdditionDTO appointmentAddition, GridSettings gridSettings)
        {
            try
            {
                appointmentAdditionRepository.Delete(appointmentAddition);
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

using Infrastructure;
using System;
using System.Linq;
using AppDBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;
using Enums;
using ClinicModule;
using PatientModule;
using UserModule;
using AppointmentCategoryModule;
using AppointmentAdditionModule;

namespace AppointmentModule
{
    public class AppointmentDSL
    {
        AppointmentRepository appointmentRepository;
        UnitOfWork UoW;
        IMapper mapper;

        public AppointmentDSL(IMapper _mapper)
        {
            UoW = new UnitOfWork(new DentalClinicDBContext());
            appointmentRepository = new AppointmentRepository(UoW, _mapper);
            mapper = _mapper;
        }

        public List<AppointmentDTO> GetAll(GridSettings gridSettings)
        {
            try
            {
                return appointmentRepository.GetAll(gridSettings).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AppointmentDTO> GetAllDashboard(AppointmentDTO filterEntity)
        {
            try
            {
                return appointmentRepository.GetAllDashboard(filterEntity).ToList();
            }
            catch (Exception e) {throw e;}
        }
        

        public AppointmentDTO GetById(int appointmentId)
        {
            try
            {
                return appointmentRepository.GetById(appointmentId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<AppointmentDTO> GetAllLite()
        {
            try
            {
                return appointmentRepository.GetAllLite().ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Add(AppointmentDTO appointment, int userId)
        {
            try
            {
                appointment.State = AppointmentStateEnum.Pending;
                appointment.Id = appointmentRepository.Add(appointment, userId);
                AddAttachmentList(appointment.AttachmentList, appointment.Id);
                AddToothList(appointment.ToothList, appointment.Id);
                AddAppointmentAdditionList(appointment.AppointmentAdditionList, appointment.Id);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        public void Update(AppointmentDTO appointment, int userId)
        {
            try
            {
                appointmentRepository.Update(appointment, userId);

                DeleteAttachmentList(appointment.Id);
                AddAttachmentList(appointment.AttachmentList, appointment.Id);

                DeleteToothList(appointment.Id);
                AddToothList(appointment.ToothList, appointment.Id);

                DeleteAppointmentAdditionList(appointment.Id);
                AddAppointmentAdditionList(appointment.AppointmentAdditionList, appointment.Id);

                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<AppointmentReportDTO> GetAppointmentReport(DateTime dateFrom, DateTime dateTo, int patientId, int categoryId, int clinicId, int userId, AppointmentStateEnum state)
        {
            try
            {
                return appointmentRepository.GetAppointmentReport(dateFrom, dateTo, patientId, categoryId, clinicId, userId, state).ToList();
            }
            catch (Exception e) { throw e; }
        }

        public void SaveState(AppointmentDTO appointment, int userId)
        {
            try
            {
                appointmentRepository.SaveState(appointment, userId);
                UoW.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        public List<AppointmentDTO> Delete(AppointmentDTO appointment, GridSettings gridSettings)
        {
            try
            {
                DeleteAttachmentList(appointment.Id);
                DeleteToothList(appointment.Id);
                DeleteAppointmentAdditionList(appointment.Id);

                appointmentRepository.Delete(appointment);
                UoW.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return GetAll(gridSettings);
        }

        

        private void AddAttachmentList(List<AttachmentDTO> attachmentList, int appointmentId)
        {
            appointmentRepository.AddAttachmentList(attachmentList, appointmentId);
        }
        private void AddToothList(List<AppointmentToothDTO> toothList, int appointmentId)
        {
            appointmentRepository.AddToothList(toothList, appointmentId);
        }
        private void AddAppointmentAdditionList(List<AppointmentAdditionDTO> appointmentAdditionList, int appointmentId)
        {
            appointmentRepository.AddAppointmentAdditionList(appointmentAdditionList, appointmentId);
        }

        private void DeleteAttachmentList(int appointmentId)
        {
            appointmentRepository.DeleteAttachmentList(appointmentId);
        }

        private void DeleteToothList(int appointmentId)
        {
            appointmentRepository.DeleteToothList(appointmentId);
        }

        private void DeleteAppointmentAdditionList(int appointmentId)
        {
            appointmentRepository.DeleteAppointmentAdditionList(appointmentId);
        }


        public List<DetailsList> GetDetailsLists()
        {
            try
            {
                List<DetailsList> detailsList = new List<DetailsList>();

                List<ClinicDTO> clinicList = new ClinicDSL(mapper).GetAllLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.Clinic,
                    List = clinicList
                });

                List<PatientDTO> patientList = new PatientDSL(mapper).GetAllLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.Patient,
                    List = patientList
                });

                List<UserDTO> userList = new UserDSL(mapper).GetAllDoctorsLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.User,
                    List = userList
                });

                List<AppointmentCategoryDTO> catList = new AppointmentCategoryDSL(mapper).GetAllLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.AppointmentCategory,
                    List = catList
                });

                List<AppointmentAdditionDTO> additionList = new AppointmentAdditionDSL(UoW,mapper).GetAllLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.AppointmentAddition,
                    List = additionList
                });

                return detailsList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DetailsList> GetDashboardDetailsLists()
        {
            try
            {
                List<DetailsList> detailsList = new List<DetailsList>();

                List<ClinicDTO> clinicList = new ClinicDSL(mapper).GetAllLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.Clinic,
                    List = clinicList
                });

                List<UserDTO> userList = new UserDSL(mapper).GetAllDoctorsLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.User,
                    List = userList
                });
                return detailsList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

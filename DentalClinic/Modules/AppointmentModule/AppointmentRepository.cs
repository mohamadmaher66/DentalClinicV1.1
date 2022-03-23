using AutoMapper;
using DTOs;
using Infrastructure;
using DBModels;
using System.Collections.Generic;
using System.Linq;
using System;
using Request;
using System.Globalization;
using Enums;
using Microsoft.EntityFrameworkCore;

namespace AppointmentModule
{
    public class AppointmentRepository
    {
        private DbContext entities;
        private DbSet<Appointment> dbset;
        private readonly IMapper _mapper;

        public AppointmentRepository(UnitOfWork UoW, IMapper mapper)
        {
            entities = UoW.DbContext;
            dbset = UoW.DbContext.Set<Appointment>();
            _mapper = mapper;
        }

        public IEnumerable<AppointmentDTO> GetAll(GridSettings gridSettings)
        {
            DateTime.TryParseExact(gridSettings.SearchText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

            IEnumerable<Appointment> appointmentList = dbset.AsSplitQuery()
                                                             .Include(a => a.Category)
                                                             .Include(a => a.Patient)
                                                             .Include(a => a.Clinic)
                                                             .Include(a => a.User)
                                                             .Where(a => string.IsNullOrEmpty(gridSettings.SearchText) ? true :
                                                                    (a.Category.Name.Contains(gridSettings.SearchText)
                                                                    || a.Clinic.Name.Contains(gridSettings.SearchText)
                                                                    || a.Patient.FullName.Contains(gridSettings.SearchText)
                                                                    || a.User.FullName.Contains(gridSettings.SearchText)
                                                                    || (date > DateTime.MinValue && a.Date.Date == date.Date)))
                                                             .AsNoTracking();

            gridSettings.RowsCount = appointmentList.Count();
            return _mapper.Map<List<AppointmentDTO>>(appointmentList.OrderByDescending(m => m.CreationDate)
                                     .Skip(gridSettings.PageSize * gridSettings.PageIndex)
                                     .Take(gridSettings.PageSize));
        }

        public IEnumerable<AppointmentDTO> GetAllDashboard(AppointmentDTO filterEntity)
        {
            IEnumerable<Appointment> appointmentList = dbset.AsSplitQuery()
                                                             .Include(a => a.Category)
                                                             .Include(a => a.Patient)
                                                             .Include(a => a.Clinic)
                                                             .Include(a => a.User)
                                                             .Include(a => a.AppointmentToothList)
                                                             .Where(a => (a.Clinic.Id == filterEntity.Clinic.Id || filterEntity.Clinic.Id == 0)
                                                                      && (a.User.Id == filterEntity.User.Id || filterEntity.User.Id == 0)
                                                                      && a.Date.Date == DateTime.Now.Date
                                                                      && (a.State == AppointmentStateEnum.Current
                                                                         || a.State == AppointmentStateEnum.Pending))
                                                             .AsNoTracking();

            return _mapper.Map<List<AppointmentDTO>>(appointmentList.OrderByDescending(a => a.State).ThenBy(a => a.CreationDate));
        }

        internal IEnumerable<AppointmentReportDTO> GetAppointmentReport(DateTime dateFrom, DateTime dateTo, int patientId, int categoryId, int clinicId, int userId, AppointmentStateEnum state)
        {
            DbSet<Clinic> clinics = entities.Set<Clinic>();
            DbSet<User> users = entities.Set<User>();
            DbSet<AppointmentCategory> categories = entities.Set<AppointmentCategory>();
            DbSet<Patient> patients = entities.Set<Patient>();

            return from appointment in dbset
                   join clinic in clinics on appointment.ClinicId equals clinic.Id
                   join user in users on appointment.UserId equals user.Id
                   join category in categories on appointment.CategoryId equals category.Id
                   join patient in patients on appointment.PatientId equals patient.Id
                   where (appointment.Date >= dateFrom && appointment.Date <= dateTo)
                        && (clinicId == 0 || appointment.ClinicId == clinicId)
                        && (userId == 0 || appointment.UserId == userId)
                        && (patientId == 0 || appointment.PatientId == patientId)
                        && (categoryId == 0 || appointment.CategoryId == categoryId)
                        && (state == AppointmentStateEnum.None || appointment.State == state)
                   orderby appointment.Date
                   select new AppointmentReportDTO()
                   {
                       PatientFullName = patient.FullName,
                       ClinicName = clinic.Name,
                       UserFullName = user.FullName,
                       CategoryName = category.Name,
                       Date = appointment.Date,
                       DiscountPercentage = appointment.DiscountPercentage,
                       PaidAmount = appointment.PaidAmount,
                       TotalPrice = appointment.TotalPrice,
                       StateName = GetStateName(appointment.State)
                   };
        }

        private static string GetStateName(AppointmentStateEnum state)
        {
            switch (state)
            {
                case AppointmentStateEnum.Cancelled: return "ملغي";
                case AppointmentStateEnum.Current: return "جارى";
                case AppointmentStateEnum.Finished: return "انتهي";
                case AppointmentStateEnum.Pending: return "قيد الانتظار";
            }
            return null;
        }

        public void SaveState(AppointmentDTO appointment, int userId)
        {
            Appointment model = _mapper.Map<Appointment>(appointment);
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = userId;

            entities.Attach(model);
            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }

        public IEnumerable<AppointmentDTO> GetAllLite()
        {
            return _mapper.Map<List<AppointmentDTO>>(dbset.OrderBy(m => m.Id));
        }

        public AppointmentDTO GetById(int appointmentId)
        {
            return _mapper.Map<AppointmentDTO>(entities.Set<Appointment>()
                .AsSplitQuery()
                .Include(PMH => PMH.AppointmentAppointmentAdditionList)
                .ThenInclude(MH => MH.AppointmentAddition)
                .Include(a => a.AppointmentToothList)
                .Include(a => a.AttachmentList)
                .Include(a => a.Category)
                .Include(a => a.Clinic)
                .Include(a => a.Patient)
                .Include(a => a.User)
                .AsNoTracking().FirstOrDefault(c => c.Id == appointmentId));
        }

        public int Add(AppointmentDTO appointment, int userId)
        {
            Appointment model = _mapper.Map<Appointment>(appointment);
            model.CategoryId = appointment.Category.Id;
            model.ClinicId = appointment.Clinic.Id;
            model.PatientId = appointment.Patient.Id;
            model.UserId = appointment.User.Id;
            model.CreationDate = DateTime.Now;
            model.CreatedBy = userId;
            model.AttachmentList = new List<Attachment>();
            model.AppointmentToothList = new List<AppointmentTooth>();

            entities.Attach(model);
            entities.Entry(model).State = EntityState.Added;
            dbset.Add(model);
            entities.SaveChanges();
            return model.Id;
        }

        public void Update(AppointmentDTO appointment, int userId)
        {
            Appointment model = _mapper.Map<Appointment>(appointment);
            model.CategoryId = appointment.Category.Id;
            model.ClinicId = appointment.Clinic.Id;
            model.PatientId = appointment.Patient.Id;
            model.UserId = appointment.User.Id;
            model.ModifiedDate = DateTime.Now;
            model.ModifiedBy = userId;
            model.AttachmentList = new List<Attachment>();
            model.AppointmentToothList = new List<AppointmentTooth>();

            entities.Attach(model);
            entities.Entry(model).State = EntityState.Modified;
            entities.Entry(model).Property(m => m.CreatedBy).IsModified = false;
            entities.Entry(model).Property(m => m.CreationDate).IsModified = false;
        }

        public void Delete(AppointmentDTO appointment)
        {
            dbset.Remove(_mapper.Map<Appointment>(appointment));
        }

        internal void AddAppointmentAdditionList(List<AppointmentAdditionDTO> appointmentAdditionList, int appointmentId)
        {
            DbSet<AppointmentAppointmentAddition> appointmentAppointmentAdditionDBSet = entities.Set<AppointmentAppointmentAddition>();

            foreach (var appointmentAddition in appointmentAdditionList)
            {
                appointmentAppointmentAdditionDBSet.Add(new AppointmentAppointmentAddition()
                {
                    AppointmentId = appointmentId,
                    AppointmentAdditionId = appointmentAddition.Id
                });
            }
        }

        internal void AddToothList(List<AppointmentToothDTO> toothList, int appointmentId)
        {
            DbSet<AppointmentTooth> appointmentToothDBSet = entities.Set<AppointmentTooth>();

            foreach (var appointmentTooth in toothList)
            {
                appointmentToothDBSet.Add(new AppointmentTooth()
                {
                    AppointmentId = appointmentId,
                    ToothNumber = appointmentTooth.ToothNumber,
                    ToothPosition = appointmentTooth.ToothPosition,
                });
            }
        }

        internal void AddAttachmentList(List<AttachmentDTO> attachmentList, int appointmentId)
        {
            DbSet<Attachment> attachmentDBSet = entities.Set<Attachment>();

            foreach (var appointmentAddition in attachmentList)
            {
                attachmentDBSet.Add(new Attachment()
                {
                    AppointmentId = appointmentId,
                    Url = appointmentAddition.Url,
                });
            }
        }

        internal void DeleteAppointmentAdditionList(int appointmentId)
        {
            DbSet<AppointmentAppointmentAddition> appointmentAdditionDBSet = entities.Set<AppointmentAppointmentAddition>();

            IEnumerable<AppointmentAppointmentAddition> appointmentAppointmentAdditionList = appointmentAdditionDBSet.Where(p => p.AppointmentId == appointmentId);
            if (appointmentAppointmentAdditionList != null && appointmentAppointmentAdditionList.Count() > 0)
            {
                appointmentAdditionDBSet.RemoveRange(appointmentAppointmentAdditionList);
            }
        }

        internal void DeleteToothList(int appointmentId)
        {
            DbSet<AppointmentTooth> appointmentToothDBSet = entities.Set<AppointmentTooth>();

            IEnumerable<AppointmentTooth> appointmentToothList = appointmentToothDBSet.Where(p => p.AppointmentId == appointmentId);
            if (appointmentToothList != null && appointmentToothList.Count() > 0)
            {
                appointmentToothDBSet.RemoveRange(appointmentToothList);
            }
        }

        internal void DeleteAttachmentList(int appointmentId)
        {
            DbSet<Attachment> attachmentDBSet = entities.Set<Attachment>();

            IEnumerable<Attachment> attachmentList = attachmentDBSet.Where(p => p.AppointmentId == appointmentId);
            if (attachmentList != null && attachmentList.Count() > 0)
            {
                attachmentDBSet.RemoveRange(attachmentList);
            }
        }
    }
}

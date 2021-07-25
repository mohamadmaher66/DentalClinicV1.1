using DBModels;
using DTOs;
using Enums;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportModule
{
    public class ReportRepository
    {
        private DbContext entities;

        public ReportRepository(UnitOfWork UoW)
        {
            entities = UoW.DbContext;
        }

        public IEnumerable<TotalExpenseReportDTO> GetTotalExpenseReport(DateTime dateFrom, DateTime dateTo, int clinicId, int userId)
        {
            DbSet<Clinic> clinics = entities.Set<Clinic>();
            DbSet<User> users = entities.Set<User>();
            DbSet<Expense> expenses = entities.Set<Expense>();
            DbSet<AppointmentCategory> categories = entities.Set<AppointmentCategory>();
            DbSet<Patient> patients = entities.Set<Patient>();
            DbSet<Appointment> appointments = entities.Set<Appointment>();

            IEnumerable<TotalExpenseReportDTO> expenseList = from expense in expenses
                                                             join clinic in clinics on expense.ClinicId equals clinic.Id
                                                             join user in users on expense.CreatedBy equals user.Id
                                                             where (expense.ActionDate >= dateFrom && expense.ActionDate <= dateTo)
                                                                  && (clinicId == 0 || expense.ClinicId == clinicId)
                                                                  && (userId == 0 || expense.CreatedBy == userId)
                                                             orderby expense.ActionDate
                                                             select new TotalExpenseReportDTO()
                                                             {
                                                                 Cost = expense.Cost,
                                                                 Name = expense.Name,
                                                                 ActionDate = expense.ActionDate,
                                                                 Type = expense.Type,
                                                                 TypeName = expense.Type == ExpenseType.In ? "داخل للعيادة" : "خارج من العيادة",
                                                                 ClinicName = clinic.Name,
                                                                 UserFullName = user.FullName
                                                             };



            IEnumerable<TotalExpenseReportDTO> appointmentList = from appointment in appointments
                                                                 join clinic in clinics on appointment.ClinicId equals clinic.Id
                                                                 join user in users on appointment.UserId equals user.Id
                                                                 join category in categories on appointment.CategoryId equals category.Id
                                                                 join patient in patients on appointment.PatientId equals patient.Id
                                                                 where (appointment.Date >= dateFrom && appointment.Date <= dateTo)
                                                                      && (clinicId == 0 || appointment.ClinicId == clinicId)
                                                                      && (userId == 0 || appointment.UserId == userId)
                                                                 orderby appointment.Date
                                                                 select new TotalExpenseReportDTO()
                                                                 {
                                                                     Cost = appointment.PaidAmount,
                                                                     Name = patient.FullName,
                                                                     ActionDate = appointment.Date,
                                                                     Type = ExpenseType.In,
                                                                     TypeName = "كشف",
                                                                     ClinicName = clinic.Name,
                                                                     UserFullName = user.FullName,
                                                                 };

            return expenseList.Union(appointmentList).OrderByDescending(e => e.ActionDate);
        }
    }
}

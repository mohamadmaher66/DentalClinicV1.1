using Infrastructure;
using System;
using DBContext;
using DTOs;
using System.Collections.Generic;
using AutoMapper;
using Request;
using Enums;
using AspNetCore.Reporting;
using Microsoft.Extensions.Hosting;
using ExpenseModule;
using ClinicModule;
using UserModule;
using System.Linq;
using AppointmentModule;
using PatientModule;
using AppointmentCategoryModule;

namespace ReportModule
{
    public class ReportDSL
    {
        IMapper mapper;
        private readonly IHostEnvironment _hostEnvironment;

        public ReportDSL(IMapper _mapper, IHostEnvironment hostEnvironment)
        {
            mapper = _mapper;
            _hostEnvironment = hostEnvironment;
        }

        public byte[] GetExpenseReport(ExpenseFilterDTO expenseFilter)
        {
            try
            {
                ExpenseDSL expenseDSL = new ExpenseDSL(mapper);
                List<ExpenseDTO> expenseList = expenseDSL.GetExpenseReport(expenseFilter.DateFrom, expenseFilter.DateTo, expenseFilter.ClinicId, expenseFilter.UserId, expenseFilter.Type);
                
                expenseFilter.InSum = expenseList.Where(e => e.Type == ExpenseType.In).Sum(e => e.Cost);
                expenseFilter.OutSum = expenseList.Where(e => e.Type == ExpenseType.Out).Sum(e => e.Cost);
                expenseFilter.ClinicName = expenseFilter.ClinicId == 0 ? "الكل" : new ClinicDSL(mapper).GetById(expenseFilter.ClinicId).Name;
                expenseFilter.UserFullName = expenseFilter.UserId == 0 ? "الكل" : new UserDSL(mapper).GetById(expenseFilter.UserId).FullName;

                switch (expenseFilter.Type)
                {
                    case 0: expenseFilter.TypeName = "الكل"; break;
                    case ExpenseType.In: expenseFilter.TypeName = "داخل للعيادة"; break;
                    case ExpenseType.Out: expenseFilter.TypeName = "خارج من العيادة"; break;
                }
                List<ExpenseFilterDTO> parameterList = new List<ExpenseFilterDTO> { expenseFilter };
               
                return GenerateReportAsync("ExpenseReport", "ExpenseList", expenseList, parameterList);
            }
            catch (Exception e) { throw e; }
        }

        public byte[] GetAppointmentReport(AppointmentFilterDTO appointmentFilter)
        {
            try
            {
                AppointmentDSL appointmentDSL = new AppointmentDSL(mapper);
                List<AppointmentReportDTO> appointmentList = appointmentDSL.GetAppointmentReport(appointmentFilter.DateFrom, appointmentFilter.DateTo, appointmentFilter.PatientId,
                                                                               appointmentFilter.CategoryId, appointmentFilter.ClinicId, appointmentFilter.UserId, appointmentFilter.State);
                appointmentFilter.TotalPaid = appointmentList.Sum(a => a.PaidAmount);
                appointmentFilter.ClinicName = appointmentFilter.ClinicId == 0 ? "الكل" : new ClinicDSL(mapper).GetById(appointmentFilter.ClinicId).Name;
                appointmentFilter.UserFullName = appointmentFilter.UserId == 0 ? "الكل" : new UserDSL(mapper).GetById(appointmentFilter.UserId).FullName;
                appointmentFilter.PatientFullName = appointmentFilter.PatientId == 0 ? "الكل" : new PatientDSL(mapper).GetById(appointmentFilter.PatientId).FullName;
                appointmentFilter.CategoryName = appointmentFilter.CategoryId == 0 ? "الكل" : new AppointmentCategoryDSL(mapper).GetById(appointmentFilter.CategoryId).Name;

                switch (appointmentFilter.State)
                {
                    case 0: appointmentFilter.StateName = "الكل"; break;
                    case AppointmentStateEnum.Cancelled: appointmentFilter.StateName = "ملغي"; break;
                    case AppointmentStateEnum.Current: appointmentFilter.StateName = "جارى"; break;
                    case AppointmentStateEnum.Finished: appointmentFilter.StateName = "انتهى"; break;
                    case AppointmentStateEnum.Pending: appointmentFilter.StateName = "قيد الانتظار"; break;
                }
                List<AppointmentFilterDTO> parameterList = new List<AppointmentFilterDTO> { appointmentFilter };

                return GenerateReportAsync("AppointmentReport", "AppointmentList", appointmentList, parameterList);
            }
            catch (Exception e) { throw e; }
        }

        public byte[] GetTotalExpenseReport(TotalExpenseFilterDTO totalExpenseFilter)
        {
            try
            {
                ReportDSL reportDSL = new ReportDSL(mapper, _hostEnvironment);
                List<TotalExpenseReportDTO> totalExpenseList = new ReportRepository(new UnitOfWork(new DentalClinicDBContext()))
                                                                .GetTotalExpenseReport(totalExpenseFilter.DateFrom, totalExpenseFilter.DateTo, totalExpenseFilter.ClinicId, totalExpenseFilter.UserId).ToList();

                totalExpenseFilter.InSum = totalExpenseList.Where(e => e.Type == ExpenseType.In).Sum(e => e.Cost);
                totalExpenseFilter.OutSum = totalExpenseList.Where(e => e.Type == ExpenseType.Out).Sum(e => e.Cost);
                totalExpenseFilter.ClinicName = totalExpenseFilter.ClinicId == 0 ? "الكل" : new ClinicDSL(mapper).GetById(totalExpenseFilter.ClinicId).Name;
                totalExpenseFilter.UserFullName = totalExpenseFilter.UserId == 0 ? "الكل" : new UserDSL(mapper).GetById(totalExpenseFilter.UserId).FullName;
                List<TotalExpenseFilterDTO> parameterList = new List<TotalExpenseFilterDTO> { totalExpenseFilter };

                return GenerateReportAsync("TotalExpenseReport", "TotalExpenseList", totalExpenseList, parameterList);
            }
            catch (Exception e) { throw e; }
        }

        public List<DetailsList> GetExpenseDetailsLists()
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

                List<UserDTO> doctorList = new UserDSL(mapper).GetAllDoctorsLite();
                detailsList.Add(new DetailsList()
                {
                    DetailsListId = (int)DetailsListEnum.User,
                    List = doctorList
                });

                return detailsList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DetailsList> GetAppointmentDetailsLists()
        {
            return (new AppointmentDSL(mapper)).GetDetailsLists();
        }


        public byte[] GenerateReportAsync<T,Y>(string reportName, string dataSourceName, List<T> dataSourceList, List<Y> parameterList)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();

            string rdlcFilePath = string.Format("{0}\\Reports\\{1}.rdlc", _hostEnvironment.ContentRootPath, reportName);
            LocalReport report = new LocalReport(rdlcFilePath);

            report.AddDataSource(dataSourceName, dataSourceList);
            report.AddDataSource("ParameterList", parameterList);

            var result = report.Execute(RenderType.Pdf, 1, null, "");
            return result.MainStream;
        }
    }
}

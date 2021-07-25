using System;
using System.Collections.Generic;

namespace DTOs
{
    public class AppointmentReportDTO
    {
        // Parameters
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string ParamPatientFullName { get; set; }
        public string ParamClinicName { get; set; }
        public string ParamCategoryName { get; set; }
        public string ParamUserFullName { get; set; }
        public string ParamStateName { get; set; }
        public string ParamTotalPrice { get; set; }

        // List
        public string CategoryName { get; set; }
        public string UserFullName { get; set; }
        public string PatientFullName { get; set; }
        public string ClinicName { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public float DiscountPercentage { get; set; }
        public float PaidAmount { get; set; }
        public string StateName { get; set; }
    }
}

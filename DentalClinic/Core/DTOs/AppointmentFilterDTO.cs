using Enums;
using System;

namespace DTOs
{
    public class AppointmentFilterDTO
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int PatientId { get; set; }
        public string PatientFullName { get; set; }
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public AppointmentStateEnum State { get; set; }
        public string StateName { get; set; }
        public double TotalPaid { get; set; }
    }
}

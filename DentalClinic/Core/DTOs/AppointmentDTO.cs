using Enums;
using System;
using System.Collections.Generic;

namespace DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public AppointmentCategoryDTO Category { get; set; }
        public UserDTO User { get; set; }
        public PatientDTO Patient { get; set; }
        public ClinicDTO Clinic { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public float DiscountPercentage { get; set; }
        public float PaidAmount { get; set; }
        public AppointmentStateEnum State { get; set; }
        public string Notes { get; set; }
        public List<AttachmentDTO> AttachmentList { get; set; }
        public List<AppointmentToothDTO> ToothList { get; set; }
        public List<AppointmentAdditionDTO> AppointmentAdditionList { get; set; }
    }
}

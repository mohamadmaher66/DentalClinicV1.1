using Enums;
using System.Collections.Generic;

namespace DTOs
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public int Age { get; set; }
        public List<MedicalHistoryDTO> MedicalHistoryList { get; set; }
    }
}

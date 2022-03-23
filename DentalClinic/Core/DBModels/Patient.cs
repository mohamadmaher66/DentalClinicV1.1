using Enums;
using System.Collections.Generic;

namespace DBModels
{
    public partial class Patient : AuditEntity
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public int Age { get; set; }

        public ICollection<PatientMedicalHistory> PatientMedicalHistoryList { get; set; }
    }
}

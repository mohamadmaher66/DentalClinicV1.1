using System.Collections.Generic;

namespace DBModels
{
    public partial class MedicalHistory : AuditEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PatientMedicalHistory> PatientMedicalHistoryList { get; set; }
    }
}

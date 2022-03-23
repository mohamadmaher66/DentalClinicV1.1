using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels
{
    public class PatientMedicalHistory
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int MedicalHistoryId { get; set; }
        public MedicalHistory MedicalHistory { get; set; }
    }
}

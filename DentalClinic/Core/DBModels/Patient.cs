using Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels
{
    public partial class Patient : AuditModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public int Age { get; set; }

        public ICollection<PatientMedicalHistory> PatientMedicalHistoryList { get; set; }
    }
}

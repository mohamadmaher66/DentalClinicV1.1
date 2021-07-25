using Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels
{
    public class Expense : AuditModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public DateTime ActionDate { get; set; }
        public string Description { get; set; }
        public ExpenseType Type { get; set; }
        public int ClinicId { get; set; }
        public Clinic Clinic { get; set; }
    }
}

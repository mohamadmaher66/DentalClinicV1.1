using Enums;
using System;

namespace DBModels
{
    public class Expense : AuditEntity
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public DateTime ActionDate { get; set; }
        public string Description { get; set; }
        public ExpenseType Type { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
    }
}

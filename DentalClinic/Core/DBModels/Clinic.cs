using System.Collections.Generic;

namespace DBModels
{
    public class Clinic : AuditEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Expense> ExpenseList { get; set; }

    }
}

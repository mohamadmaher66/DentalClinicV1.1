using Enums;
using System;

namespace DTOs
{
    public class TotalExpenseFilterDTO
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ExpenseType Type { get; set; }
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public double InSum { get; set; }
        public double OutSum { get; set; }
    }
}

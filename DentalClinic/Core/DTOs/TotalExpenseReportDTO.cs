using Enums;
using System;

namespace DTOs
{
    public class TotalExpenseReportDTO
    {
        // Parameters
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ParamClinicName { get; set; }
        public string ParamUserFullName { get; set; }

        // List
        public string Name { get; set; }
        public double Cost { get; set; }
        public DateTime ActionDate { get; set; }
        public ExpenseType Type { get; set; }
        public string TypeName { get; set; }
        public string ClinicName { get; set; }
        public string UserFullName { get; set; }
    }
}

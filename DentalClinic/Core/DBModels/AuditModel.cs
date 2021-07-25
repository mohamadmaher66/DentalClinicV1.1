using System;
using System.Collections.Generic;
using System.Text;

namespace DBModels
{
    public class AuditModel
    {
        public int CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

using System.Collections.Generic;

namespace DBModels
{
    public class AppointmentAddition : AuditEntity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description{ get; set; }

        public ICollection<AppointmentAppointmentAddition> AppointmentAppointmentAddition { get; set; }
    }
}

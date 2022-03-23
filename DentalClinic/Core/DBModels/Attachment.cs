namespace DBModels
{
    public class Attachment : AuditEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}

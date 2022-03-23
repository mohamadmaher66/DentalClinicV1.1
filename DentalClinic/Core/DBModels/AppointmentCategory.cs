namespace DBModels
{
    public class AppointmentCategory : AuditEntity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
    }
}

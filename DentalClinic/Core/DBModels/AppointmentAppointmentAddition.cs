namespace DBModels
{
    public class AppointmentAppointmentAddition
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public int AppointmentAdditionId { get; set; }
        public AppointmentAddition AppointmentAddition { get; set; }
    }
}

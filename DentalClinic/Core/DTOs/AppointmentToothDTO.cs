using Enums;

namespace DTOs
{
    public class AppointmentToothDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int ToothNumber { get; set; }
        public ToothpositionEnum ToothPosition { get; set; }
    }
}

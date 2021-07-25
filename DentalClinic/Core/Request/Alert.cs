using Enums;

namespace Request
{
    public class Alert
    {
        public string Title { get; set; }
        public AlertTypeEnum Type { get; set; }
        public string Message { get; set; }
    }
}

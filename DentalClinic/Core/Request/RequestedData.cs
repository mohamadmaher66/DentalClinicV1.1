using System.Collections.Generic;

namespace Request
{
    public class RequestedData<T> where T : class
    {
        public T Entity { get; set; }
        public List<T> EntityList { get; set; }
        public List<Alert> Alerts { get; set; }
        public List<DetailsList> DetailsList { get; set; }
        public GridSettings GridSettings { get; set; }
        public int UserId { get; set; }

        public RequestedData()
        {
            Alerts = new List<Alert>();
        }
    }
}

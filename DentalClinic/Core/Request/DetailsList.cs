using Enums;
using System.Collections.Generic;

namespace Request
{
    public class DetailsList
    {
        public int DetailsListId { get; set; }
        public IEnumerable<object> List { get; set; }
        public DetailsListEnum Type { get; set; }
    }
}

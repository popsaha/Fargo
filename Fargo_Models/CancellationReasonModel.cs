using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class CancellationReasonModel
    {
        public long CANCELLATION_ID { get; set; }
        public string REASON { get; set; }
        public string DESCRIPTION { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class StoreTrackingModel
    {
        public long STORE_TRACKING_ID { get; set; }
        public Nullable<long> STORE_ID { get; set; }
        public string STORE_NAME { get; set; }
        public string FROM_TRACKING_NO { get; set; }
        public string TO_TRACKING_NO { get; set; }
        public string CURRENT_TRACKING_NO { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<long> CREATED_BY { get; set; }
        public string CREATED_ON { get; set; }
        public Nullable<long> MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_ON { get; set; }
        public Nullable<bool> IS_DELETED { get; set; }
        public Nullable<long> DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }
    }
}

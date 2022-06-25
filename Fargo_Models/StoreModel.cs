using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class StoreModel
    {
        public long STORE_ID { get; set; }
        public long USER_ID { get; set; }
        public string STORE_NAME { get; set; }
        public string STORE_CODE { get; set; }
        public string FROM_TRACKING_NO { get; set; }
        public string TO_TRACKING_NO { get; set; }
        public string CURRENT_TRACKING_NO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DATA_SOURCE { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<long> CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<long> MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_ON { get; set; }
        public Nullable<long> DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }
    }
}

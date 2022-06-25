using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class RoleModel
    {
        public long ROLE_ID { get; set; }
        public long USER_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public string ROLE_CODE { get; set; }
        public string LEVEL { get; set; }
        public string HIERARCHY { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<long> PARENT_ROLE_ID { get; set; }
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

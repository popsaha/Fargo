using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
   public class UserModel
    {
        public long USER_ID { get; set; }
        public string USER_CODE { get; set; }
        public string ROLE_CODE { get; set; }
        public string ROLE_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string NAME { get; set; }
        public string EMAIL_ID { get; set; }
        public string GENDER { get; set; }
        public string STREET { get; set; }
        public string LANDMARK { get; set; }
        public string CITY { get; set; }
        public string DISTRICT { get; set; }
        public Nullable<long> STATE_ID { get; set; }
        public Nullable<long> COUNTRY_ID { get; set; }
        public string PINCODE { get; set; }
        public string IMEI_NUMBER { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string CONTACT_NO { get; set; }
        public string ALTERNATE_CONTACT_NO { get; set; }
        public string PROFILE_PHOTO { get; set; }
        public string PROFILE_PHOTO_URL { get; set; }
        public Nullable<long> STORE_ID { get; set; }
        public Nullable<long> ROLE_ID { get; set; }
        public Nullable<long> PARENT_USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public Nullable<bool> IS_LOG_OUT { get; set; }
        public Nullable<System.DateTime> WEB_LOGGEDIN_ON { get; set; }
        public Nullable<System.DateTime> MOBILE_LOGGEDIN_ON { get; set; }
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

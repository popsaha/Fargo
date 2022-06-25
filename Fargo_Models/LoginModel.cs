using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class LoginModel
    {
        public string USER_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL_ID { get; set; }
        public string IMEI_NUMBER { get; set; }      
        public string ROLE_NAME { get; set; }
        public string ROLE_CODE { get; set; }
        public long USER_ID { get; set; }
        public long ROLE_ID { get; set; }
        public long CASHIER_ID { get; set; }
        public long MANAGER_ID { get; set; }
        public long STORE_ID { get; set; }
        public string LAST_LOGGEDIN_ON { get; set; }
        public string PROFILE_PHOTO { get; set; }
        public string PROFILE_PHOTO_URL { get; set; }
    }
}

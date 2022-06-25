using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }

    public class LoginResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public LoginModel Data { get; set; }  
    }
}

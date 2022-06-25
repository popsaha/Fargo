using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class RoleModuleMappingModel
    {
        public string MODULE_NAME { get; set; }
        public string SUBMENU_NAME { get; set; }
        public string MENU_NAME { get; set; }
        public string DISPLAY { get; set; }
        public int SEQUENCE { get; set; }
        public string URL { get; set; }
        public long MODULE_ID { get; set; }
        public long MENU_ID { get; set; }
        public long SUBMENU_ID { get; set; }
        public bool IS_SUBMENU { get; set; }
        public bool HAS_SUBMENU { get; set; }
        public string CONTROL_ID { get; set; }
        public long ROLE_ID { get; set; }
    }
}

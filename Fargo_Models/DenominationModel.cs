using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class DenominationModel
    {
        public long DENOMINATION_ID { get; set; }
        public string VALUE { get; set; }
        public string SYMBOL { get; set; }
        public string INITIALS { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string COUNTRY_CODE { get; set; }
        public Nullable<long> COUNTRY_ID { get; set; }
        public string DATA_SOURCE { get; set; }
        public Nullable<bool> IS_ACTIVE { get; set; }
        public Nullable<long> CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_ON { get; set; }
        public Nullable<long> MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_ON { get; set; }
        public Nullable<long> DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_ON { get; set; }

        //*******************************ADDITIONAL FEILDS*******************************
        public Nullable<float> QUANTITY { get; set; }
        public Nullable<float> AMOUNT { get; set; }
    }
    public class MobileDenomination
    {
        public long DENOMINATION_ID { get; set; }
        public string NAME { get; set; }
        public string VALUE { get; set; }
    }
}

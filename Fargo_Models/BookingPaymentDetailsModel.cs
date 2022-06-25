using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class BookingPaymentDetailsModel
    {
        public long BOOKING_PAYMENT_DETAILS_ID { get; set; }
        public Nullable<long> BOOKING_TRANSACTION_ID { get; set; }
        public string REFERENCE_NUMBER { get; set; }
        public string TRACKING_NUMBER { get; set; }
        public string PAYMENT_MODE { get; set; }
        public Nullable<double> AMOUNT { get; set; }
        public string STATUS { get; set; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
   public class BookingTransactionMasterModel
    {
        public long BOOKING_TRANSACTION_ID { get; set; }
        public Nullable<long> USER_ID { get; set; }
        public Nullable<long> CASHIER_ID { get; set; }
        public Nullable<long> STORE_ID { get; set; }
        public string IMEI_NUMBER { get; set; }
        public long CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_CONTACT { get; set; }
        public string COURIER_ADDRESS { get; set; }
        public Nullable<double> TOTAL_AMOUNT { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string DATE { get; set; }
        public string TIME { get; set; }
        public string STORE_NAME { get; set; }
        public string CASHIER_NAME { get; set; }
        public string MANAGER_NAME { get; set; }
        public string REQUESTED_ON { get; set; }
        public string RESPONDED_ON { get; set; }
        public List<BookingOrderDetailsModel> BOOKING_ORDER_DETAILS { get; set; }
        public List<BookingPaymentDetailsModel> BOOKING_PAYMENT_DETAILS { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class BookingDayEndCloseModel
    {
        public long BOOKING_DAY_END_TRANSACTION_ID { get; set; }
        public Nullable<long> STORE_ID { get; set; }
        public Nullable<long> CASHIER_ID { get; set; }
        public Nullable<bool> IS_CASHIER_APPROVED { get; set; }
        public string CASHIER_REMARK { get; set; }
        //public Nullable<long> DENOMINATION_ID { get; set; }
        //public Nullable<double> QUANTITY { get; set; }
        public Nullable<double> TOTAL_AMOUNT { get; set; }
        public Nullable<long> MANAGER_ID { get; set; }
        public Nullable<bool> IS_MANAGER_APPROVED { get; set; }
        public string MANAGER_REMARK { get; set; }
        public string STATUS { get; set; }       
        public List<DenominationModel> DENOMINATION_DETAILS { get; set; }

        //*******************************ADDITIONAL FEILDS*******************************
        public string CASHIER_NAME { get; set; }
        public string MANAGER_NAME { get; set; }
        public string STORE_NAME { get; set; }
        public string DENOMINATION_NAME { get; set; }
        public string REQUESTED_DATE { get; set; }
        public string RESPONDED_DATE { get; set; }
        public string CASHIER_APPROVED { get; set; }
        public string MANAGER_APPROVED { get; set; }
        public Nullable<long> USER_ID { get; set; }
        public Nullable<bool> IS_NEXT { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string PAGE_NUMBER { get; set; }
    }

    public class BookingDayEndCloseResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<BookingDayEndCloseModel> Data { get; set; }
    }
}

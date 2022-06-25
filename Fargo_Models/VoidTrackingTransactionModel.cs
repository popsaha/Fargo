using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fargo_Models
{
    public class VoidTrackingTransactionModel
    {
        public long VOID_TRACKING_TRANSACTION_ID { get; set; }
        public long STORE_ID { get; set; }
        public long CANCELLATION_ID { get; set; }
        public double CREDIT_NOTE_AMOUNT { get; set; }
        public string TRACKING_NUMBER { get; set; }
        public string STORE_NAME { get; set; }
        public long MANAGER_ID { get; set; }
        public long CASHIER_ID { get; set; }
        public long USER_ID { get; set; }
        public string CASHIER_NAME { get; set; }
        public bool IS_CASHIER_APPROVED { get; set; }
        public string CASHIER_REMARK { get; set; }
        public string MANAGER_NAME { get; set; }
        public bool IS_MANAGER_APPROVED { get; set; }
        public string MANAGER_REMARK { get; set; }
        public string REQUESTED_DATE { get; set; }
        public string RESPONDED_DATE { get; set; }
        public string CASHIER_APPROVED { get; set; }
        public string MANAGER_APPROVED { get; set; }
        public string STATUS { get; set; }
        public string REASON { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public Nullable<bool> IS_NEXT { get; set; }
        public string PAGE_NUMBER { get; set; }
    }

    public class VoidTrackingTransactionResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsNext { get; set; }
        public List<VoidTrackingTransactionModel> Data { get; set; }
    }
}

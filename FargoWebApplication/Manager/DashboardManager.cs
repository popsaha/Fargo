using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;

namespace FargoWebApplication.Manager
{
    public class DashboardManager
    {
        public static DataSet DataCount(out string TransactionCount, out string UserCount, out string RoleCount, out string StoreCount)
        {
            TransactionCount = string.Empty; UserCount = string.Empty; RoleCount = string.Empty; StoreCount = string.Empty;
            DataSet dataSet = new DataSet();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG","1");
                dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spDashboard", sp1);
                if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    TransactionCount = dataSet.Tables[0].Rows[0][0].ToString();
                }
                if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                {
                    UserCount = dataSet.Tables[1].Rows[0][0].ToString();
                }
                if (dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                {
                    RoleCount = dataSet.Tables[2].Rows[0][0].ToString();
                }
                if (dataSet.Tables[3] != null && dataSet.Tables[3].Rows.Count > 0)
                {
                    StoreCount = dataSet.Tables[3].Rows[0][0].ToString();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataSet;
        }


        public static List<BookingTransactionMasterModel> LstBookingTransaction(DataSet dataSet)
        {
            List<BookingTransactionMasterModel> LstBookingTransactionReport = new List<BookingTransactionMasterModel>();
            try
            {
                if (dataSet.Tables[4] != null && dataSet.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataSet.Tables[4].Rows)
                    {
                        BookingTransactionMasterModel bookingTransactionMaster = new BookingTransactionMasterModel();

                        bookingTransactionMaster.BOOKING_TRANSACTION_ID = Convert.ToInt64(dataRow["BOOKING_TRANSACTION_ID"].ToString());
                        bookingTransactionMaster.CASHIER_NAME = dataRow["FIRST_NAME"].ToString() + " " + dataRow["LAST_NAME"].ToString();
                        bookingTransactionMaster.IMEI_NUMBER = dataRow["IMEI_NUMBER"].ToString();
                        bookingTransactionMaster.CUSTOMER_NAME = dataRow["CUSTOMER_NAME"].ToString();
                        bookingTransactionMaster.CUSTOMER_CONTACT = dataRow["CUSTOMER_CONTACT"].ToString();
                        bookingTransactionMaster.TOTAL_AMOUNT = Convert.ToDouble(dataRow["TOTAL_AMOUNT"].ToString());
                        bookingTransactionMaster.STORE_NAME = dataRow["STORE_NAME"].ToString();
                        bookingTransactionMaster.DATE = dataRow["ENTRY_DATE"].ToString();

                        LstBookingTransactionReport.Add(bookingTransactionMaster);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingTransactionReport;
        }

        public static List<VoidTrackingTransactionModel> LstVoidTransaction(DataSet dataSet)
        {
            List<VoidTrackingTransactionModel> LstVoidTransaction = new List<VoidTrackingTransactionModel>();
            try
            {
                if (dataSet.Tables[5] != null && dataSet.Tables[5].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataSet.Tables[5].Rows)
                    {
                        VoidTrackingTransactionModel voidTrackingTransaction = new VoidTrackingTransactionModel();

                        voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(dataRow["VOID_TRACKING_TRANSACTION_ID"].ToString());
                        voidTrackingTransaction.STORE_ID = Convert.ToInt64(dataRow["STORE_ID"].ToString());
                        voidTrackingTransaction.CASHIER_ID = Convert.ToInt64(dataRow["CASHIER_ID"].ToString());
                        voidTrackingTransaction.MANAGER_ID = Convert.ToInt64(dataRow["MANAGER_ID"].ToString());
                        voidTrackingTransaction.IS_CASHIER_APPROVED = Convert.ToBoolean(dataRow["IS_CASHIER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_MANAGER_APPROVED = Convert.ToBoolean(dataRow["IS_MANAGER_APPROVED"].ToString());
                        voidTrackingTransaction.CASHIER_NAME = dataRow["CASHIER_NAME"].ToString();
                        voidTrackingTransaction.STORE_NAME = dataRow["STORE_NAME"].ToString();
                        voidTrackingTransaction.TRACKING_NUMBER = dataRow["TRACKING_NUMBER"].ToString();
                        voidTrackingTransaction.CASHIER_REMARK = dataRow["CASHIER_REMARK"].ToString();
                        voidTrackingTransaction.MANAGER_NAME = dataRow["MANAGER_NAME"].ToString();
                        voidTrackingTransaction.MANAGER_REMARK = dataRow["MANAGER_REMARK"].ToString();
                        voidTrackingTransaction.STATUS = dataRow["STATUS"].ToString();
                        voidTrackingTransaction.CASHIER_APPROVED = dataRow["CASHIER_APPROVED"].ToString();
                        voidTrackingTransaction.MANAGER_APPROVED = dataRow["MANAGER_APPROVED"].ToString();
                        voidTrackingTransaction.REQUESTED_DATE = dataRow["REQUESTED_DATE"].ToString();
                        voidTrackingTransaction.RESPONDED_DATE = dataRow["RESPONDED_DATE"].ToString();
                        voidTrackingTransaction.CREDIT_NOTE_AMOUNT = Convert.ToDouble(dataRow["CREDIT_NOTE_AMOUNT"].ToString());
                        voidTrackingTransaction.REASON = dataRow["REASON"].ToString();

                        LstVoidTransaction.Add(voidTrackingTransaction);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstVoidTransaction;
        }
    }
}
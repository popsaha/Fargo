using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Fargo_DataAccessLayers;
using Fargo_Models;
using Fargo_Application.App_Start;
using FargoWebApplication.Filter;

namespace FargoWebApplication.Manager
{
    public class VoidTrackingTransactionManager
    {
        public static int SubmitVoidTransactionRequest(VoidTrackingTransactionModel voidTrackingTransaction)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", voidTrackingTransaction.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@CANCELLATION_ID", voidTrackingTransaction.CANCELLATION_ID);
                SqlParameter sp3 = new SqlParameter("@CREDIT_NOTE_AMOUNT", voidTrackingTransaction.CREDIT_NOTE_AMOUNT);
                SqlParameter sp4 = new SqlParameter("@TRACKING_NUMBER", voidTrackingTransaction.TRACKING_NUMBER);
                SqlParameter sp5 = new SqlParameter("@CASHIER_ID", voidTrackingTransaction.CASHIER_ID);
                SqlParameter sp6 = new SqlParameter("@CASHIER_REMARK", voidTrackingTransaction.CASHIER_REMARK);
                SqlParameter sp7 = new SqlParameter("@CREATED_BY", voidTrackingTransaction.USER_ID);
                SqlParameter sp8 = new SqlParameter("@CREATED_ON", DateTime.Now);
                SqlParameter sp9 = new SqlParameter("@FLAG", "1");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spVoidTrackingTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<VoidTrackingTransactionModel> LstVoidTransactionRequest(string MANAGER_ID)
        {
            List<VoidTrackingTransactionModel> LstVoidTransactionRequest = new List<VoidTrackingTransactionModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@PARENT_USER_ID", MANAGER_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spVoidTrackingTransaction", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        VoidTrackingTransactionModel voidTrackingTransaction = new VoidTrackingTransactionModel();

                        voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["VOID_TRACKING_TRANSACTION_ID"].ToString());
                        voidTrackingTransaction.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        voidTrackingTransaction.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        voidTrackingTransaction.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        voidTrackingTransaction.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        voidTrackingTransaction.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        voidTrackingTransaction.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        voidTrackingTransaction.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        voidTrackingTransaction.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        voidTrackingTransaction.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        voidTrackingTransaction.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        voidTrackingTransaction.STATUS = sqlDataReader["STATUS"].ToString();
                        voidTrackingTransaction.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        voidTrackingTransaction.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        voidTrackingTransaction.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        voidTrackingTransaction.RESPONDED_DATE = sqlDataReader["RESPONDED_DATE"].ToString();
                        voidTrackingTransaction.CREDIT_NOTE_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_NOTE_AMOUNT"].ToString());
                        voidTrackingTransaction.REASON = sqlDataReader["REASON"].ToString();


                        LstVoidTransactionRequest.Add(voidTrackingTransaction);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstVoidTransactionRequest;
        }

        public static int SubmitVoidTransactionResponse(VoidTrackingTransactionModel voidTrackingTransaction)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@VOID_TRACKING_TRANSACTION_ID", voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", voidTrackingTransaction.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@TRACKING_NUMBER", voidTrackingTransaction.TRACKING_NUMBER);
                SqlParameter sp4 = new SqlParameter("@MANAGER_ID", voidTrackingTransaction.MANAGER_ID);
                SqlParameter sp5 = new SqlParameter("@IS_MANAGER_APPROVED", voidTrackingTransaction.IS_MANAGER_APPROVED);
                SqlParameter sp6 = new SqlParameter("@MANAGER_REMARK", voidTrackingTransaction.MANAGER_REMARK);
                SqlParameter sp7 = new SqlParameter("@MODIFIED_BY", voidTrackingTransaction.USER_ID);
                SqlParameter sp8 = new SqlParameter("@MODIFIED_ON", DateTime.Now);
                SqlParameter sp9 = new SqlParameter("@FLAG", "3");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spVoidTrackingTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<VoidTrackingTransactionModel> ReportVoidTransactionRequest(VoidTrackingTransactionModel _VoidTrackingTransactionModel)
        {
            List<VoidTrackingTransactionModel> ReportVoidTransactionRequest = new List<VoidTrackingTransactionModel>();
            try
            {
                string ROLE_CODE = string.Empty;
                SqlParameter _sp1 = null; SqlParameter _sp2 = null; SqlParameter _sp3 = null; SqlParameter _sp4 = null; SqlParameter _sp5 = null;
                SqlParameter sp1 = new SqlParameter("@USER_ID", _VoidTrackingTransactionModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "4");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spVoidTrackingTransaction", sp1, sp2);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    ROLE_CODE = dataTable.Rows[0][0].ToString();
                }
                if (ROLE_CODE.Equals("MNGR"))
                {
                    _sp1 = new SqlParameter("@PARENT_USER_ID", _VoidTrackingTransactionModel.USER_ID);
                    _sp2 = new SqlParameter("@FROM_DATE", _VoidTrackingTransactionModel.FROM_DATE);
                    _sp3 = new SqlParameter("@TO_DATE", _VoidTrackingTransactionModel.TO_DATE);
                    _sp4 = new SqlParameter("@PAGE_NUMBER", _VoidTrackingTransactionModel.PAGE_NUMBER);
                    _sp5 = new SqlParameter("@FLAG", "5");
                }
                if (ROLE_CODE.Equals("CSHR"))
                {
                    _sp1 = new SqlParameter("@CASHIER_ID", _VoidTrackingTransactionModel.USER_ID);
                    _sp2 = new SqlParameter("@FROM_DATE", _VoidTrackingTransactionModel.FROM_DATE);
                    _sp3 = new SqlParameter("@TO_DATE", _VoidTrackingTransactionModel.TO_DATE);
                    _sp4 = new SqlParameter("@PAGE_NUMBER", _VoidTrackingTransactionModel.PAGE_NUMBER);
                    _sp5 = new SqlParameter("@FLAG", "6");
                }
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spVoidTrackingTransaction", _sp1, _sp2, _sp3, _sp4, _sp5);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        VoidTrackingTransactionModel voidTrackingTransaction = new VoidTrackingTransactionModel();

                        voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["VOID_TRACKING_TRANSACTION_ID"].ToString());
                        voidTrackingTransaction.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        voidTrackingTransaction.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        voidTrackingTransaction.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        voidTrackingTransaction.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_NEXT = Convert.ToBoolean(sqlDataReader["IS_NEXT"].ToString());
                        voidTrackingTransaction.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        voidTrackingTransaction.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        voidTrackingTransaction.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        voidTrackingTransaction.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        voidTrackingTransaction.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        voidTrackingTransaction.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        voidTrackingTransaction.STATUS = sqlDataReader["STATUS"].ToString();
                        voidTrackingTransaction.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        voidTrackingTransaction.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        voidTrackingTransaction.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        voidTrackingTransaction.RESPONDED_DATE = sqlDataReader["RESPONDED_DATE"].ToString();
                        voidTrackingTransaction.CREDIT_NOTE_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_NOTE_AMOUNT"].ToString());
                        voidTrackingTransaction.REASON = sqlDataReader["REASON"].ToString();

                        ReportVoidTransactionRequest.Add(voidTrackingTransaction);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return ReportVoidTransactionRequest;
        }

        public static List<VoidTrackingTransactionModel> ReportVoidTransactionRequest(VoidTrackingTransactionModel _VoidTrackingTransactionModel, string DATA_SOURCE)
        {
            string fromDate = "01-01-2000"; string toDate = DateTime.Now.ToString("MM-dd-yyyy");
            List<VoidTrackingTransactionModel> LstVoidTrackingTransaction = new List<VoidTrackingTransactionModel>();
            try
            {
                string ROLE_CODE = string.Empty;
                SqlParameter _sp1 = null; SqlParameter _sp2 = null; SqlParameter _sp3 = null; SqlParameter _sp4 = null;
                _sp1 = new SqlParameter("@STORE_ID", _VoidTrackingTransactionModel.STORE_ID);
                if (string.IsNullOrEmpty(_VoidTrackingTransactionModel.FROM_DATE) && string.IsNullOrEmpty(_VoidTrackingTransactionModel.TO_DATE))
                {
                    _sp2 = new SqlParameter("@FROM_DATE", fromDate);
                    _sp3 = new SqlParameter("@TO_DATE", toDate);
                }
                else
                {
                    _sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(_VoidTrackingTransactionModel.FROM_DATE));
                    _sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(_VoidTrackingTransactionModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(_VoidTrackingTransactionModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(_VoidTrackingTransactionModel.TO_DATE));
                }
                _sp4 = new SqlParameter("@FLAG", _VoidTrackingTransactionModel.STORE_ID > 0 ? "3" : "4");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportVoidTrackingTransaction", _sp1, _sp2, _sp3, _sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        VoidTrackingTransactionModel voidTrackingTransaction = new VoidTrackingTransactionModel();

                        voidTrackingTransaction.VOID_TRACKING_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["VOID_TRACKING_TRANSACTION_ID"].ToString());
                        voidTrackingTransaction.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        voidTrackingTransaction.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        voidTrackingTransaction.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        voidTrackingTransaction.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        voidTrackingTransaction.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        voidTrackingTransaction.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        voidTrackingTransaction.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        voidTrackingTransaction.TRACKING_NUMBER = sqlDataReader["TRACKING_NUMBER"].ToString();
                        voidTrackingTransaction.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        voidTrackingTransaction.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        voidTrackingTransaction.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        voidTrackingTransaction.STATUS = sqlDataReader["STATUS"].ToString();
                        voidTrackingTransaction.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        voidTrackingTransaction.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        voidTrackingTransaction.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        voidTrackingTransaction.RESPONDED_DATE = sqlDataReader["RESPONDED_DATE"].ToString();
                        voidTrackingTransaction.CREDIT_NOTE_AMOUNT = Convert.ToDouble(sqlDataReader["CREDIT_NOTE_AMOUNT"].ToString());
                        voidTrackingTransaction.REASON = sqlDataReader["REASON"].ToString();

                        LstVoidTrackingTransaction.Add(voidTrackingTransaction);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstVoidTrackingTransaction;
        }

        public static List<StoreModel> LstStores()
        {
            List<StoreModel> LstStores = new List<StoreModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportVoidTrackingTransaction", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        StoreModel storeModel = new StoreModel();
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        LstStores.Add(storeModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstStores;
        }
    }
}
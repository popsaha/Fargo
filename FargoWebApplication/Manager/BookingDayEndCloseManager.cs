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
    public class BookingDayEndCloseManager
    {
        public static int SubmitDayEndCloseRequest(BookingDayEndCloseModel bookingDayEndCloseModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", bookingDayEndCloseModel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@CASHIER_ID", bookingDayEndCloseModel.CASHIER_ID);
                SqlParameter sp3 = new SqlParameter("@CASHIER_REMARK", bookingDayEndCloseModel.CASHIER_REMARK);
                SqlParameter sp4 = new SqlParameter("@TOTAL_AMOUNT", bookingDayEndCloseModel.TOTAL_AMOUNT);
                SqlParameter sp5 = new SqlParameter("@CREATED_BY", bookingDayEndCloseModel.USER_ID);
                SqlParameter sp6 = new SqlParameter("@CREATED_ON", DateTime.Now);
                SqlParameter sp7 = new SqlParameter("@FLAG", "1");

                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spBookingDayEndCloseTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7);
                if (dataTable != null)
                {
                    string BOOKING_DAY_END_TRANSACTION_ID = dataTable.Rows[0][0].ToString();
                    DataTable _DTDenomination = DTDenomination(bookingDayEndCloseModel.DENOMINATION_DETAILS, BOOKING_DAY_END_TRANSACTION_ID, bookingDayEndCloseModel.USER_ID.ToString());

                    SqlParameter _sp1 = new SqlParameter("@tblBookingDayEndTransactionDetails", _DTDenomination);
                    SqlParameter _sp2 = new SqlParameter("@FLAG", "2");

                    result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spBookingDayEndCloseTransaction", _sp1, _sp2);
                }    
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        private static DataTable DTDenomination(List<DenominationModel> _DENOMINATION_DETAILS, string BOOKING_DAY_END_TRANSACTION_ID, string USER_ID)
        {
            DataTable _DTDenomination = new DataTable();
            _DTDenomination.Columns.Add("BOOKING_DAY_END_TRANSACTION_ID");
            _DTDenomination.Columns.Add("DENOMINATION_ID");
            _DTDenomination.Columns.Add("QUANTITY");
            _DTDenomination.Columns.Add("AMOUNT");
            _DTDenomination.Columns.Add("DESCRIPTION");
            _DTDenomination.Columns.Add("DATA_SOURCE");
            _DTDenomination.Columns.Add("IS_ACTIVE");
            _DTDenomination.Columns.Add("CREATED_BY");
            _DTDenomination.Columns.Add("CREATED_ON");
            _DTDenomination.AcceptChanges();
            try
            {
                foreach (DenominationModel denominationModel in _DENOMINATION_DETAILS)
                {
                    _DTDenomination.Rows.Add(BOOKING_DAY_END_TRANSACTION_ID, denominationModel.DENOMINATION_ID, denominationModel.QUANTITY, denominationModel.AMOUNT, denominationModel.DESCRIPTION, "M", "1", USER_ID, DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss"));
                    _DTDenomination.AcceptChanges();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return _DTDenomination;
        }

        public static List<BookingDayEndCloseModel> LstDayEndCloseRequest(string MANAGER_ID)
        {
            List<BookingDayEndCloseModel> LstBookingDayEndCloseModel = new List<BookingDayEndCloseModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@PARENT_USER_ID", MANAGER_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spBookingDayEndCloseTransaction", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        BookingDayEndCloseModel bookingDayEndCloseModel = new BookingDayEndCloseModel();

                        bookingDayEndCloseModel.BOOKING_DAY_END_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["BOOKING_DAY_END_TRANSACTION_ID"].ToString());
                        bookingDayEndCloseModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        bookingDayEndCloseModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        bookingDayEndCloseModel.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        bookingDayEndCloseModel.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        bookingDayEndCloseModel.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_AMOUNT"].ToString());
                        bookingDayEndCloseModel.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        bookingDayEndCloseModel.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        bookingDayEndCloseModel.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        bookingDayEndCloseModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        bookingDayEndCloseModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        bookingDayEndCloseModel.STATUS = sqlDataReader["STATUS"].ToString();
                        bookingDayEndCloseModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        bookingDayEndCloseModel.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        bookingDayEndCloseModel.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        bookingDayEndCloseModel.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        bookingDayEndCloseModel.RESPONDED_DATE = sqlDataReader["RESPONDED_DATE"].ToString();

                        LstBookingDayEndCloseModel.Add(bookingDayEndCloseModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstBookingDayEndCloseModel;
        }

        public static int SubmitDayEndCloseResponse(BookingDayEndCloseModel bookingDayEndCloseModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@BOOKING_DAY_END_TRANSACTION_ID", bookingDayEndCloseModel.BOOKING_DAY_END_TRANSACTION_ID);      
                SqlParameter sp2 = new SqlParameter("@STORE_ID", bookingDayEndCloseModel.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@MANAGER_ID", bookingDayEndCloseModel.MANAGER_ID);
                SqlParameter sp4 = new SqlParameter("@IS_MANAGER_APPROVED", bookingDayEndCloseModel.IS_MANAGER_APPROVED);
                SqlParameter sp5 = new SqlParameter("@MANAGER_REMARK", bookingDayEndCloseModel.MANAGER_REMARK);
                SqlParameter sp6 = new SqlParameter("@MODIFIED_BY", bookingDayEndCloseModel.USER_ID);
                SqlParameter sp7 = new SqlParameter("@MODIFIED_ON", DateTime.Now);
                SqlParameter sp8 = new SqlParameter("@FLAG", "4");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spBookingDayEndCloseTransaction", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<BookingDayEndCloseModel> ReportDayEndCloseRequest(BookingDayEndCloseModel _BookingDayEndCloseModel)
        {
            List<BookingDayEndCloseModel> ReportDayEndCloseRequest = new List<BookingDayEndCloseModel>();
            try
            {
                string ROLE_CODE = string.Empty;
                SqlParameter _sp1 = null; SqlParameter _sp2 = null; SqlParameter _sp3 = null; SqlParameter _sp4 = null; SqlParameter _sp5 = null;
                SqlParameter sp1 = new SqlParameter("@USER_ID", _BookingDayEndCloseModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "5");
                DataTable dataTable = clsDataAccess.ExecuteDataTable(CommandType.StoredProcedure, "spBookingDayEndCloseTransaction", sp1, sp2);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    ROLE_CODE = dataTable.Rows[0][0].ToString();
                }
                if (ROLE_CODE.Equals("MNGR"))
                {
                    _sp1 = new SqlParameter("@PARENT_USER_ID", _BookingDayEndCloseModel.USER_ID);
                    _sp2 = new SqlParameter("@PAGE_NUMBER", _BookingDayEndCloseModel.PAGE_NUMBER);
                    _sp3 = new SqlParameter("@FROM_DATE", _BookingDayEndCloseModel.FROM_DATE);
                    _sp4 = new SqlParameter("@TO_DATE", _BookingDayEndCloseModel.TO_DATE);
                    _sp5 = new SqlParameter("@FLAG", "6");
                }
                if (ROLE_CODE.Equals("CSHR"))
                {
                    _sp1 = new SqlParameter("@CASHIER_ID", _BookingDayEndCloseModel.USER_ID);
                    _sp2 = new SqlParameter("@PAGE_NUMBER", _BookingDayEndCloseModel.PAGE_NUMBER);
                    _sp3 = new SqlParameter("@FROM_DATE", _BookingDayEndCloseModel.FROM_DATE);
                    _sp4 = new SqlParameter("@TO_DATE", _BookingDayEndCloseModel.TO_DATE);
                    _sp3 = new SqlParameter("@FLAG", "7");
                }
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spBookingDayEndCloseTransaction", _sp1, _sp2, _sp3, _sp4, _sp5);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        BookingDayEndCloseModel bookingDayEndCloseModel = new BookingDayEndCloseModel();

                        bookingDayEndCloseModel.BOOKING_DAY_END_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["BOOKING_DAY_END_TRANSACTION_ID"].ToString());
                        bookingDayEndCloseModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        bookingDayEndCloseModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        bookingDayEndCloseModel.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        bookingDayEndCloseModel.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        bookingDayEndCloseModel.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_AMOUNT"].ToString());
                        bookingDayEndCloseModel.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        bookingDayEndCloseModel.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        bookingDayEndCloseModel.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        bookingDayEndCloseModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        bookingDayEndCloseModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        bookingDayEndCloseModel.STATUS = sqlDataReader["STATUS"].ToString();
                        bookingDayEndCloseModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        bookingDayEndCloseModel.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        bookingDayEndCloseModel.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        bookingDayEndCloseModel.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        bookingDayEndCloseModel.RESPONDED_DATE = sqlDataReader["RESPONDED_DATE"].ToString();
                        bookingDayEndCloseModel.IS_NEXT = Convert.ToBoolean(sqlDataReader["IS_NEXT"].ToString());

                        ReportDayEndCloseRequest.Add(bookingDayEndCloseModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return ReportDayEndCloseRequest;
        }

        public static List<BookingDayEndCloseModel> ReportDayEndCloseRequest(BookingDayEndCloseModel _BookingDayEndCloseModel, string DATA_SOURCE)
        {

            string fromDate = "01-01-2022"; string toDate = DateTime.Now.AddDays(1).ToString("MM-dd-yyyy");
            List<BookingDayEndCloseModel> ReportDayEndCloseRequest = new List<BookingDayEndCloseModel>();
            try
            {
                string ROLE_CODE = string.Empty;
                SqlParameter _sp1 = null; SqlParameter _sp2 = null; SqlParameter _sp3 = null; SqlParameter _sp4 = null;
                _sp1 = new SqlParameter("@STORE_ID", _BookingDayEndCloseModel.STORE_ID);
                if (string.IsNullOrEmpty(_BookingDayEndCloseModel.FROM_DATE) && string.IsNullOrEmpty(_BookingDayEndCloseModel.TO_DATE))
                {
                    _sp2 = new SqlParameter("@FROM_DATE", fromDate);
                    _sp3 = new SqlParameter("@TO_DATE", toDate);
                }
                else
                {
                    _sp2 = new SqlParameter("@FROM_DATE", ConvertDateFormat.ConvertMMDDYYYY(_BookingDayEndCloseModel.FROM_DATE));
                    _sp3 = new SqlParameter("@TO_DATE", string.IsNullOrEmpty(_BookingDayEndCloseModel.TO_DATE) ? ConvertDateFormat.ConvertMMDDYYYY(_BookingDayEndCloseModel.FROM_DATE) : ConvertDateFormat.ConvertMMDDYYYY(_BookingDayEndCloseModel.TO_DATE));
                }
                _sp4 = new SqlParameter("@FLAG", _BookingDayEndCloseModel.STORE_ID > 0 ? "3" : "4");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportBookingDayEndCloseTransaction", _sp1, _sp2, _sp3, _sp4);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        BookingDayEndCloseModel bookingDayEndCloseModel = new BookingDayEndCloseModel();

                        bookingDayEndCloseModel.BOOKING_DAY_END_TRANSACTION_ID = Convert.ToInt64(sqlDataReader["BOOKING_DAY_END_TRANSACTION_ID"].ToString());
                        bookingDayEndCloseModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        bookingDayEndCloseModel.CASHIER_ID = Convert.ToInt64(sqlDataReader["CASHIER_ID"].ToString());
                        bookingDayEndCloseModel.MANAGER_ID = Convert.ToInt64(sqlDataReader["MANAGER_ID"].ToString());
                        bookingDayEndCloseModel.CASHIER_REMARK = sqlDataReader["CASHIER_REMARK"].ToString();
                        bookingDayEndCloseModel.TOTAL_AMOUNT = Convert.ToDouble(sqlDataReader["TOTAL_AMOUNT"].ToString());
                        bookingDayEndCloseModel.MANAGER_REMARK = sqlDataReader["MANAGER_REMARK"].ToString();
                        bookingDayEndCloseModel.CASHIER_APPROVED = sqlDataReader["CASHIER_APPROVED"].ToString();
                        bookingDayEndCloseModel.MANAGER_APPROVED = sqlDataReader["MANAGER_APPROVED"].ToString();
                        bookingDayEndCloseModel.CASHIER_NAME = sqlDataReader["CASHIER_NAME"].ToString();
                        bookingDayEndCloseModel.MANAGER_NAME = sqlDataReader["MANAGER_NAME"].ToString();
                        bookingDayEndCloseModel.STATUS = sqlDataReader["STATUS"].ToString();
                        bookingDayEndCloseModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        bookingDayEndCloseModel.IS_CASHIER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_CASHIER_APPROVED"].ToString());
                        bookingDayEndCloseModel.IS_MANAGER_APPROVED = Convert.ToBoolean(sqlDataReader["IS_MANAGER_APPROVED"].ToString());
                        bookingDayEndCloseModel.REQUESTED_DATE = sqlDataReader["REQUESTED_DATE"].ToString();
                        bookingDayEndCloseModel.RESPONDED_DATE = sqlDataReader["RESPONDED_DATE"].ToString();

                        ReportDayEndCloseRequest.Add(bookingDayEndCloseModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return ReportDayEndCloseRequest;
        }

        public static List<StoreModel> LstStores()
        {
            List<StoreModel> LstStores = new List<StoreModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spReportBookingDayEndCloseTransaction", sp1);
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
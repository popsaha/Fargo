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
    public class CreditCustomerManager
    {
        public static List<CreditCustomerModel> LstCreditCustomers()
        {
            List<CreditCustomerModel> LstCreditCustomers = new List<CreditCustomerModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCreditCustomer", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CreditCustomerModel creditCustomer = new CreditCustomerModel();

                        creditCustomer.CUSTOMER_ID = Convert.ToInt64(sqlDataReader["CUSTOMER_ID"].ToString());
                        creditCustomer.CUSTOMER_CODE = sqlDataReader["CUSTOMER_CODE"].ToString();
                        creditCustomer.CUSTOMER_NAME = sqlDataReader["CUSTOMER_NAME"].ToString();
                        creditCustomer.COMPANY = sqlDataReader["COMPANY"].ToString();
                        creditCustomer.PIN_CODE = sqlDataReader["PIN_CODE"].ToString();
                        creditCustomer.COUNTRY_ID = Convert.ToInt64(sqlDataReader["COUNTRY_ID"].ToString());
                        creditCustomer.STATE_ID = Convert.ToInt64(sqlDataReader["STATE_ID"].ToString());
                        creditCustomer.CITY = sqlDataReader["CITY"].ToString();
                        creditCustomer.ADDRESS = sqlDataReader["ADDRESS"].ToString();
                        creditCustomer.PHONE_NO = sqlDataReader["PHONE_NO"].ToString();
                        creditCustomer.EMAIL_ID = sqlDataReader["EMAIL_ID"].ToString();
                        creditCustomer.PASSWORD = sqlDataReader["PASSWORD"].ToString();
                        creditCustomer.CUSTOMER_COMMISSION = sqlDataReader["CUSTOMER_COMMISSION"].ToString();
                        creditCustomer.DATA_SOURCE = sqlDataReader["DATA_SOURCE"].ToString();

                        LstCreditCustomers.Add(creditCustomer);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCreditCustomers;
        }
    }
}
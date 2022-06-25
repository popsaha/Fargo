using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FargoWebApplication.Manager
{
    public class LoginManager
    {
        public static LoginModel ValidateLogin(LoginModel _LoginModel,string DATA_SOURCE)
        {
            LoginModel loginModel = new LoginModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@USERNAME", _LoginModel.USERNAME);
                SqlParameter sp2 = new SqlParameter("@PASSWORD", _LoginModel.PASSWORD);
                SqlParameter sp3 = new SqlParameter("@FLAG", DATA_SOURCE.Equals("W")?"1":"2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spLogin", sp1, sp2, sp3);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        loginModel.USER_ID = Convert.ToInt64(sqlDataReader["USER_ID"].ToString());
                        loginModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        loginModel.ROLE_ID = Convert.ToInt64(sqlDataReader["ROLE_ID"].ToString());
                        loginModel.USER_CODE = sqlDataReader["USER_CODE"].ToString();
                        loginModel.FIRST_NAME = sqlDataReader["FIRST_NAME"].ToString();
                        loginModel.LAST_NAME = sqlDataReader["LAST_NAME"].ToString();
                        loginModel.USERNAME = sqlDataReader["USERNAME"].ToString();
                        loginModel.EMAIL_ID = sqlDataReader["EMAIL_ID"].ToString();
                        loginModel.IMEI_NUMBER = sqlDataReader["IMEI_NUMBER"].ToString();
                        loginModel.ROLE_NAME = sqlDataReader["ROLE_NAME"].ToString();
                        loginModel.ROLE_CODE = sqlDataReader["ROLE_CODE"].ToString();
                        loginModel.PASSWORD = null;
                        loginModel.PROFILE_PHOTO = sqlDataReader["PROFILE_PHOTO"].ToString();
                        loginModel.PROFILE_PHOTO_URL = sqlDataReader["PROFILE_PHOTO_URL"].ToString();
                        loginModel.LAST_LOGGEDIN_ON = sqlDataReader["LAST_LOGGEDIN_ON"].ToString();
                        if (loginModel.ROLE_CODE == "MNGR")
                        {
                            loginModel.MANAGER_ID = loginModel.USER_ID;
                            loginModel.CASHIER_ID = 0;
                        }
                        if (loginModel.ROLE_CODE == "CSHR")
                        {
                            loginModel.CASHIER_ID = loginModel.USER_ID;
                            loginModel.MANAGER_ID = 0;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return loginModel;
        }

        public static LoginModel ForgotPassword(LoginModel _LoginModel)
        {
            LoginModel loginModel = new LoginModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@USERNAME", _LoginModel.USERNAME);
                SqlParameter sp2 = new SqlParameter("@EMAIL_ID", _LoginModel.EMAIL_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spLogin", sp1, sp2, sp3);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        loginModel.USER_ID = Convert.ToInt64(sqlDataReader["USER_ID"].ToString());
                        loginModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        loginModel.USER_CODE = sqlDataReader["USER_CODE"].ToString();
                        loginModel.FIRST_NAME = sqlDataReader["FIRST_NAME"].ToString();
                        loginModel.LAST_NAME = sqlDataReader["LAST_NAME"].ToString();
                        loginModel.USERNAME = sqlDataReader["USERNAME"].ToString();
                        loginModel.EMAIL_ID = sqlDataReader["EMAIL_ID"].ToString();
                        loginModel.IMEI_NUMBER = sqlDataReader["IMEI_NUMBER"].ToString();
                        loginModel.ROLE_NAME = sqlDataReader["ROLE_NAME"].ToString();
                        loginModel.ROLE_CODE = sqlDataReader["ROLE_CODE"].ToString();
                        loginModel.PASSWORD = sqlDataReader["PASSWORD"].ToString();

                        if (loginModel.ROLE_CODE == "MNGR003")
                        {
                            loginModel.MANAGER_ID = loginModel.USER_ID;
                            loginModel.CASHIER_ID = 0;
                        }
                        if (loginModel.ROLE_CODE == "CSHR004")
                        {
                            loginModel.CASHIER_ID = loginModel.USER_ID;
                            loginModel.MANAGER_ID = 0;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return loginModel;
        }
    }
}
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
    public class StoreManager
    {

        public static int Submit(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_NAME", storeModel.STORE_NAME);
                SqlParameter sp2 = new SqlParameter("@STORE_CODE", storeModel.STORE_CODE);
                SqlParameter sp3 = new SqlParameter("@DESCRIPTION", storeModel.DESCRIPTION);
                SqlParameter sp4 = new SqlParameter("@CREATED_BY", storeModel.USER_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "2");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int Update(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", storeModel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_NAME", storeModel.STORE_NAME);
                SqlParameter sp3 = new SqlParameter("@STORE_CODE", storeModel.STORE_CODE);
                SqlParameter sp4 = new SqlParameter("@DESCRIPTION", storeModel.DESCRIPTION);
                SqlParameter sp5 = new SqlParameter("@MODIFIED_BY", storeModel.USER_ID);
                SqlParameter sp6 = new SqlParameter("@FLAG", "4");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3, sp4, sp5, sp6);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int Delete(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", storeModel.STORE_ID);
                SqlParameter sp2 = new SqlParameter("@DELETED_BY", storeModel.USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<StoreModel> LstStores()
        {
            List<StoreModel> LstStores = new List<StoreModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spStore", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        StoreModel storeModel = new StoreModel();
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        storeModel.STORE_CODE = sqlDataReader["STORE_CODE"].ToString();
                        storeModel.FROM_TRACKING_NO = sqlDataReader["FROM_TRACKING_NO"].ToString();
                        storeModel.TO_TRACKING_NO = sqlDataReader["TO_TRACKING_NO"].ToString();
                        storeModel.CURRENT_TRACKING_NO = sqlDataReader["CURRENT_TRACKING_NO"].ToString();
                        storeModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
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

        public static StoreModel Edit(long STORE_ID)
        {
            StoreModel storeModel = new StoreModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@STORE_ID", STORE_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spStore", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        storeModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        storeModel.STORE_NAME = sqlDataReader["STORE_NAME"].ToString();
                        storeModel.STORE_CODE = sqlDataReader["STORE_CODE"].ToString();
                        storeModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return storeModel;
        }




        public static int EditStoreTrackingNo(StoreModel storeModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", storeModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@STORE_ID", storeModel.STORE_ID);
                SqlParameter sp3 = new SqlParameter("@FROM_TRACKING_NO", storeModel.FROM_TRACKING_NO);
                SqlParameter sp4 = new SqlParameter("@TO_TRACKING_NO", storeModel.TO_TRACKING_NO);
                SqlParameter sp5 = new SqlParameter("@CURRENT_TRACKING_NO", storeModel.CURRENT_TRACKING_NO);                
                SqlParameter sp6 = new SqlParameter("@FLAG", "6");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spStore", sp1, sp2, sp3, sp4, sp5, sp6);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}
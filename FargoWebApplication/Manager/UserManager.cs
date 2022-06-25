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
    public class UserManager
    {
        public static DataSet LstMasterInfo(out List<UserModel> LstUsers, out List<UserModel> LstSuperiorUsers, out List<StoreModel> LstStores, out List<RoleModel> LstRoles, out List<CountryModel> LstCountry, out List<StateModel> LstStates)
        {
            DataSet dataSet = new DataSet();
            LstUsers = null; LstSuperiorUsers = null; LstStores = null; LstRoles = null; LstCountry = null; LstStates = null;
            try
            {
                SqlParameter sp1 = new SqlParameter("@Flag", "1");
                dataSet = clsDataAccess.ExecuteDataset(CommandType.StoredProcedure, "spUser", sp1);
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0] != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        LstUsers = (from DataRow dataRow in dataSet.Tables[0].Rows
                                    select new UserModel()
                                    {
                                       USER_ID = Convert.ToInt64(dataRow["USER_ID"]),
                                       FIRST_NAME = dataRow["FIRST_NAME"].ToString(),
                                       LAST_NAME = dataRow["LAST_NAME"].ToString(),
                                       EMAIL_ID = dataRow["EMAIL_ID"].ToString(),
                                       ROLE_CODE = dataRow["ROLE_CODE"].ToString(),
                                       ROLE_NAME = dataRow["ROLE_NAME"].ToString(),
                                       IMEI_NUMBER = dataRow["IMEI_NUMBER"].ToString(),
                                       CITY = dataRow["CITY"].ToString(),
                                       CONTACT_NO = dataRow["CONTACT_NO"].ToString(),
                                       USERNAME = dataRow["USERNAME"].ToString()
                                    }).ToList(); 
                    }
                    if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                    {
                        LstSuperiorUsers = (from DataRow dataRow in dataSet.Tables[1].Rows
                                    select new UserModel()
                                    {
                                        USER_ID = Convert.ToInt64(dataRow["USER_ID"]),
                                        NAME = dataRow["FIRST_NAME"].ToString() + " " + dataRow["LAST_NAME"].ToString()
                                    }).ToList();
                    }
                    if (dataSet.Tables[2] != null && dataSet.Tables[2].Rows.Count > 0)
                    {
                        LstStores = (from DataRow dataRow in dataSet.Tables[2].Rows
                                    select new StoreModel()
                                    {
                                        STORE_ID = Convert.ToInt64(dataRow["STORE_ID"]),
                                        STORE_NAME = dataRow["STORE_NAME"].ToString()
                                    }).ToList();
                    }
                    if (dataSet.Tables[3] != null && dataSet.Tables[3].Rows.Count > 0)
                    {
                        LstRoles = (from DataRow dataRow in dataSet.Tables[3].Rows
                                    select new RoleModel()
                                    {
                                        ROLE_ID = Convert.ToInt64(dataRow["ROLE_ID"]),
                                        ROLE_NAME = dataRow["ROLE_NAME"].ToString()
                                    }).ToList();
                    }
                    if (dataSet.Tables[4] != null && dataSet.Tables[4].Rows.Count > 0)
                    {
                        LstCountry = (from DataRow dataRow in dataSet.Tables[4].Rows
                                    select new CountryModel()
                                    {
                                        COUNTRY_ID = Convert.ToInt64(dataRow["COUNTRY_ID"]),
                                        COUNTRY_NAME = dataRow["COUNTRY_NAME"].ToString()
                                    }).ToList();
                    }
                    if (dataSet.Tables[5] != null && dataSet.Tables[5].Rows.Count > 0)
                    {
                        LstStates = (from DataRow dataRow in dataSet.Tables[5].Rows
                                    select new StateModel()
                                    {
                                        STATE_ID = Convert.ToInt64(dataRow["STATE_ID"]),
                                        STATE_NAME = dataRow["STATE_NAME"].ToString()
                                    }).ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return dataSet;
        }
        public static int Submit(UserModel userMasterModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@FIRST_NAME", userMasterModel.FIRST_NAME);
                SqlParameter sp2 = new SqlParameter("@LAST_NAME", userMasterModel.LAST_NAME);
                SqlParameter sp3 = new SqlParameter("@EMAIL_ID", userMasterModel.EMAIL_ID);
                SqlParameter sp4 = new SqlParameter("@GENDER", userMasterModel.GENDER);
                SqlParameter sp5 = new SqlParameter("@STREET", userMasterModel.STREET);
                SqlParameter sp6 = new SqlParameter("@LANDMARK", userMasterModel.LANDMARK);
                SqlParameter sp7 = new SqlParameter("@CITY", userMasterModel.CITY);
                SqlParameter sp8 = new SqlParameter("@DISTRICT", userMasterModel.DISTRICT);
                SqlParameter sp9 = new SqlParameter("@PINCODE", userMasterModel.PINCODE);
                SqlParameter sp10 = new SqlParameter("@COUNTRY_ID", userMasterModel.COUNTRY_ID);
                SqlParameter sp11 = new SqlParameter("@STATE_ID", userMasterModel.STATE_ID);
                SqlParameter sp12 = new SqlParameter("@DATE_OF_BIRTH", ConvertDateFormat.ConvertMMDDYYYY(userMasterModel.DATE_OF_BIRTH));
                SqlParameter sp13 = new SqlParameter("@CONTACT_NO", userMasterModel.CONTACT_NO);
                SqlParameter sp14 = new SqlParameter("@ALTERNATE_CONTACT_NO", userMasterModel.ALTERNATE_CONTACT_NO);
                SqlParameter sp15 = new SqlParameter("@STORE_ID", userMasterModel.STORE_ID);
                SqlParameter sp16 = new SqlParameter("@ROLE_ID", userMasterModel.ROLE_ID);
                SqlParameter sp17 = new SqlParameter("@PARENT_USER_ID", userMasterModel.PARENT_USER_ID);
                SqlParameter sp18 = new SqlParameter("@USERNAME", userMasterModel.USERNAME);
                SqlParameter sp19 = new SqlParameter("@PASSWORD", userMasterModel.PASSWORD);
                SqlParameter sp20 = new SqlParameter("@PROFILE_PHOTO", userMasterModel.PROFILE_PHOTO);
                SqlParameter sp21 = new SqlParameter("@PROFILE_PHOTO_URL", userMasterModel.PROFILE_PHOTO_URL);
                SqlParameter sp22 = new SqlParameter("@CREATED_BY", userMasterModel.CREATED_BY);
                SqlParameter sp23=new SqlParameter("@FLAG","2");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spUser", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, sp11, sp12, sp13, sp14, sp15, sp16, sp17, sp18, sp19, sp20, sp21, sp22, sp23);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

       

        public static int Update(UserModel userMasterModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", userMasterModel.USER_ID);
                SqlParameter sp2 = new SqlParameter("@USER_CODE", userMasterModel.USER_CODE);
                SqlParameter sp3 = new SqlParameter("@FIRST_NAME", userMasterModel.FIRST_NAME);
                SqlParameter sp4 = new SqlParameter("@LAST_NAME", userMasterModel.LAST_NAME);
                SqlParameter sp5 = new SqlParameter("@EMAIL_ID", userMasterModel.EMAIL_ID);
                SqlParameter sp6 = new SqlParameter("@GENDER", userMasterModel.GENDER);
                SqlParameter sp7 = new SqlParameter("@STREET", userMasterModel.STREET);
                SqlParameter sp8 = new SqlParameter("@LANDMARK", userMasterModel.LANDMARK);
                SqlParameter sp9 = new SqlParameter("@CITY", userMasterModel.CITY);
                SqlParameter sp10 = new SqlParameter("@DISTRICT", userMasterModel.DISTRICT);
                SqlParameter sp11 = new SqlParameter("@PINCODE", userMasterModel.PINCODE);
                SqlParameter sp12 = new SqlParameter("@COUNTRY_ID", userMasterModel.COUNTRY_ID);
                SqlParameter sp13 = new SqlParameter("@STATE_ID", userMasterModel.STATE_ID);
                SqlParameter sp14 = new SqlParameter("@DATE_OF_BIRTH", ConvertDateFormat.ConvertMMDDYYYY(userMasterModel.DATE_OF_BIRTH));
                SqlParameter sp15 = new SqlParameter("@CONTACT_NO", userMasterModel.CONTACT_NO);
                SqlParameter sp16 = new SqlParameter("@ALTERNATE_CONTACT_NO", userMasterModel.ALTERNATE_CONTACT_NO);
                SqlParameter sp17 = new SqlParameter("@STORE_ID", userMasterModel.STORE_ID);
                SqlParameter sp18 = new SqlParameter("@ROLE_ID", userMasterModel.ROLE_ID);
                SqlParameter sp19 = new SqlParameter("@PARENT_USER_ID", userMasterModel.PARENT_USER_ID);
                SqlParameter sp20 = new SqlParameter("@USERNAME", userMasterModel.USERNAME);
                SqlParameter sp21 = new SqlParameter("@PASSWORD", userMasterModel.PASSWORD);
                SqlParameter sp22 = new SqlParameter("@PROFILE_PHOTO", userMasterModel.PROFILE_PHOTO);
                SqlParameter sp23 = new SqlParameter("@PROFILE_PHOTO_URL", userMasterModel.PROFILE_PHOTO_URL);
                SqlParameter sp24 = new SqlParameter("@MODIFIED_BY", userMasterModel.MODIFIED_BY);
                SqlParameter sp25 = new SqlParameter("@FLAG", "4");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spUser", sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, sp11, sp12, sp13, sp14, sp15, sp16, sp17, sp18, sp19, sp20, sp21, sp22, sp23,sp24, sp25);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static UserModel Edit(long USER_ID)
        {
            UserModel userMasterModel = new UserModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", USER_ID);
                SqlParameter sp2= new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spUser", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        userMasterModel.USER_ID = Convert.ToInt64(sqlDataReader["USER_ID"].ToString());
                        userMasterModel.USER_CODE = sqlDataReader["USER_CODE"].ToString();
                        userMasterModel.ROLE_CODE = sqlDataReader["ROLE_CODE"].ToString();
                        userMasterModel.ROLE_NAME = sqlDataReader["ROLE_NAME"].ToString();
                        userMasterModel.FIRST_NAME = sqlDataReader["FIRST_NAME"].ToString();
                        userMasterModel.LAST_NAME = sqlDataReader["LAST_NAME"].ToString();
                        userMasterModel.EMAIL_ID = sqlDataReader["EMAIL_ID"].ToString();
                        userMasterModel.GENDER = sqlDataReader["GENDER"].ToString();
                        userMasterModel.STREET = sqlDataReader["STREET"].ToString();
                        userMasterModel.LANDMARK = sqlDataReader["LANDMARK"].ToString();
                        userMasterModel.CITY = sqlDataReader["CITY"].ToString();
                        userMasterModel.DISTRICT = sqlDataReader["DISTRICT"].ToString();
                        userMasterModel.PINCODE = sqlDataReader["PINCODE"].ToString();
                        userMasterModel.COUNTRY_ID = Convert.ToInt64(sqlDataReader["COUNTRY_ID"].ToString());
                        userMasterModel.STATE_ID = Convert.ToInt64(sqlDataReader["STATE_ID"].ToString());
                        userMasterModel.DATE_OF_BIRTH = sqlDataReader["DATE_OF_BIRTH"].ToString();
                        userMasterModel.CONTACT_NO = sqlDataReader["CONTACT_NO"].ToString();
                        userMasterModel.ALTERNATE_CONTACT_NO = sqlDataReader["ALTERNATE_CONTACT_NO"].ToString();
                        userMasterModel.STORE_ID = Convert.ToInt64(sqlDataReader["STORE_ID"].ToString());
                        userMasterModel.ROLE_ID = Convert.ToInt64(sqlDataReader["ROLE_ID"].ToString());
                        userMasterModel.PARENT_USER_ID = Convert.ToInt64(sqlDataReader["PARENT_USER_ID"].ToString());
                        userMasterModel.USERNAME = sqlDataReader["USERNAME"].ToString();
                        userMasterModel.PASSWORD = sqlDataReader["PASSWORD"].ToString();
                        userMasterModel.PROFILE_PHOTO = sqlDataReader["PROFILE_PHOTO"].ToString();
                        userMasterModel.PROFILE_PHOTO_URL = sqlDataReader["PROFILE_PHOTO_URL"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return userMasterModel;
        }

     
        public static int Delete(long USER_ID, long? DELETED_BY)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@USER_ID", USER_ID);
                SqlParameter sp2 = new SqlParameter("@DELETED_BY", DELETED_BY);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spUser", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}
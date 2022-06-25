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
    public class RoleManager
    {
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();


        public static int Submit(RoleModel roleModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@ROLE_NAME", roleModel.ROLE_NAME);
                SqlParameter sp2 = new SqlParameter("@DESCRIPTION", roleModel.DESCRIPTION);
                SqlParameter sp3 = new SqlParameter("@CREATED_BY", roleModel.USER_ID);
                SqlParameter sp4 = new SqlParameter("@FLAG", "2");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spRole", sp1, sp2, sp3, sp4);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int Update(RoleModel roleModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@ROLE_ID", roleModel.ROLE_ID);
                SqlParameter sp2 = new SqlParameter("@ROLE_NAME", roleModel.ROLE_NAME);
                SqlParameter sp3 = new SqlParameter("@DESCRIPTION", roleModel.DESCRIPTION);
                SqlParameter sp4 = new SqlParameter("@MODIFIED_BY", roleModel.USER_ID);
                SqlParameter sp5 = new SqlParameter("@FLAG", "4");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spRole", sp1, sp2, sp3, sp4, sp5);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static int Delete(RoleModel roleModel)
        {
            int result = 0;
            try
            {
                SqlParameter sp1 = new SqlParameter("@ROLE_ID", roleModel.ROLE_ID);
                SqlParameter sp2 = new SqlParameter("@DELETED_BY", roleModel.USER_ID);
                SqlParameter sp3 = new SqlParameter("@FLAG", "5");
                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spRole", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }

        public static List<RoleModel> LstRoles()
        {
            List<RoleModel> LstRoles = new List<RoleModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spRole", sp1);
                if (sqlDataReader.HasRows) 
                {
                    while (sqlDataReader.Read())
                    {
                        RoleModel roleModel = new RoleModel();
                        roleModel.ROLE_ID = Convert.ToInt64(sqlDataReader["ROLE_ID"].ToString());
                        roleModel.ROLE_NAME = sqlDataReader["ROLE_NAME"].ToString();
                        roleModel.ROLE_CODE = sqlDataReader["ROLE_CODE"].ToString();
                        roleModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                        LstRoles.Add(roleModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstRoles;
        }

        public static RoleModel Edit(long ROLE_ID)
        {
            RoleModel roleModel = new RoleModel();
            try
            {
                SqlParameter sp1 = new SqlParameter("@ROLE_ID", ROLE_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spRole", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {  
                        roleModel.ROLE_ID = Convert.ToInt64(sqlDataReader["ROLE_ID"].ToString());
                        roleModel.ROLE_NAME = sqlDataReader["ROLE_NAME"].ToString();
                        roleModel.ROLE_CODE = sqlDataReader["ROLE_CODE"].ToString();
                        roleModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return roleModel;
        }




        public static List<RoleModuleMappingModel> LstMenu()
        {
            List<RoleModuleMappingModel> LstMenu = new List<RoleModuleMappingModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@Flag", "2");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spRoleModuleMapping", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        RoleModuleMappingModel roleModuleMapping = new RoleModuleMappingModel();

                        roleModuleMapping.MODULE_NAME = sqlDataReader["MODULE_NAME"].ToString();
                        roleModuleMapping.SUBMENU_NAME = sqlDataReader["SUBMENU_NAME"].ToString();
                        roleModuleMapping.MENU_NAME = sqlDataReader["MENU_NAME"].ToString();
                        roleModuleMapping.DISPLAY = sqlDataReader["DISPLAY"].ToString();
                        roleModuleMapping.SEQUENCE = Convert.ToInt16(sqlDataReader["SEQUENCE"].ToString());
                        roleModuleMapping.URL = sqlDataReader["URL"].ToString();
                        roleModuleMapping.MODULE_ID = Convert.ToInt64(sqlDataReader["MODULE_ID"].ToString());
                        roleModuleMapping.MENU_ID = Convert.ToInt64(sqlDataReader["MENU_ID"].ToString());
                        roleModuleMapping.SUBMENU_ID = Convert.ToInt64(sqlDataReader["SUBMENU_ID"].ToString());
                        roleModuleMapping.HAS_SUBMENU = Convert.ToBoolean(sqlDataReader["HAS_SUBMENU"].ToString());
                        roleModuleMapping.CONTROL_ID = sqlDataReader["CONTROL_ID"].ToString();

                        LstMenu.Add(roleModuleMapping);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstMenu;
        }

        public static List<RoleModuleMappingModel> LstMenu(string ROLE_ID)
        {
            List<RoleModuleMappingModel> LstMenu = new List<RoleModuleMappingModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@ROLE_ID", ROLE_ID);
                SqlParameter sp2 = new SqlParameter("@FLAG", "3");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spRoleModuleMapping", sp1, sp2);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        RoleModuleMappingModel roleModuleMapping = new RoleModuleMappingModel();

                        roleModuleMapping.MODULE_NAME = sqlDataReader["MODULE_NAME"].ToString();
                        roleModuleMapping.SUBMENU_NAME = sqlDataReader["SUBMENU_NAME"].ToString();
                        roleModuleMapping.MENU_NAME = sqlDataReader["MENU_NAME"].ToString();
                        roleModuleMapping.DISPLAY = sqlDataReader["DISPLAY"].ToString();
                        roleModuleMapping.SEQUENCE = Convert.ToInt16(sqlDataReader["SEQUENCE"].ToString());
                        roleModuleMapping.URL = sqlDataReader["URL"].ToString();
                        roleModuleMapping.MODULE_ID = Convert.ToInt64(sqlDataReader["MODULE_ID"].ToString());
                        roleModuleMapping.MENU_ID = Convert.ToInt64(sqlDataReader["MENU_ID"].ToString());
                        roleModuleMapping.SUBMENU_ID = Convert.ToInt64(sqlDataReader["SUBMENU_ID"].ToString());
                        roleModuleMapping.HAS_SUBMENU = Convert.ToBoolean(sqlDataReader["HAS_SUBMENU"].ToString());
                        roleModuleMapping.CONTROL_ID = sqlDataReader["CONTROL_ID"].ToString();

                        LstMenu.Add(roleModuleMapping);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstMenu;
        }
        public static int SubmitRoleModuleMapping(List<RoleModuleMappingModel> roleModuleMapping, long USER_ID)
        {
            int result = 0; string ROLE_ID = "";
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ROLE_ID");
                dataTable.Columns.Add("MODULE_ID");
                dataTable.Columns.Add("MENU_ID");
                dataTable.Columns.Add("SUBMENU_ID");
                dataTable.Columns.Add("IS_ACTIVE");
                dataTable.Columns.Add("DATA_SOURCE");
                dataTable.Columns.Add("CREATED_BY");
                dataTable.Columns.Add("CREATED_ON");
                dataTable.AcceptChanges();

                foreach (RoleModuleMappingModel item in roleModuleMapping)
                {
                    string[] CONTROL_ID = item.CONTROL_ID.ToString().Split('_');

                    ROLE_ID = item.ROLE_ID.ToString();
                    string MODULE_ID = CONTROL_ID[1].ToString();
                    string MENU_ID = CONTROL_ID[2].ToString();
                    string SUBMENU_ID = CONTROL_ID[3].ToString();
                    bool IS_ACTIVE = true;
                    string DATA_SOURCE = "W";
                    string CREATED_BY = USER_ID.ToString();
                    string CREATED_ON = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

                    dataTable.Rows.Add(ROLE_ID, MODULE_ID, MENU_ID, SUBMENU_ID, IS_ACTIVE, DATA_SOURCE, CREATED_BY, CREATED_ON);
                    dataTable.AcceptChanges();
                }
                SqlParameter sp1 = new SqlParameter("@ROLE_ID", ROLE_ID);
                SqlParameter sp2 = new SqlParameter("@tblRoleModuleMapping", dataTable);
                SqlParameter sp3 = new SqlParameter("@FLAG", "4");

                result = clsDataAccess.ExecuteNonQuery(CommandType.StoredProcedure, "spRoleModuleMapping", sp1, sp2, sp3);
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return result;
        }
    }
}
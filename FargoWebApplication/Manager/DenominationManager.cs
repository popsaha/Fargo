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
    public class DenominationManager
    {
        public static List<MobileDenomination> LstDenominations()
        {
            List<MobileDenomination> LstDenominations = new List<MobileDenomination>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spDenomination", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        MobileDenomination mobileDenomination = new MobileDenomination();

                        mobileDenomination.DENOMINATION_ID = Convert.ToInt64(sqlDataReader["DENOMINATION_ID"].ToString());
                        mobileDenomination.NAME = sqlDataReader["NAME"].ToString();
                        mobileDenomination.VALUE = sqlDataReader["VALUE"].ToString();

                        LstDenominations.Add(mobileDenomination);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstDenominations;
        }
    }
}
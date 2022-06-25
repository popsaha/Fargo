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
    public class CancellationReasonManager
    {
        public static List<CancellationReasonModel> LstCancellationReason()
        {
            List<CancellationReasonModel> LstCancellationReason = new List<CancellationReasonModel>();
            try
            {
                SqlParameter sp1 = new SqlParameter("@FLAG", "1");
                SqlDataReader sqlDataReader = clsDataAccess.ExecuteReader(CommandType.StoredProcedure, "spCancellationReason", sp1);
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CancellationReasonModel cancellationReasonModel = new CancellationReasonModel();

                        cancellationReasonModel.CANCELLATION_ID = Convert.ToInt64(sqlDataReader["CANCELLATION_ID"].ToString());
                        cancellationReasonModel.REASON = sqlDataReader["REASON"].ToString();
                        cancellationReasonModel.DESCRIPTION = sqlDataReader["DESCRIPTION"].ToString();

                        LstCancellationReason.Add(cancellationReasonModel);
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = ExceptionLogging.SendErrorToText(exception);
            }
            return LstCancellationReason;
        }
    }
}
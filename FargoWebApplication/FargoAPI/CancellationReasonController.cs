using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fargo_DataAccessLayers;
using FargoWebApplication.Filter;
using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Manager;
using System.Threading;


namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class CancellationReasonController : ApiController
    {
        [HttpGet]
        [Route("api/CancellationReason/LstCancellationReason")]
        public IHttpActionResult LstCancellationReason()
        {
            List<CancellationReasonModel> LstCancellationReason = new List<CancellationReasonModel>();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstCancellationReason = CancellationReasonManager.LstCancellationReason();

                    if (LstCancellationReason.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    return Ok(LstCancellationReason);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return InternalServerError();
            }
        }
    }
}

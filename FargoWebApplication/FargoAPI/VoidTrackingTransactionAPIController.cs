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
    public class VoidTrackingTransactionAPIController : ApiController
    {
        [HttpPost]
        [Route("api/VoidTrackingTransactionAPI/SubmitVoidTransactionRequest")]
        public HttpResponseMessage SubmitVoidTransactionRequest([FromBody] VoidTrackingTransactionModel voidTrackingTransaction)
         {
           
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = VoidTrackingTransactionManager.SubmitVoidTransactionRequest(voidTrackingTransaction);
                    if (result > 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "Request sent to Manager.";
                        responseModel.Description = "Request sent to Manager.";
                        return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Request not sent.";
                        responseModel.Description = "Something went wrong. Please try again.";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }

        [HttpGet]
        [Route("api/VoidTrackingTransactionAPI/LstVoidTransactionRequest")]
        public IHttpActionResult LstVoidTransactionRequest(string MANAGER_ID)
        {
            List<VoidTrackingTransactionModel> LstVoidTransactionRequest = new List<VoidTrackingTransactionModel>();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstVoidTransactionRequest = VoidTrackingTransactionManager.LstVoidTransactionRequest(MANAGER_ID);

                    if (LstVoidTransactionRequest.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    return Ok(LstVoidTransactionRequest);
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

        [HttpPost]
        [Route("api/VoidTrackingTransactionAPI/SubmitVoidTransactionResponse")]
        public HttpResponseMessage SubmitVoidTransactionResponse([FromBody] VoidTrackingTransactionModel voidTrackingTransaction)
        {
           
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = VoidTrackingTransactionManager.SubmitVoidTransactionResponse(voidTrackingTransaction);
                    if (result > 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "Response sent successfully.";
                        responseModel.Description = "Response sent successfully.";
                        return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                    }
                    else
                    {
                        responseModel.Status = "Failed";
                        responseModel.Message = "Response not sent.";
                        responseModel.Description = "Something went wrong. Please try again.";
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, responseModel);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/VoidTrackingTransactionAPI/ReportVoidTransactionRequest")]
        public HttpResponseMessage ReportVoidTransactionRequest(VoidTrackingTransactionModel _VoidTrackingTransactionModel)
        {
            VoidTrackingTransactionResponseModel ReportVoidTransactionRequest = new VoidTrackingTransactionResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    ReportVoidTransactionRequest.Data = VoidTrackingTransactionManager.ReportVoidTransactionRequest(_VoidTrackingTransactionModel);
                    if (ReportVoidTransactionRequest.Data.Count == 0)
                    {
                        ReportVoidTransactionRequest.Status = "Success";
                        ReportVoidTransactionRequest.Message = "No record found.";
                        ReportVoidTransactionRequest.Description = "No record found.";
                        ReportVoidTransactionRequest.IsNext = false;
                        return Request.CreateResponse(HttpStatusCode.OK, ReportVoidTransactionRequest);
                    }
                    else
                    {
                        ReportVoidTransactionRequest.Status = "Success";
                        ReportVoidTransactionRequest.Message = "Record's found.";
                        ReportVoidTransactionRequest.Description = ReportVoidTransactionRequest.Data.Count+" record's found.";
                        ReportVoidTransactionRequest.IsNext = ReportVoidTransactionRequest.Data[0].IS_NEXT;
                        return Request.CreateResponse(HttpStatusCode.OK, ReportVoidTransactionRequest);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }


    }
}

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
    public class BookingTransactionAPIController : ApiController
    {
         DbFargoApplicationEntities _db = new DbFargoApplicationEntities();

         [HttpPost]
         [Route("api/BookingTransactionAPI/BookingTransactionMaster")]
         public HttpResponseMessage BookingTransactionMaster([FromBody] BookingTransactionMasterModel bookingTransactionMaster)
         {
             ResponseModel responseModel = new ResponseModel();
             try
             {
                 string Username = Thread.CurrentPrincipal.Identity.Name;
                 if (!string.IsNullOrEmpty(Username))
                 {
                     int result = BookingTransactionMasterManager.SubmitBookingTransaction(bookingTransactionMaster);
                     if (result > 0)
                     {
                        responseModel.Status="Success";
                        responseModel.Message="Transaction booked successfully.";
                        responseModel.Description="Transaction booked successfully.";
                        return Request.CreateResponse(HttpStatusCode.Created, responseModel);
                     }
                     else
                     {
                        responseModel.Status="Failed";
                        responseModel.Message="Transaction not done.";
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
        
       
        
    }
}

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
    public class BookingDayEndCloseAPIController : ApiController
    {
        [HttpPost]
        [Route("api/BookingDayEndCloseAPI/SubmitDayEndCloseRequest")]
      
        public HttpResponseMessage SubmitDayEndCloseRequest([FromBody] BookingDayEndCloseModel bookingDayEndCloseModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = BookingDayEndCloseManager.SubmitDayEndCloseRequest(bookingDayEndCloseModel);
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
        [Route("api/BookingDayEndCloseAPI/LstDayEndCloseRequest")]
        public IHttpActionResult LstDayEndCloseRequest(string MANAGER_ID)
        {
            List<BookingDayEndCloseModel> LstDayEndCloseRequest = new List<BookingDayEndCloseModel>();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    LstDayEndCloseRequest = BookingDayEndCloseManager.LstDayEndCloseRequest(MANAGER_ID);

                    if (LstDayEndCloseRequest.Count == 0)
                    {
                        responseModel.Status = "Success";
                        responseModel.Message = "No record found.";
                        responseModel.Description = "No record found.";
                        return Ok(responseModel);
                    }
                    return Ok(LstDayEndCloseRequest);
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
        [Route("api/BookingDayEndCloseAPI/SubmitDayEndCloseResponse")]
        public HttpResponseMessage SubmitDayEndCloseResponse([FromBody] BookingDayEndCloseModel voidTrackingTransaction)
        {
            try
            {
                ResponseModel responseModel= new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    int result = BookingDayEndCloseManager.SubmitDayEndCloseResponse(voidTrackingTransaction);
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
        [Route("api/BookingDayEndCloseAPI/ReportDayEndCloseRequest")]
        public HttpResponseMessage ReportDayEndCloseRequest(BookingDayEndCloseModel _BookingDayEndCloseModel)
        {
            BookingDayEndCloseResponseModel ReportDayEndCloseRequest = new BookingDayEndCloseResponseModel();
            try
            {
                ResponseModel responseModel = new ResponseModel();
                string Username = Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrEmpty(Username))
                {
                    ReportDayEndCloseRequest.Data = BookingDayEndCloseManager.ReportDayEndCloseRequest(_BookingDayEndCloseModel);

                    if (ReportDayEndCloseRequest.Data.Count == 0)
                    {
                        ReportDayEndCloseRequest.Status = "Success";
                        ReportDayEndCloseRequest.Message = "No record found.";
                        ReportDayEndCloseRequest.Description = "No record found.";
                        ReportDayEndCloseRequest.IsNext = false;
                        return Request.CreateResponse(HttpStatusCode.OK, ReportDayEndCloseRequest);
                    }
                    else
                    {
                        ReportDayEndCloseRequest.Status = "Success";
                        ReportDayEndCloseRequest.Message = "Record's found.";
                        ReportDayEndCloseRequest.Description = ReportDayEndCloseRequest.Data.Count + " record's found.";
                        ReportDayEndCloseRequest.IsNext = ReportDayEndCloseRequest.Data[0].IS_NEXT;
                        return Request.CreateResponse(HttpStatusCode.OK, ReportDayEndCloseRequest);
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

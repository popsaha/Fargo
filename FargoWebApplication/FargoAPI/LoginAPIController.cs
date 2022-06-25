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
using System.Configuration;
using System.IO;
//using System.Web.Mail;
using System.Net.Mail;
using FargoWebApplication.Manager;

namespace FargoWebApplication.FargoAPI
{
    [BasicAuthentication]
    public class LoginAPIController : ApiController
    {
        DbFargoApplicationEntities _db = new DbFargoApplicationEntities();

        [HttpPost]
        [Route("api/LoginAPI/ValidateLogin")]

        public HttpResponseMessage ValidateLogin([FromBody] LoginModel _LoginModel)
        {
            try
            {
               LoginResponseModel loginResponseModel = new LoginResponseModel();
               LoginModel loginModel = LoginManager.ValidateLogin(_LoginModel, "M");
               if (loginModel.USER_ID > 0)
                {
                    loginResponseModel.Data = loginModel;
                    loginResponseModel.Status = "Success";
                    loginResponseModel.Message = "Login successfully.";
                    loginResponseModel.Description = "Credentials successfully matched.";
                }
                else
                {
                    loginResponseModel.Data = null;
                    loginResponseModel.Status = "Failed";
                    loginResponseModel.Message = "Invalid username or password.";
                    loginResponseModel.Description = "Credentials not matched.";
                }
               return Request.CreateResponse(HttpStatusCode.OK, loginResponseModel);            
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }

        [HttpPost]
        [Route("api/LoginAPI/ForgotPassword")]
        public HttpResponseMessage ForgotPassword([FromBody] LoginModel _LoginModel)
        {
            try
            {
                LoginResponseModel loginResponseModel = new LoginResponseModel();
                LoginModel loginModel = LoginManager.ForgotPassword(_LoginModel);
                if (loginModel.USER_ID > 0)
                {
                    string Password = loginModel.PASSWORD;
                    loginModel.PASSWORD = null;

                    string SenderEmailId = ConfigurationManager.AppSettings["SenderEmailId"].ToString();
                    string SenderPassword = ConfigurationManager.AppSettings["SenderPassword"].ToString();
                    string SMTPServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                    int PortNumber = Convert.ToInt16(ConfigurationManager.AppSettings["PortNumber"]);
                    Boolean IsSSLEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSLEnabled"]);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.To.Add(loginModel.EMAIL_ID);
                    mailMessage.From = new MailAddress(SenderEmailId);
                    mailMessage.Subject = "REGARDING PASSWORD RECOVERY.";
                    string Body = "Hello Ashish, Your registered password is : " + Password;
                    mailMessage.Body = Body;
                    mailMessage.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(SenderEmailId, SenderPassword); // Enter seders User name and password  
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);

                    loginResponseModel.Data = loginModel;
                    loginResponseModel.Status = "Success";
                    loginResponseModel.Message = "Password sent to your EmailId.";
                    loginResponseModel.Description = "Credentials successfully matched.";
                }
                else
                {
                    loginResponseModel.Data = null;
                    loginResponseModel.Status = "Failed";
                    loginResponseModel.Message = "Invalid Username or EmailId.";
                    loginResponseModel.Description = "Credentials not matched.";
                }
                return Request.CreateResponse(HttpStatusCode.OK, loginResponseModel);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }
        }
    }
}

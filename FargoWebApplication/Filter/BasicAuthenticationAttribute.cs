using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Fargo_DataAccessLayers;
using System.Linq;

namespace FargoWebApplication.Filter
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers
                                            .Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string Username = usernamePasswordArray[0];
                string Password = usernamePasswordArray[1];

                //  string username = "amits@yahoo.com";//usernamePasswordArray[0];
                //  string password = "Fargo@123";// usernamePasswordArray[1];

                if (AuthenticateUserAPI.Login(Username, Password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new Exception("Unauthorized, Please try again."));
                }
            }
        }

        public class AuthenticateUserAPI
        {
            public static bool Login(string Username, string Password)
            {
                using (DbFargoApplicationEntities _DB = new DbFargoApplicationEntities())
                {
                    return _DB.USER_MASTER.Any(user => user.USERNAME.Equals(Username, StringComparison.OrdinalIgnoreCase)
                                              && user.PASSWORD == Password);
                    //return EMAIL_ID.Equals("amits@yahoo.com") && PASSWORD.Equals("Fargo@123");
                }
            }
        }
    }
}
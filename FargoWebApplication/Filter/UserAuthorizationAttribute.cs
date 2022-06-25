using FargoWebApplication.Filter;
using Fargo_DataAccessLayers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Fargo_Models;

namespace FargoWebApplication.Filter
{
    public class UserAuthorizationAttribute : ActionFilterAttribute
    {
        private DbFargoApplicationEntities _db;
        public UserAuthorizationAttribute()
        {
            _db = new DbFargoApplicationEntities();
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {
                var SessionInformation = (LoginModel)HttpContext.Current.Session["SessionInformation"];
                if (SessionInformation == null)
                {
                    var CookiesInformation = HttpContext.Current.Request.Cookies["CookiesInformation"];
                    if (CookiesInformation == null)
                        actionContext.Result = new RedirectResult("/Login/Login");
                    else
                    {
                        string EncryptedCookiesInformation = CookiesInformation.Value.ToString();
                        byte[] InformationBytes = EncryptDecryptString.FromBase32String(EncryptedCookiesInformation);
                        string DecryptedCookiesInformation = System.Text.Encoding.UTF8.GetString(InformationBytes);
                        string[] UserId_password = DecryptedCookiesInformation.Split(':');

                        string EmailId = UserId_password[0];
                        string Password = Encrypt(UserId_password[1]);

                        var LoginUserDetails = _db.USER_MASTER.Where(a => a.EMAIL_ID.Trim().ToLower().Equals(EmailId) && a.PASSWORD.Equals(Password)).FirstOrDefault();
                        if (LoginUserDetails == null || LoginUserDetails.IS_LOG_OUT == true)
                            actionContext.Result = new RedirectResult("/Login/Login");
                        else
                            HttpContext.Current.Session["SessionInformation"] = LoginUserDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                actionContext.Result = new RedirectResult("/Login/Login");
            }
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}
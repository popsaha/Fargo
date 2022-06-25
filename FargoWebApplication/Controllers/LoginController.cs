using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Fargo_DataAccessLayers;
using FargoWebApplication.Filter;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Fargo_Application.App_Start;
using Fargo_Models;
using FargoWebApplication.Manager;

namespace FargoWebApplication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public ActionResult Login()
        {
            if (TempData["LoginFailed"] != null)
            {
                ViewBag.LoginFailed = TempData["LoginFailed"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Password = loginModel.PASSWORD;// Encrypt(UserMaster.PASSWORD);                 
                    LoginModel LoginUserDetails = LoginManager.ValidateLogin(loginModel, "W");                    
                    if (LoginUserDetails != null)
                    {
                        Session["SessionInformation"] = LoginUserDetails;

                        HttpCookie CookiesInformation = new HttpCookie("CookiesInformation");
                        CookiesInformation.Value = EncryptDecryptString.Encrypt(LoginUserDetails.USERNAME.Trim().ToLower().ToString() + ":" + LoginUserDetails.PASSWORD);
                        CookiesInformation.Expires = DateTime.Now.AddYears(1);
                        Response.Cookies.Add(CookiesInformation);

                        List<RoleModuleMappingModel> LstMenu = RoleManager.LstMenu(LoginUserDetails.ROLE_ID.ToString());
                        Session["LstMenu"] = LstMenu;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["LoginFailed"] = "Login Failed";
                        return RedirectToAction("Login", "Login");
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception; 
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
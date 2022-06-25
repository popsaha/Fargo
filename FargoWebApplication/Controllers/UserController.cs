using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FargoWebApplication.Controllers
{
     //[UserAuthorization]
    public class UserController : Controller
    {

         public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        // GET: User
        public ActionResult Index()
        {
            try
            {
                List<UserModel> LstUsers = null;
                List<UserModel> LstSuperiorUsers = null;
                List<StoreModel> LstStores = null;
                List<RoleModel> LstRoles = null;
                List<CountryModel> LstCountry = null;
                List<StateModel> LstStates = null;

                DataSet dataSet = UserManager.LstMasterInfo(out LstUsers, out LstSuperiorUsers, out LstStores, out LstRoles, out LstCountry, out LstStates);

                ViewBag.LstUsers = LstUsers;
                ViewBag.LstSuperiorUsers = LstSuperiorUsers;
                ViewBag.LstStores = LstStores;
                ViewBag.LstRoles = LstRoles;
                ViewBag.LstCountry = LstCountry;
                ViewBag.LstStates = LstStates;
              
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception); 
            }
            return View();
        }

        public JsonResult Edit(long USER_ID)
        {
            try
            {
                var UserInfo = UserManager.Edit(USER_ID);
                return Json(UserInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public ActionResult Create(FormCollection formCollection)
        {
            int result = 0;
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            try
            {
                UserModel userModel = new UserModel();
                string fname = ""; string Image = "";
                userModel.FIRST_NAME = formCollection["FIRST_NAME"];
                userModel.LAST_NAME = formCollection["LAST_NAME"];
                userModel.EMAIL_ID = formCollection["EMAIL_ID"].ToString();
                userModel.GENDER = formCollection["GENDER"].ToString();
                userModel.STREET = formCollection["STREET"].ToString();
                userModel.LANDMARK = formCollection["LANDMARK"].ToString();
                userModel.CITY = formCollection["CITY"].ToString();
                userModel.DISTRICT = formCollection["DISTRICT"].ToString();
                userModel.PINCODE = formCollection["PINCODE"].ToString();
                userModel.STATE_ID = Convert.ToInt64(formCollection["STATE_ID"].ToString());
                userModel.COUNTRY_ID = Convert.ToInt64(formCollection["COUNTRY_ID"].ToString());
                userModel.DATE_OF_BIRTH = formCollection["DATE_OF_BIRTH"];
                userModel.CONTACT_NO = formCollection["CONTACT_NO"].ToString();
                userModel.ALTERNATE_CONTACT_NO = formCollection["ALTERNATE_CONTACT_NO"].ToString();
                userModel.STORE_ID = Convert.ToInt64(formCollection["STORE_ID"].ToString());
                userModel.ROLE_ID = Convert.ToInt64(formCollection["ROLE_ID"].ToString());
                userModel.PARENT_USER_ID = Convert.ToInt64(formCollection["PARENT_USER_ID"].ToString());
                userModel.USERNAME = formCollection["USERNAME"].ToString();
                userModel.PASSWORD = formCollection["PASSWORD"].ToString();
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        if (i == 0)
                        {
                            HttpFileCollectionBase Signfiles = Request.Files;

                            HttpPostedFileBase Signfile = Signfiles[i];
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = Signfile.FileName.Split(new char[] { '\\' });
                                Image = testfiles[testfiles.Length - 1];
                                Image = Path.GetExtension(Image);
                            }
                            else
                            {
                                string Logo = DateTime.Now.ToString("ddMMyyyymmss");
                                fname = Signfile.FileName;
                                Image = Logo + Path.GetExtension(fname);
                                userModel.PROFILE_PHOTO = Image;
                                userModel.PROFILE_PHOTO_URL = Server.MapPath("~/Upload/UserImages/");
                            }
                            string[] AllowExtension = { ".jpg", ".jpeg", ".png" };
                            for (int j = 0; j < AllowExtension.Length; j++)
                            {
                                string ProfilePhoto = AllowExtension[j];
                                string FileAddress = Server.MapPath("~/Upload/UserImages/" + ProfilePhoto);
                                FileInfo fileInfo = new FileInfo(FileAddress);
                                if (fileInfo.Exists)
                                {
                                    fileInfo.Delete();
                                }
                            }
                            fname = Path.Combine(Server.MapPath("~/Upload/UserImages/"), Image);
                            Signfile.SaveAs(fname);
                        }
                    }
                };
                userModel.CREATED_BY = SessionInformation.USER_ID;
                result = UserManager.Submit(userModel);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Update(FormCollection formCollection)
        {
            int result = 0;
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            try
            {
                USER_MASTER _USER_MASTER = new USER_MASTER();
                UserModel userModel = new UserModel();
                string fname = ""; string Image = "";
                userModel.USER_ID = Convert.ToInt64(formCollection["USER_ID"].ToString());
                userModel.FIRST_NAME = formCollection["FIRST_NAME"];
                userModel.LAST_NAME = formCollection["LAST_NAME"];
                userModel.EMAIL_ID = formCollection["EMAIL_ID"].ToString();
                userModel.GENDER = formCollection["GENDER"].ToString();
                userModel.STREET = formCollection["STREET"].ToString();
                userModel.LANDMARK = formCollection["LANDMARK"].ToString();
                userModel.CITY = formCollection["CITY"].ToString();
                userModel.DISTRICT = formCollection["DISTRICT"].ToString();
                userModel.PINCODE = formCollection["PINCODE"].ToString();
                userModel.STATE_ID = Convert.ToInt64(formCollection["STATE_ID"].ToString());
                userModel.COUNTRY_ID = Convert.ToInt64(formCollection["COUNTRY_ID"].ToString());
                userModel.DATE_OF_BIRTH = formCollection["DATE_OF_BIRTH"];
                userModel.CONTACT_NO = formCollection["CONTACT_NO"].ToString();
                userModel.ALTERNATE_CONTACT_NO = formCollection["ALTERNATE_CONTACT_NO"].ToString();
                userModel.STORE_ID = Convert.ToInt64(formCollection["STORE_ID"].ToString());
                userModel.ROLE_ID = Convert.ToInt64(formCollection["ROLE_ID"].ToString());
                userModel.PARENT_USER_ID = Convert.ToInt64(formCollection["PARENT_USER_ID"].ToString());
                userModel.USERNAME = formCollection["USERNAME"].ToString();
                userModel.PASSWORD = formCollection["PASSWORD"].ToString();
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        if (i == 0)
                        {
                            HttpFileCollectionBase Signfiles = Request.Files;

                            HttpPostedFileBase Signfile = Signfiles[i];
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = Signfile.FileName.Split(new char[] { '\\' });
                                Image = testfiles[testfiles.Length - 1];
                                Image = Path.GetExtension(Image);
                            }
                            else
                            {
                                string Logo = DateTime.Now.ToString("ddMMyyyymmss");
                                fname = Signfile.FileName;
                                Image = Logo + Path.GetExtension(fname);
                                userModel.PROFILE_PHOTO = Image;
                                userModel.PROFILE_PHOTO_URL = Server.MapPath("~/Upload/UserImages/");
                            }
                            string[] AllowExtension = { ".jpg", ".jpeg", ".png" };
                            for (int j = 0; j < AllowExtension.Length; j++)
                            {
                                string ProfilePhoto = AllowExtension[j];
                                string FileAddress = Server.MapPath("~/Upload/UserImages/" + ProfilePhoto);
                                FileInfo fileInfo = new FileInfo(FileAddress);
                                if (fileInfo.Exists)
                                {
                                    fileInfo.Delete();
                                }
                            }
                            fname = Path.Combine(Server.MapPath("~/Upload/UserImages/"), Image);
                            Signfile.SaveAs(fname);
                        }
                    }
                };
                userModel.MODIFIED_BY = SessionInformation.USER_ID;
                result = UserManager.Update(userModel);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(UserModel userModel)
        {
            int result = 0;
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            try
            {
                userModel.DELETED_BY = SessionInformation.USER_ID;
                result = UserManager.Delete(userModel.USER_ID, userModel.DELETED_BY);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
    }
}
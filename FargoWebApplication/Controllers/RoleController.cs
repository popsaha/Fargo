using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using FargoWebApplication.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fargo_Models;
using FargoWebApplication.Manager;

namespace FargoWebApplication.Controllers
{
    //[UserAuthorization]
    public class RoleController : Controller
    {
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public ActionResult Index()
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            ViewBag.UserId = SessionInformation.USER_ID; ViewBag.Message = null;

            List<RoleModel> LstRoles = RoleManager.LstRoles();
            ViewBag.LstRoles = LstRoles;

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }            
            return View();
        }

        [HttpPost]
        public ActionResult Index(RoleModel roleModel, string Submit, string Update)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                TempData["Message"] = null;
                if (!string.IsNullOrEmpty(Submit))
                {
                    roleModel.USER_ID=SessionInformation.USER_ID;
                    int result = RoleManager.Submit(roleModel);
                    if (result > 0)
                        TempData["Message"] = "Records successfully added.";
                    else
                        TempData["Message"] = "Records not added.";
                }
                if (!string.IsNullOrEmpty(Update))
                {
                    roleModel.USER_ID = SessionInformation.USER_ID;
                    int result = RoleManager.Update(roleModel);
                    if (result > 0)
                        TempData["Message"] = "Records successfully updated.";
                    else
                        TempData["Message"] = "Records not updated.";
                }        
                return RedirectToAction("Index", "Role");
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }

        public ActionResult Edit(long ROLE_ID)
        {
            try
            {
                RoleModel roleModel = RoleManager.Edit(ROLE_ID);
                return Json(roleModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }


        public ActionResult Delete(long ROLE_ID)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];
                string Message = null;
                RoleModel roleModel = new RoleModel();
                roleModel.ROLE_ID = ROLE_ID;
                roleModel.USER_ID = SessionInformation.USER_ID;
                int result = RoleManager.Delete(roleModel);
                if (result > 0)
                    Message = "Record deleted.";
                else
                    Message = "Record not deleted."; 
                    return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }

        public ActionResult Mapping()
        {
            List<ROLE_MASTER> LST_ROLES = _db.ROLE_MASTER.ToList().Where(x => x.IS_ACTIVE == true).Select(x => new ROLE_MASTER { ROLE_ID = x.ROLE_ID, ROLE_NAME = x.ROLE_NAME, DESCRIPTION = x.DESCRIPTION }).ToList();
            ViewBag.LST_ROLES = LST_ROLES;

            List<RoleModuleMappingModel> LstMenu = RoleManager.LstMenu();
            ViewBag.LstMenu = LstMenu;

            return View();
        }

        public ActionResult LstRoleMenu(string ROLE_ID)
        {
            List<RoleModuleMappingModel> LstMenu = RoleManager.LstMenu(ROLE_ID);
            return Json(LstMenu, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SubmitRoleModuleMapping(List<RoleModuleMappingModel> roleModuleMapping)
        {
            try
            {
                var SessionInformation = (LoginModel)Session["SessionInformation"];

                int result = RoleManager.SubmitRoleModuleMapping(roleModuleMapping, SessionInformation.USER_ID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                ExceptionLogging.SendErrorToText(exception);
                throw exception;
            }
        }
   
    }
}
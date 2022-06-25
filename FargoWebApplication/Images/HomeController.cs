using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApplication.Controllers
{
    [UserAuthorization]
    public class HomeController : Controller
    {
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public ActionResult Index()
        {
            var TotalTransactionCount = _db.BOOKING_TRANSACTION_MASTER.ToList().Where(x=>x.IS_ACTIVE==true && x.IS_DELETED==false).Count();
            var TotalUserCount = _db.USER_MASTER.ToList().Where(x => x.IS_ACTIVE == true && x.IS_DELETED == false).Count();
            var TotalRoleCount = _db.ROLE_MASTER.ToList().Where(x => x.IS_ACTIVE == true && x.IS_DELETED == false).Count();
            var TotalStoreCount = _db.STORE_MASTER.ToList().Where(x => x.IS_ACTIVE == true && x.IS_DELETED == false).Count();

            ViewBag.TotalTransactionCount = TotalTransactionCount;
            ViewBag.TotalUserCount = TotalUserCount;
            ViewBag.TotalRoleCount = TotalRoleCount;
            ViewBag.TotalStoreCount = TotalStoreCount;

            return View();
        }

        public ActionResult Table()
        {
            return View();
        }
    }
}
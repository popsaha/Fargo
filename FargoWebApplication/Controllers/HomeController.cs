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
   // [UserAuthorization]
    public class HomeController : Controller
    {
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public ActionResult Index()
        {
            string TransactionCount=string.Empty;
            string UserCount=string.Empty;
            string RoleCount=string.Empty;
            string StoreCount=string.Empty;

            DataSet dataSet= DashboardManager.DataCount(out TransactionCount, out UserCount, out RoleCount, out StoreCount);
            List<BookingTransactionMasterModel> LstBookingTransaction = DashboardManager.LstBookingTransaction(dataSet);
            List<VoidTrackingTransactionModel> LstVoidTransaction = DashboardManager.LstVoidTransaction(dataSet);

            ViewBag.TotalTransactionCount = TransactionCount;
            ViewBag.TotalUserCount = UserCount;
            ViewBag.TotalRoleCount = RoleCount;
            ViewBag.TotalStoreCount = StoreCount;
            
            if (LstBookingTransaction.Count>0)
                ViewBag.LstBookingTransaction = LstBookingTransaction;
            else
                ViewBag.LstBookingTransaction = null;

            if (LstVoidTransaction.Count > 0)
                ViewBag.LstVoidTransaction = LstVoidTransaction;
            else
                ViewBag.LstVoidTransaction = null;

            return View();
        }

        public ActionResult Table()
        {
            return View();
        }
    }
}
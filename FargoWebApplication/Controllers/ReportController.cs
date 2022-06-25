using Fargo_Application.App_Start;
using Fargo_DataAccessLayers;
using Fargo_Models;
using FargoWebApplication.Filter;
using FargoWebApplication.Manager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApplication.Controllers
{
    //[UserAuthorization] 
    public class ReportController : Controller
    {
        public DbFargoApplicationEntities _db = new DbFargoApplicationEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transaction()
        {
            List<StoreModel> LstStores = BookingTransactionMasterManager.LstStores();
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");
            return View();
        }

        [HttpPost]
        public ActionResult Transaction(BookingTransactionMasterModel bookingTransactionModel)
        {
            List<StoreModel> LstStores = BookingTransactionMasterManager.LstStores();
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");

            var LstBookingTransactionReport = BookingTransactionMasterManager.BookingTransactionReport(bookingTransactionModel);
            if (LstBookingTransactionReport.Count < 1)
            {
                ViewBag.LstBookingTransactionReport = null;
                ViewBag.IsData = "0";
            }
            else
            {
                ViewBag.LstBookingTransactionReport = LstBookingTransactionReport;
                ViewBag.IsData = "1";
            }
            return View();
        }

        public ActionResult Void()
        {
            List<StoreModel> LstStores = VoidTrackingTransactionManager.LstStores();
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");

            return View();
        }

        [HttpPost]
        public ActionResult Void(VoidTrackingTransactionModel voidTrackingTransactionModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            voidTrackingTransactionModel.USER_ID = SessionInformation.USER_ID;

            List<StoreModel> LstStores = VoidTrackingTransactionManager.LstStores();
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");

            List<VoidTrackingTransactionModel> LstVoidTrackingTransaction = VoidTrackingTransactionManager.ReportVoidTransactionRequest(voidTrackingTransactionModel, "W");
            if (LstVoidTrackingTransaction.Count < 1)
            {
                ViewBag.LstVoidTrackingTransaction = null;
                ViewBag.IsData = "0";
            }
            else
            {
                ViewBag.LstVoidTrackingTransaction = LstVoidTrackingTransaction;
                ViewBag.IsData = "1";
            }
            return View();
        }

        public ActionResult DayEndClose()
        {
            List<StoreModel> LstStores = BookingDayEndCloseManager.LstStores();
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");

            return View();
        }

        [HttpPost]
        public ActionResult DayEndClose(BookingDayEndCloseModel bookingDayEndCloseModel)
        {
            var SessionInformation = (LoginModel)Session["SessionInformation"];
            bookingDayEndCloseModel.USER_ID = SessionInformation.USER_ID;

            List<StoreModel> LstStores = BookingDayEndCloseManager.LstStores();
            ViewData["LstStores"] = new SelectList(LstStores, "STORE_ID", "STORE_NAME");

            List<BookingDayEndCloseModel> LstBookingDayEndClose = BookingDayEndCloseManager.ReportDayEndCloseRequest(bookingDayEndCloseModel, "W");
            if (LstBookingDayEndClose.Count < 1)
            {
                ViewBag.LstBookingDayEndClose = null;
                ViewBag.IsData = "0";
            }
            else
            {
                ViewBag.LstBookingDayEndClose = LstBookingDayEndClose;
                ViewBag.IsData = "1";
            }
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fintech.Controllers
{
    public class AdjustmentController : Controller
    {
        // GET: Adjustment
        public ActionResult Audit()
        {
            return View();
        }

        public ActionResult ContraEntry()
        {
            return View();
        }

        public ActionResult CurrencyAdjustment()
        {
            return View();
        }

        public ActionResult JournalsHistory()
        {
            return View();
        }

        public ActionResult NewEntry()
        {
            return View();
        }

        public ActionResult NewJournal()
        {
            return View();
        }

        public ActionResult NewSubLedgerTransfer()
        {
            return View();
        }

        public ActionResult ProfitTransfer()
        {
            return View();
        }

        public ActionResult RevalueAccounts()
        {
            return View();
        }

        public ActionResult RevalueJournalTransaction()
        {
            return View();
        }

        public ActionResult RollbackTransaction()
        {
            return View();
        }

        public ActionResult SubLedgerTransfer()
        {
            return View();
        }

    }
}
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fintech.Controllers
{
    public class BankingController : Controller
    {
        // GET: Banking
        public ActionResult banking_dashboard()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult Receipt()
        {
            return View();
        }

        // Save Bank account manually
        [HttpPost]
        public JsonResult SaveBankAccount(BankAccountCreate model)
        {
            var bal = new fintech.BAL.BankingBAL();

            int entityId = Convert.ToInt32(Session["EntityID"]);
            int? loginId = Session["LoginID"] as int?;

            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = bal.SaveBankAccount(model, entityId, loginId, out messageCode);
                result = new
                {
                    status = success,
                    message = messageCode
                };
            }
            catch (Exception ex)
            {
                result = new
                {
                    status = false,
                    message = ex.Message
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
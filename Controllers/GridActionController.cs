// ============================================================
// GridActionController.cs
// Handles all 5 grid actions for all 4 modules from one place.
// Route: /GridAction/{action}
// ============================================================

using System;
using System.Web.Mvc;
using YourApp.BAL;
using YourApp.Models;

namespace YourApp.Controllers
{
    public class GridActionController : Controller
    {
        private readonly GridActionBAL _bal;

        public GridActionController()
        {
            _bal = new GridActionBAL();
        }

        // ── 1. PRINT ─────────────────────────────────────────
        [HttpPost]
        public JsonResult Print(GridActionRequest request)
        {
            try
            {
                if (!_IsValid(request, out string err))
                    return Json(new { success = false, message = err });

                byte[] pdfBytes = _bal.GeneratePdf(request.ModuleType, request.Ids);
                return Json(new
                {
                    success = true,
                    pdfBase64 = Convert.ToBase64String(pdfBytes)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ── 2. EMAIL ──────────────────────────────────────────
        [HttpPost]
        public JsonResult Email(GridActionEmailRequest request)
        {
            try
            {
                if (!_IsValid(request, out string err))
                    return Json(new { success = false, message = err });

                if (string.IsNullOrWhiteSpace(request.ToEmail))
                    return Json(new { success = false, message = "Recipient email is required." });

                byte[] pdfBytes = _bal.GeneratePdf(request.ModuleType, request.Ids);
                _bal.SendEmail(request.ModuleType, request.ToEmail, request.EmailSub, request.EmailBody, pdfBytes);

                return Json(new { success = true, message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ── 4. DOWNLOAD (PDF) ─────────────────────────────────
        //[HttpPost]
        //public JsonResult Download(GridActionRequest request)
        //{
        //    try
        //    {
        //        if (!_IsValid(request, out string err))
        //            return Json(new { success = false, message = err });

        //        byte[] pdfBytes = _bal.GeneratePdf(request.ModuleType, request.Ids);
        //        return Json(new
        //        {
        //            success = true,
        //            pdfBase64 = Convert.ToBase64String(pdfBytes)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        // ── 5. DELETE ─────────────────────────────────────────
        [HttpPost]
        public JsonResult Delete(GridActionRequest request)
        {
            try
            {
                if (!_IsValid(request, out string err))
                    return Json(new { success = false, message = err });

                int deletedCount = _bal.DeleteRecords(request.ModuleType, request.Ids);
                return Json(new
                {
                    success = true,
                    message = $"{deletedCount} {(deletedCount>1?"records":"record")}  deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ── Shared validation ─────────────────────────────────
        private bool _IsValid(GridActionRequest request, out string error)
        {
            error = null;
            var allowed = new[] { "Estimate", "Invoice", "RecurringInvoice","SalesOrder", "CreditNote", "PurchaseOrder" , "PurchaseCreditNote", "PurchaseRecurringBills", "PurchaseBills", "ExpenseHistory" };

            if (request == null)
            { error = "Invalid request."; return false; }

            if (string.IsNullOrWhiteSpace(request.ModuleType) ||
                Array.IndexOf(allowed, request.ModuleType) < 0)
            { error = "Invalid module type."; return false; }

            if (request.Ids == null || request.Ids.Length == 0)
            { error = "No records selected."; return false; }

            return true;
        }
    }
}

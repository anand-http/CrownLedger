using fintech.Filters;
using fintech.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fintech.Controllers
{
    [FinFilter]
    public class HomeController : Controller
    {
        public ActionResult DashBoard()
        {
            return View();
        }
        public ActionResult company_Details(int id = 0)
        {
            try
            {
                var bal = new fintech.BAL.HomeBAL();
                var company = bal.GetCompanyDetails(id);
                if (company == null)
                {
                    company = new fintech.Company();
                }
                return View(company);
            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        public ActionResult chart_of_accounts()
        {
            try
            {
                var bal = new fintech.BAL.HomeBAL();
                int? entityId = Session["EntityID"] as int?;
                int length = 10;
                string orderby = "order by GL_CODE desc";
                var accounts = bal.GetAllChartOfAccounts(entityId, 0, length, orderby, null, null) ?? new List<fintech.GetAllChartOfAccounts_Result>();
                return View(accounts);
            }
            catch (Exception ex) {
                return Content(ex.ToString());
            }
            
        }

        public JsonResult GetAccountList(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault());
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault());
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault());

            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() +
                                        "][name]")?.FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();


            var statusFilter = Request.Form.GetValues("statusFilter")?.FirstOrDefault() ?? "";
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();

            for (int i = 0; i < icolelngth; i++)
            {
                if (Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() != "")
                {
                    search = search + " and " + Request.Form.GetValues("columns[" + i + "][name]")?.FirstOrDefault() + " like '%" + Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() + "%'";
                }
            }

            string Orderby = "";
            if (sortColumn != "")
            {
                Orderby = "Order By " + sortColumn + " " + sortColumnDir;
            }

            var homebal = new fintech.BAL.HomeBAL();
            List<GetAllChartOfAccounts_Result> chartofacct = homebal.GetAllChartOfAccounts(entityId, start, length, Orderby, search, statusFilter);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (chartofacct.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(chartofacct.First().TotalRecord);
                dataTableData.recordsFiltered = Convert.ToInt64(chartofacct.First().TotalRecord);
            }
            dataTableData.data = chartofacct;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult opening_balance()
        {
            try
            {
                var bal = new fintech.BAL.HomeBAL();
                //var accounts = bal.GetOpeningBalanceGridData(DateTime.Now);
                return View("opening_balance");
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
        public ActionResult Connect_bank_account()
        {
            try
            {
                var bal = new fintech.BAL.HomeBAL();
                var model = new fintech.Models.BankAccountViewModel
                {
                    Banks = bal.GetBankList(),
                    Accounts = bal.GetBankAccounts()
                };
                return View("Connect_bank_account", model);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpPost]
        public JsonResult DeleteBankAccount(int accountId)
        {
            try
            {
                var bal = new fintech.BAL.HomeBAL();
                var result = bal.DeleteBankAccount(accountId);
                return Json(new { success = result > 0, message = result > 0 ? "Account deleted successfully." : "Delete failed." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        public ActionResult Product_and_Service()
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int length = 10;
                string Orderby = "order by ItemName desc";
                var bal = new fintech.BAL.HomeBAL();
                var prodnser = bal.GetProductAndService(entityId, 0, length, Orderby, null, null,null,null);
                return View(prodnser);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        public JsonResult GetProdnServiceList(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault());
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault());
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault());

            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() +
                                        "][name]")?.FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();


            var statusFilter = Request.Form.GetValues("statusFilter")?.FirstOrDefault() ?? "";
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var startDate = Request.Form.GetValues("startDate")?.FirstOrDefault() ?? "";
            var endDate = Request.Form.GetValues("endDate")?.FirstOrDefault() ?? "";
            for (int i = 0; i < icolelngth; i++)
            {
                if (Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() != "")
                {
                    search = search + " and " + Request.Form.GetValues("columns[" + i + "][name]")?.FirstOrDefault() + " like '%" + Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() + "%'";
                }
            }

            string Orderby = "";
            if (sortColumn != "")
            {
                Orderby = "Order By " + sortColumn + " " + sortColumnDir;
            }

            var homebal = new fintech.BAL.HomeBAL();
            List<sp_GetProductAndService_Result> chartofacct = homebal.GetProductAndService(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (chartofacct.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(chartofacct.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(chartofacct.First().Records);
            }
            dataTableData.data = chartofacct;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult maintain_tax_details()
        {
            int entityId = Convert.ToInt32(Session["EntityID"]);
            int length = 10;
            string orderby = "Order By Tax_ID desc";
            var bal = new fintech.BAL.HomeBAL();

            // Load all three lists separately for initial page load
            var model = new TaxDetailsViewModel
            {
                GSTList = bal.GetTaxList(entityId, 0, length, orderby, null, gst: 1, tds: 0, other: 0),
                TDSList = bal.GetTaxList(entityId, 0, length, orderby, null, gst: 0, tds: 1, other: 0),
                OtherList = bal.GetTaxList(entityId, 0, length, orderby, null, gst: 0, tds: 0, other: 1)
            };

            return View(model);
        }

        public JsonResult GettaxList(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;

            // ── Safety check — if session lost, return early ──────────────────
            if (entityId == null)
                return Json(new { error = "Session expired" }, JsonRequestBehavior.AllowGet);

            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault() ?? "1");
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault() ?? "10");

            var sortColumn = Request.Form.GetValues(
                "columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]"
            )?.FirstOrDefault() ?? "";

            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault() ?? "desc";

            var search = Request.Form.GetValues("search[value]")?.FirstOrDefault() ?? "";

            // ── Tax Type Flags ────────────────────────────────────────────────
            int gst = int.Parse(Request.Form.GetValues("gst")?.FirstOrDefault() ?? "0");
            int tds = int.Parse(Request.Form.GetValues("tds")?.FirstOrDefault() ?? "0");
            int other = int.Parse(Request.Form.GetValues("other")?.FirstOrDefault() ?? "0");

            // ── Column-level search ───────────────────────────────────────────
            for (int i = 0; i < icolelngth; i++)
            {
                var colSearch = Request.Form.GetValues($"columns[{i}][search][value]")?.FirstOrDefault();
                var colName = Request.Form.GetValues($"columns[{i}][name]")?.FirstOrDefault();

                if (!string.IsNullOrEmpty(colSearch) && !string.IsNullOrEmpty(colName))
                    search += $" and {colName} like '%{colSearch}%'";
            }

            // ── Order By ──────────────────────────────────────────────────────
            string orderby = !string.IsNullOrEmpty(sortColumn)
                ? $"Order By {sortColumn} {sortColumnDir}"
                : "Order By Tax_ID desc";

            var homebal = new fintech.BAL.HomeBAL();
            List<mst_Tax> taxList = homebal.GetTaxList(
                entityId, start, length, orderby,
                string.IsNullOrEmpty(search) ? null : search,  // ← pass null if empty, not ""
                gst: gst, tds: tds, other: other
            );

            return Json(new lDataTable
            {
                draw = draw,
                recordsTotal = taxList.Any() ? Convert.ToInt64(taxList.First().Records) : 0,
                recordsFiltered = taxList.Any() ? Convert.ToInt64(taxList.First().Records) : 0,
                data = taxList
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult maintain_currency_details()
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int length = 10;
                string Orderby = "order by Curncy_ID desc";
                var bal = new fintech.BAL.HomeBAL();
                var currency = bal.GetCurrencyList(entityId, 0, length, Orderby, null);
                return View(currency);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        public JsonResult GetCurrencyList(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;

            // ── Safety check — if session lost, return early ──────────────────
            if (entityId == null)
                return Json(new { error = "Session expired" }, JsonRequestBehavior.AllowGet);

            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault() ?? "1");
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault() ?? "10");

            var sortColumn = Request.Form.GetValues(
                "columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() + "][name]"
            )?.FirstOrDefault() ?? "";

            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault() ?? "desc";

            var search = Request.Form.GetValues("search[value]")?.FirstOrDefault() ?? "";

            // ── Tax Type Flags ────────────────────────────────────────────────
            int gst = int.Parse(Request.Form.GetValues("gst")?.FirstOrDefault() ?? "0");
            int tds = int.Parse(Request.Form.GetValues("tds")?.FirstOrDefault() ?? "0");
            int other = int.Parse(Request.Form.GetValues("other")?.FirstOrDefault() ?? "0");

            // ── Column-level search ───────────────────────────────────────────
            for (int i = 0; i < icolelngth; i++)
            {
                var colSearch = Request.Form.GetValues($"columns[{i}][search][value]")?.FirstOrDefault();
                var colName = Request.Form.GetValues($"columns[{i}][name]")?.FirstOrDefault();

                if (!string.IsNullOrEmpty(colSearch) && !string.IsNullOrEmpty(colName))
                    search += $" and {colName} like '%{colSearch}%'";
            }

            // ── Order By ──────────────────────────────────────────────────────
            string orderby = !string.IsNullOrEmpty(sortColumn)
                ? $"Order By {sortColumn} {sortColumnDir}"
                : "Order By Curncy_ID desc";

            var homebal = new fintech.BAL.HomeBAL();
            mst_Currency currencylist = homebal.GetCurrencyList(entityId, start, length, orderby, search);

            return Json(new lDataTable
            {
                draw = draw,
                recordsTotal = currencylist.CurrencyList.Any() ? Convert.ToInt64(currencylist.CurrencyList.First().TotalRecord) : 0,
                recordsFiltered = currencylist.CurrencyList.Any() ? Convert.ToInt64(currencylist.CurrencyList.First().TotalRecord) : 0,
                data = new
                {
                    CurrencyList = currencylist.CurrencyList,
                    ExchangeRate = currencylist.ExchangeRate
                }
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult add_currency()
        {
            return View("add_currency");
        }


        [ValidateAntiForgeryToken]
        public ActionResult AddNewTax(mst_Tax model)
        {
            if (ModelState.IsValid)
            {
                var bal = new fintech.BAL.HomeBAL();
                model.Entity_ID = Convert.ToInt32(Session["EntityID"]);
                model.Login_ID = Convert.ToInt32(Session["LoginID"]);
                bal.AddNewTax(model);
                // Optionally, you can redirect or return a partial view update
                return RedirectToAction("maintain_tax_details");
            }
            return View("maintain_tax_details", model);
        }
        public ActionResult CurrencyRateDetails()
        {
            return View("currency-rate-details");
        }
        public ActionResult CurrencyRateType()
        {
            return View("currency-rate-type");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult SaveOrUpdateCompany(GetCompanyDetails_Result model)
        {
            try
            {
                if (model.CreatedAt == default(DateTime))
                    model.CreatedAt = DateTime.Now;
                var bal = new fintech.BAL.HomeBAL();
                model.Entity_Id = Convert.ToInt32(Session["EntityID"]);
                model.Login_Id = Session["LoginID"] as int?;
                var id = bal.SaveOrUpdateCompany(model);
                return Json(new { success = true, message = "Company details saved successfully.", id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveCompanyDetailsDraft(GetCompanyDetails_Result model)
        {
            try
            {
                if (model.CreatedAt == default(DateTime))
                    model.CreatedAt = DateTime.Now;
                var bal = new fintech.BAL.HomeBAL();
                var id = bal.SaveOrUpdateCompany(model); // Optionally set a draft flag in the model if you add such a field
                return Json(new { success = true, message = "Company details saved as draft.", id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult EditCompanyDetails(GetCompanyDetails_Result model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var bal = new fintech.BAL.HomeBAL();
                    bal.SaveOrUpdateCompany(model);
                    return Json(new { success = true, message = "Company details updated successfully." });
                }
                return Json(new { success = false, message = "Invalid data." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveGLCode(GetAllChartOfAccounts_Result model)
        {
            try
            {
                //model.Entity_ID = Convert.ToInt32(Session["EntityID"]);
                model.Entity_ID = 10;
                //model.Login_ID = Convert.ToInt32(Session["LoginID"]);
                model.Login_ID = 1;
                var bal = new fintech.BAL.HomeBAL();
                string msgCode = bal.SaveOrUpdateGLCode(model, User.Identity.Name);

                return Json(new { success = true, message = msgCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SaveGLCode Error: " + ex.ToString());
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveProductAndService(sp_GetProductAndService_Result model)
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int? LoginId = Session["LoginID"] as int?;
                model.Entity_Id = entityId;
                model.Login_Id = LoginId;
                var bal = new fintech.BAL.HomeBAL();
                string messageCode = bal.SaveOrUpdateProductAndService(model);

                return Json(new
                {
                    success = messageCode == "I0001",
                    message = messageCode == "I0001" ? "Saved successfully." : "Something went wrong. Code: " + messageCode
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error: " + ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult SaveTax(mst_Tax model, List<TaxDetail> Details)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid data." });

                if (string.IsNullOrWhiteSpace(model.Tax_Code))
                    return Json(new { success = false, message = "Tax Code is required." });

                model.Entity_ID = Convert.ToInt32(Session["EntityID"]);
                model.Login_ID = Convert.ToInt32(Session["LoginID"]);

                var bal = new fintech.BAL.HomeBAL();
                bool result = bal.SaveTax(model, Details ?? new List<TaxDetail>());

                return result
                    ? Json(new { success = true, message = "Tax saved successfully." })
                    : Json(new { success = false, message = "Failed to save tax." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveExchangerate(ExchangeRateViewModel model)
        {
            try
            {
                if (model == null)
                    return Json(new ApiResponse { Success = false, Message = "Invalid request data." });

                var bal = new fintech.BAL.HomeBAL();
                model.Entity_ID = Convert.ToInt32(Session["EntityID"]);
                model.Login_ID = Session["LoginID"] as int?;
                bool result = bal.SaveExchangeRate(model);

                return Json(new ApiResponse
                {
                    Success = result,
                    Message = result ? "Exchange rate saved successfully." : "Failed to save exchange rate."
                });
            }
            catch (Exception ex)
            {
                return Json(new ApiResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Save(CurrencyRateType model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Invalid data." });

                var bal = new fintech.BAL.HomeBAL();
                model.Entity_ID = Convert.ToInt32(Session["EntityID"]);
                model.Login_ID = Session["LoginID"] as int?;

                string messageCode = bal.SaveRateType(model); // ✅ changed int to string

                if (messageCode == "I0001" || messageCode == "U0001")
                {
                    return Json(new
                    {
                        success = true,
                        message = model.RateTypeId == 0 ? "Rate Type saved successfully." : "Rate Type updated successfully."
                    });
                }
                else if (messageCode == "V0001")
                {
                    return Json(new { success = false, message = "Rate Type Code already exists." });
                }
                else if (messageCode == "V0002")
                {
                    return Json(new { success = false, message = "Rate Type not found." });
                }
                else if (messageCode == "E0100")
                {
                    return Json(new { success = false, message = "Cannot insert with inactive state." });
                }
                else if (messageCode == "E0101")
                {
                    return Json(new { success = false, message = "Cannot update an inactive record." });
                }
                else
                {
                    return Json(new { success = false, message = "An unexpected error occurred." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
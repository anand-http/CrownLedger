using ClosedXML.Excel;
using fintech.BAL;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace fintech.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesBAL _bal;

        public SalesController()
        {
            _bal = new SalesBAL();
        }

        public ActionResult SetUpCustomer()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Client_ID desc";
            var bal = new fintech.BAL.SalesBAL();
            var model = bal.GetCustomerList(entityId, 0, length, orderby, null, null);

            return View(model);
        }
        [HttpPost]
        public JsonResult GetClientList(int icolelngth)
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

            var customersBal = new fintech.BAL.SalesBAL();
            CustomerFullResult Customers = customersBal.GetCustomerList(entityId, start, length, Orderby, search, statusFilter);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (Customers.Customers.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(Customers.Customers.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(Customers.Customers.First().Records);
            }
            dataTableData.data = Customers.Customers;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateCustomerStatus(int clientId, string status)
        {
            var bal = new fintech.BAL.SalesBAL();
            bool updated = bal.UpdateCustomerStatus(clientId, status);

            if (updated)
                return Json(new { success = true, message = "Status updated successfully." });
            else
                return Json(new { success = false, message = "Failed to update status." });
        }

        [HttpPost]
        public JsonResult SaveSalesOrder(SalesOrderItemModel model)
        {
            int entityId = Convert.ToInt32(Session["EntityID"]);
            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = new SalesBAL().SaveSalesOrder(model, entityId, out messageCode);
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
        public ActionResult CreateCreditNotesHistoryPaymentAwaited()
        {
            return View("create-credit-notes-history(Payment-awaited)");
        }

        public ActionResult CreateCreditNotesHistory()
        {
            return View("create-credit-notes-history");
        }

        public ActionResult CreateNewEstimate()
        {
            return View("create-new-estimate");
        }

        public ActionResult NewCreditNotes()
        {
            return View("new-credit-notes");
        }

        public ActionResult NewCreditNotes04()
        {
            return View("new-credit-notes04");
        }

        public ActionResult NewRecurringSalesInvoice()
        {
            return View("new-recurring-sales-invoice");
        }

        public ActionResult NewSalesInvoice()
        {
            return View("new-sales-invoice");
        }

        public ActionResult NewSalesInvoice04()
        {
            return View("new-sales-invoice04");
        }

        public ActionResult NewSalesInvoiceHistory()
        {
            return View("new-sales-invoice-history");
        }

        public ActionResult NewSalesOrder()
        {
            return View("new-sales-order");
        }

        public ActionResult RecurringInvoiceHistory()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by InvoiceId desc";
            var bal = new fintech.BAL.SalesBAL();
            var recinv = bal.GetSalesRecInvHistory(null, entityId, 0, length, orderby, null, null, null);
            return View("recurring-invoice-history", recinv);
        }

        public JsonResult GetSalesRecInvHistory(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            //int? entityId = 10;
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

            var salesCrNoteBal = new fintech.BAL.SalesBAL();
            SalesRecInvHistoryFullResult result = salesCrNoteBal.GetSalesRecInvHistory(statusFilter, entityId, start, length, Orderby, search, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.SalesRecInvHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.SalesRecInvHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.SalesRecInvHistory.First().Records);
            }
            dataTableData.data = result.SalesRecInvHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaleOrder04()
        {
            return View("sale-order-04");
        }

        public ActionResult SaleOrderHistory04()
        {
            return View("sale-order-history04");
        }

        public ActionResult SalesCreditNotesHistory()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by CreditNote_Id desc";
            var bal = new fintech.BAL.SalesBAL();
            var creditnotes = bal.GetSalesCreditNoteHistory(null, entityId, 0, length, orderby, null, null, null);
            return View("sales-credit-notes-history", creditnotes);
        }

        public JsonResult GetSalesCreditNoteHistory(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            //int? entityId = 10;
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

            var salesCrNoteBal = new fintech.BAL.SalesBAL();
            SalesCreditNoteHistoryFullResult result = salesCrNoteBal.GetSalesCreditNoteHistory(statusFilter, entityId, start, length, Orderby, search, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.SalesCreditNoteHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.SalesCreditNoteHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.SalesCreditNoteHistory.First().Records);
            }
            dataTableData.data = result.SalesCreditNoteHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesEstimate04()
        {
            return View("sales-estimate-04");
        }

        public ActionResult SalesEstimatesHistory()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Sale_EstimateId desc";
            var bal = new fintech.BAL.SalesBAL();
            var estimates = bal.GetSalesEstimateHistory(null, entityId, 0, length, orderby, null, null, null);
            return View("sales-estimates-history", estimates);
            //return View("sales-estimate-history");
        }

        [HttpPost]
        public JsonResult GetSalesEstimateHistory(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            //int? entityId = 10;
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

            var salesOrderBal = new fintech.BAL.SalesBAL();
            SalesEstimateHistoryFullResult result = salesOrderBal.GetSalesEstimateHistory(statusFilter, entityId, start, length, Orderby, search, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.SalesEstimateHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.SalesEstimateHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.SalesEstimateHistory.First().Records);
            }
            dataTableData.data = result.SalesEstimateHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesInvoiceHistory()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by InvoiceId desc";
            var bal = new fintech.BAL.SalesBAL();
            var invoices = bal.GetSalesInvoiceHistory(null, entityId, 0, length, orderby, null, null, null);
            return View("sales-invoice-history", invoices);
        }

        [HttpPost]
        public JsonResult GetSalesInvoiceHistory(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            //int? entityId = 10;
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

            var salesBal = new fintech.BAL.SalesBAL();
            SalesInvoiceHistoryFullResult result = salesBal.GetSalesInvoiceHistory(statusFilter, entityId, start, length, Orderby, search, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.SalesInvoiceHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.SalesInvoiceHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.SalesInvoiceHistory.First().Records);
            }
            dataTableData.data = result.SalesInvoiceHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalesInvoiceHistory04PaymentAwaited()
        {
            return View("sales-invoice-history-04-Payment-awaited");
        }

        public ActionResult SalesInvoiceOrder04Paid()
        {
            return View("sales-invoice-order-04(paid)");
        }

        public ActionResult SalesInvoiceOrder04PaymentAwaited()
        {
            return View("sales-invoice-order-04(Payment-awaited)");
        }

        public ActionResult SalesOrderHistory()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Sale_OrderId desc";
            var bal = new fintech.BAL.SalesBAL();
            var orders = bal.GetSalesOrderHistory(entityId, 0, length, orderby, null, null, null, null);
            return View("sales-order-history", orders);
        }

        [HttpPost]
        public JsonResult GetSalesOrderHistory(int icolelngth)
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
            var startDate = Request.Form.GetValues("startDate")?.FirstOrDefault() ?? "";
            var endDate = Request.Form.GetValues("endDate")?.FirstOrDefault() ?? "";
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

            var salesOrderBal = new fintech.BAL.SalesBAL();
            SalesOrderHistoryFullResult result = salesOrderBal.GetSalesOrderHistory(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.SalesOrderHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.SalesOrderHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.SalesOrderHistory.First().Records);
            }
            dataTableData.data = result.SalesOrderHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCustomer()
        {
            return View("view-customer");
        }

        //[HttpPost]
        //public ActionResult CreateCustomer(GetCustomerList_Result model)
        //{
        [HttpPost]
        public ActionResult CreateCustomer(SaveCustomerList_Result model)
        {
            //if (ModelState.IsValid)
            //{
            var bal = new fintech.BAL.SalesBAL();
            var result = bal.UpsertCustomer(model);

            if (result != null)
            {
                int entityId = Convert.ToInt32(Session["EntityID"]);
                if (model.Contacts != null && model.Contacts.Count > 0)
                    bal.SaveContactDetails(model.Client_ID, entityId, model.Contacts);

                string message = (model.Client_ID == 0)
                    ? "Customer created successfully."
                    : "Customer updated successfully.";

                return Json(new
                {
                    success = true,
                    message,
                    Client_Id = result.Client_Id
                });
            }
            else
            {
                return Json(new { success = false, message = "Customer operation failed." });
            }
            //}

            //return Json(new { success = false, message = "Invalid data." });
        }


        [HttpPost]
        public JsonResult ImportCustomers(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return Json(new { status = false, message = "No file selected." });

            string extension = Path.GetExtension(file.FileName)?.ToLower();
            if (extension != ".csv" && extension != ".xlsx")
                return Json(new { status = false, message = "Invalid format. Only CSV and XLSX are allowed." });

            try
            {
                List<ImportCustomer> customers;
                List<Contact_Details> contacts;

                if (extension == ".csv")
                {
                    // single flat file — customer + contact columns on same row
                    ParseFlatCSV(file, out customers, out contacts);
                }
                else
                {
                    ParseExcel(file, out customers, out contacts);
                }

                if (customers == null || customers.Count == 0)
                    return Json(new { status = false, message = "File is empty or has no valid data." });

                int? entityId = Session["EntityID"] as int?;
                var bal = new fintech.BAL.SalesBAL();
                var result = bal.ImportCustomers(customers, contacts, entityId);

                return Json(new { status = result.Success, message = result.Message, inserted = result.InsertedCount });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error processing file: " + ex.Message });
            }
        }

        //private void ParseFlatCSV(HttpPostedFileBase file, out List<ImportCustomer> customers, out List<Contact_Details> contacts)
        //{
        //    customers = new List<ImportCustomer>();
        //    contacts = new List<Contact_Details>();

        //    // track which Client_IDs have already been added
        //    var seenClientIds = new HashSet<string>();

        //    using (var reader = new StreamReader(file.InputStream, Encoding.UTF8))
        //    {
        //        reader.ReadLine(); // skip header
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            if (string.IsNullOrWhiteSpace(line)) continue;

        //            var cols = line.Split(',');
        //            string rawId = cols.ElementAtOrDefault(0)?.Trim();

        //            // add customer only once per Client_ID
        //            if (!seenClientIds.Contains(rawId))
        //            {
        //                seenClientIds.Add(rawId);
        //                try { customers.Add(MapToCustomer(cols)); } catch { }
        //            }

        //            // always add contact (cols 51–61)
        //            try
        //            {
        //                int? clientId = TryParseNullableInt(rawId);
        //                var contact = MapToContact(cols);
        //                contacts.Add((contact));
        //            }
        //            catch { }
        //        }
        //    }
        //}
        private List<ImportCustomer> ParseCSV(HttpPostedFileBase file)
        {
            var records = new List<ImportCustomer>();
            using (var reader = new StreamReader(file.InputStream, Encoding.UTF8))
            {
                string headerLine = reader.ReadLine(); // skip header
                if (headerLine == null) return records;

                string line;
                int rowNum = 2;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var cols = line.Split(',');
                    try
                    {
                        records.Add(MapToCustomer(cols));
                    }
                    catch
                    {
                        // skip bad rows silently or log
                    }
                    rowNum++;
                }
            }
            return records;
        }

        private void ParseFlatCSV(HttpPostedFileBase file, out List<ImportCustomer> customers, out List<Contact_Details> contacts)
        {
            customers = new List<ImportCustomer>();
            contacts = new List<Contact_Details>();

            var seenClientIds = new HashSet<string>();

            using (var reader = new StreamReader(file.InputStream, Encoding.UTF8))
            {
                string line;
                bool readingCustomers = false;
                bool readingContacts = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Detect customer section header
                    if (line.StartsWith("CustomerName,Client_Code", StringComparison.OrdinalIgnoreCase))
                    {
                        readingCustomers = true;
                        readingContacts = false;
                        continue;
                    }

                    // Detect contact section header
                    if (line.StartsWith("Client_ID,Contact_ID", StringComparison.OrdinalIgnoreCase))
                    {
                        readingCustomers = false;
                        readingContacts = true;
                        continue;
                    }

                    var cols = line.Split(',');

                    // -----------------------------
                    // Customer rows
                    // -----------------------------
                    if (readingCustomers)
                    {
                        string rawId = cols.ElementAtOrDefault(1)?.Trim(); // Client_Code

                        if (!string.IsNullOrWhiteSpace(rawId) && !seenClientIds.Contains(rawId))
                        {
                            seenClientIds.Add(rawId);

                            try
                            {
                                customers.Add(MapToCustomer(cols));
                            }
                            catch
                            {
                                // optionally log customer parsing error
                            }
                        }
                    }

                    // -----------------------------
                    // Contact rows
                    // -----------------------------
                    else if (readingContacts)
                    {
                        try
                        {
                            var contact = MapToContact(cols);

                            // only add if at least contact id or name exists
                            if (contact != null &&
                                (contact.Contact_ID.HasValue || !string.IsNullOrWhiteSpace(contact.Contact_Name)))
                            {
                                contacts.Add(contact);
                            }
                        }
                        catch
                        {
                            // optionally log contact parsing error
                        }
                    }
                }
            }
        }


        private void ParseExcel(HttpPostedFileBase file, out List<ImportCustomer> customers, out List<Contact_Details> contacts)
        {
            customers = new List<ImportCustomer>();
            contacts = new List<Contact_Details>();

            using (var wb = new XLWorkbook(file.InputStream))
            {
                // ── Sheet 1: CustomerData ──
                var custSheet = wb.Worksheet("Customers");
                if (custSheet != null)
                {
                    foreach (var row in custSheet.RowsUsed().Skip(1))
                    {
                        try
                        {
                            var cols = Enumerable.Range(1, 50)
                                                 .Select(i => row.Cell(i).GetString())
                                                 .ToArray();
                            customers.Add(MapToCustomer(cols));
                        }
                        catch { }
                    }
                }

                // ── Sheet 2: Contacts ──
                var contSheet = wb.Worksheet("Contacts");
                if (contSheet != null)
                {
                    foreach (var row in contSheet.RowsUsed().Skip(1))
                    {
                        try
                        {
                            int? clientId = TryParseNullableInt(row.Cell(1).GetString());
                            var contact = new Contact_Details
                            {
                                Contact_ID = TryParseNullableInt(row.Cell(2).GetString()),
                                Contact_Source = row.Cell(3).GetString(),
                                Contact_SourceID = TryParseNullableInt(row.Cell(4).GetString()),
                                Contact_ContypID = TryParseNullableInt(row.Cell(5).GetString()),
                                Contact_Name = row.Cell(6).GetString(),
                                Contact_Email = row.Cell(7).GetString(),
                                Contact_Fax = row.Cell(8).GetString(),
                                Contact_Phone = row.Cell(9).GetString(),
                                Contact_Mobile = row.Cell(10).GetString(),
                                Designation = row.Cell(11).GetString(),
                                Contact_Type_desc = row.Cell(12).GetString()
                            };
                            contacts.Add((contact));
                        }
                        catch { }
                    }
                }
            }
        }

        private ImportCustomer MapToCustomer(string[] c)
        {
            return new ImportCustomer
            {
                CustomerName = c.ElementAtOrDefault(0),
                Client_Code = c.ElementAtOrDefault(1),
                Email = c.ElementAtOrDefault(2),
                Telephone = c.ElementAtOrDefault(3),
                AmountOutstanding = TryParseDecimal(c.ElementAtOrDefault(4)),
                UnusedCredit = TryParseDecimal(c.ElementAtOrDefault(5)),
                Website = c.ElementAtOrDefault(6),
                Client_Status = c.ElementAtOrDefault(7),
                Country = c.ElementAtOrDefault(8),  // Contry_Id
                State = c.ElementAtOrDefault(9),  // State_Id
                Department = c.ElementAtOrDefault(10),  // DepartMentId
                DefaultCurrency = c.ElementAtOrDefault(11),  // Currency_Id
                Address1 = c.ElementAtOrDefault(12),
                Address2 = c.ElementAtOrDefault(13),
                CorrespondenceAddress1 = c.ElementAtOrDefault(14),
                CorrespondenceAddress2 = c.ElementAtOrDefault(15),
                City = c.ElementAtOrDefault(16),  // City_Id
                City2 = c.ElementAtOrDefault(17),
                State2 = c.ElementAtOrDefault(18),
                Country2 = c.ElementAtOrDefault(19),
                ZipCode = c.ElementAtOrDefault(20),
                ZipCode2 = c.ElementAtOrDefault(21),
                BankCurrency = c.ElementAtOrDefault(22),
                AccountNumber = c.ElementAtOrDefault(23),
                Payee = c.ElementAtOrDefault(24),
                BankName = c.ElementAtOrDefault(25),
                IBAN_IFSC = c.ElementAtOrDefault(26),
                SWIFTCode = c.ElementAtOrDefault(27),
                SORTCode = c.ElementAtOrDefault(28),
                IsDefaultBank = TryParseBool(c.ElementAtOrDefault(29)),
                GST_VAT = c.ElementAtOrDefault(30),
                PAN = c.ElementAtOrDefault(31),
                TaxCode = c.ElementAtOrDefault(32),
                BillPercentage = TryParseDecimal(c.ElementAtOrDefault(33)),
                DueDateBasedOn = c.ElementAtOrDefault(34),
                CreditDays = TryParseNullableInt(c.ElementAtOrDefault(35)),
                Records = TryParseNullableInt(c.ElementAtOrDefault(36)),
                CreditLimits = TryParseDecimal(c.ElementAtOrDefault(37)),
                Status = c.ElementAtOrDefault(38)
            };
        }

        // ── Map contact columns (50–60) ──
        //private Contact_Details MapToContact(string[] c)
        //{
        //    return new Contact_Details
        //    {
        //        Contact_ID = TryParseNullableInt(c.ElementAtOrDefault(50)),
        //        Contact_Source = c.ElementAtOrDefault(51),
        //        Contact_SourceID = TryParseNullableInt(c.ElementAtOrDefault(52)),
        //        Contact_ContypID = TryParseNullableInt(c.ElementAtOrDefault(53)),
        //        Contact_Name = c.ElementAtOrDefault(54),
        //        Contact_Email = c.ElementAtOrDefault(55),
        //        Contact_Fax = c.ElementAtOrDefault(56),
        //        Contact_Phone = c.ElementAtOrDefault(57),
        //        Contact_Mobile = c.ElementAtOrDefault(58),
        //        Designation = c.ElementAtOrDefault(59),
        //        Contact_Type_desc = c.ElementAtOrDefault(60)
        //    };
        //}

        private Contact_Details MapToContact(string[] c)
        {
            return new Contact_Details
            {
                Contact_ID = TryParseNullableInt(c.ElementAtOrDefault(1)),
                Contact_Source = c.ElementAtOrDefault(2),
                Contact_SourceID = TryParseNullableInt(c.ElementAtOrDefault(3)),
                Contact_ContypID = TryParseNullableInt(c.ElementAtOrDefault(4)),
                Contact_Name = c.ElementAtOrDefault(5),
                Contact_Email = c.ElementAtOrDefault(6),
                Contact_Fax = c.ElementAtOrDefault(7),
                Contact_Phone = c.ElementAtOrDefault(8),
                Contact_Mobile = c.ElementAtOrDefault(9),
                Designation = c.ElementAtOrDefault(10),
                Contact_Type_desc = c.ElementAtOrDefault(11)
            };
        }

        // ── Helpers ──
        private int TryParseInt(string val) => int.TryParse(val, out int r) ? r : 0;

        private int? TryParseNullableInt(string val) => int.TryParse(val, out int r) ? r : (int?)null;

        private decimal? TryParseDecimal(string val) => decimal.TryParse(val, out decimal r) ? r : (decimal?)null;

        private long? TryParselong(string val) => long.TryParse(val, out long r) ? r : (long?)null;
        private bool? TryParseBool(string val) => bool.TryParse(val, out bool r) ? r : (bool?)null;


        [HttpPost]
        public ActionResult ExportCustomers(string exportType)
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Client_Id desc";
            var bal = new fintech.BAL.SalesBAL();
            var result = bal.GetCustomerList(entityId, 0, length, orderby, null, null);

            return exportType == "CSV" ? ExportCustomersCSV(result) : ExportCustomersExcel(result);
        }
        public ActionResult ExportCustomersCSV(CustomerFullResult result)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CustomerName,Client_Code,Email,Telephone," +
                          "AmountOutstanding,UnusedCredit,Website,Client_Status," +
                          "Country,State,Department," +
                          "DefaultCurrency,Address1,Address2," +
                          "CorrespondenceAddress1,CorrespondenceAddress2," +
                          "City,City2,State2,Country2," +
                          "ZipCode,ZipCode2,BankCurrency,AccountNumber,Payee," +
                          "BankName,IBAN_IFSC,SWIFTCode,SORTCode,IsDefaultBank," +
                          "GST_VAT,PAN,TaxCode,BillPercentage,DueDateBasedOn," +
                          "CreditDays,Records,CreditLimits,Status," +
                          "Contact_ID,Contact_Source,Contact_SourceID,Contact_ContypID," +
                          "Contact_Name,Contact_Email,Contact_Fax,Contact_Phone," +
                          "Contact_Mobile,Designation,Contact_Type_desc");

            foreach (var c in result.Customers)
            {
                // get contacts for this customer via Contact_SourceID = Client_ID
                var customerContacts = result.Contacts
                    .Where(x => x.Contact_SourceID == (int?)c.Client_ID)
                    .ToList();

                if (!customerContacts.Any())
                {
                    // customer with no contacts — write one row with empty contact columns
                    sb.AppendLine(BuildCustomerCSVRow(c) + ",,,,,,,,,,,");
                }
                else
                {
                    // repeat customer row for each contact
                    foreach (var contact in customerContacts)
                    {
                        sb.AppendLine(BuildCustomerCSVRow(c) + "," + string.Join(",",
                            contact.Contact_ID,
                            Escape(contact.Contact_Source),
                            contact.Contact_SourceID,
                            contact.Contact_ContypID,
                            Escape(contact.Contact_Name),
                            Escape(contact.Contact_Email),
                            Escape(contact.Contact_Fax),
                            Escape(contact.Contact_Phone),
                            Escape(contact.Contact_Mobile),
                            Escape(contact.Designation),
                            Escape(contact.Contact_Type_desc)
                        ));
                    }
                }
            }

            Response.AddHeader("Content-Disposition",
                $"attachment; filename=\"Customers_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv\"");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv");
        }

        private string BuildCustomerCSVRow(GetCustomerList_Result c)
        {
            return string.Join(",",
                Escape(c.CustomerName), Escape(c.Client_Code),
                Escape(c.Email), Escape(c.Telephone), c.AmountOutstanding, c.UnusedCredit,
                Escape(c.Website), Escape(c.Client_Status),
                Escape(c.Country), Escape(c.State),
                Escape(c.Department), Escape(c.DefaultCurrency),
                Escape(c.Address1), Escape(c.Address2),
                Escape(c.CorrespondenceAddress1), Escape(c.CorrespondenceAddress2),
                Escape(c.City), Escape(c.City2),
                Escape(c.State2), Escape(c.Country2),
                Escape(c.ZipCode), Escape(c.ZipCode2),
                Escape(c.BankCurrency), Escape(c.AccountNumber),
                Escape(c.Payee), Escape(c.BankName), Escape(c.IBAN_IFSC),
                Escape(c.SWIFTCode), Escape(c.SORTCode), c.IsDefaultBank,
                Escape(c.GST_VAT), Escape(c.PAN), Escape(c.TaxCode),
                c.BillPercentage, Escape(c.DueDateBasedOn),
                c.CreditDays, c.Records, c.CreditLimits, Escape(c.Status)
            );
        }


        public ActionResult ExportCustomersExcel(CustomerFullResult result)
        {
            using (var wb = new XLWorkbook())
            {
                // ── Sheet 1: Customers ──
                var ws = wb.Worksheets.Add("CustomerData");
                var custHeaders = new[] {
                "CustomerName", "Client_Code", "Email", "Telephone",
                "AmountOutstanding", "UnusedCredit", "Website", "Client_Status",
                "Country", "State", "Department", "DefaultCurrency",
                "Address1", "Address2", "CorrespondenceAddress1", "CorrespondenceAddress2",
                "City", "City2", "State2", "Country2",
                "ZipCode", "ZipCode2",
                "BankCurrency", "AccountNumber", "Payee", "BankName",
                "IBAN_IFSC", "SWIFTCode", "SORTCode", "IsDefaultBank",
                "GST_VAT", "PAN", "TaxCode",
                "BillPercentage", "DueDateBasedOn", "CreditDays", "Records", "CreditLimits", "Status"
            };
                StyleHeader(ws, custHeaders);

                int row = 2;
                foreach (var c in result.Customers)
                {
                    ws.Cell(row, 1).Value = c.CustomerName;
                    ws.Cell(row, 2).Value = c.Client_Code;
                    ws.Cell(row, 3).Value = c.Email;
                    ws.Cell(row, 4).Value = c.Telephone;
                    ws.Cell(row, 5).Value = c.AmountOutstanding?.ToString();
                    ws.Cell(row, 6).Value = c.UnusedCredit?.ToString();
                    ws.Cell(row, 7).Value = c.Website;
                    ws.Cell(row, 8).Value = c.Client_Status;
                    ws.Cell(row, 9).Value = c.Country;
                    ws.Cell(row, 10).Value = c.State;
                    ws.Cell(row, 11).Value = c.Department;
                    ws.Cell(row, 12).Value = c.DefaultCurrency;
                    ws.Cell(row, 13).Value = c.Address1;
                    ws.Cell(row, 14).Value = c.Address2;
                    ws.Cell(row, 15).Value = c.CorrespondenceAddress1;
                    ws.Cell(row, 16).Value = c.CorrespondenceAddress2;
                    ws.Cell(row, 17).Value = c.City;
                    ws.Cell(row, 18).Value = c.City2;
                    ws.Cell(row, 19).Value = c.State2;
                    ws.Cell(row, 20).Value = c.Country2;
                    ws.Cell(row, 21).Value = c.ZipCode;
                    ws.Cell(row, 22).Value = c.ZipCode2;
                    ws.Cell(row, 23).Value = c.BankCurrency;
                    ws.Cell(row, 24).Value = c.AccountNumber;
                    ws.Cell(row, 25).Value = c.Payee;
                    ws.Cell(row, 26).Value = c.BankName;
                    ws.Cell(row, 27).Value = c.IBAN_IFSC;
                    ws.Cell(row, 28).Value = c.SWIFTCode;
                    ws.Cell(row, 29).Value = c.SORTCode;
                    ws.Cell(row, 30).Value = c.IsDefaultBank?.ToString();
                    ws.Cell(row, 31).Value = c.GST_VAT;
                    ws.Cell(row, 32).Value = c.PAN;
                    ws.Cell(row, 33).Value = c.TaxCode;
                    ws.Cell(row, 34).Value = c.BillPercentage?.ToString();
                    ws.Cell(row, 35).Value = c.DueDateBasedOn;
                    ws.Cell(row, 36).Value = c.CreditDays?.ToString();
                    ws.Cell(row, 37).Value = c.Records?.ToString();
                    ws.Cell(row, 38).Value = c.CreditLimits?.ToString();
                    ws.Cell(row, 39).Value = c.Status;
                    row++;
                }
                ws.Columns().AdjustToContents();
                ws.SheetView.FreezeRows(1);

                // ── Sheet 2: Contacts ──
                var ws2 = wb.Worksheets.Add("Contacts");
                var contHeaders = new[] {
                "Contact_ID","Contact_Source","Contact_SourceID",
                "Contact_ContypID","Contact_Name","Contact_Email","Contact_Fax",
                "Contact_Phone","Contact_Mobile","Designation","Contact_Type_desc"
            };
                StyleHeader(ws2, contHeaders);

                int crow = 2;
                foreach (var c in result.Contacts)
                {
                    ws2.Cell(crow, 1).Value = c.Contact_ID?.ToString();
                    ws2.Cell(crow, 2).Value = c.Contact_Source;
                    ws2.Cell(crow, 3).Value = c.Contact_SourceID?.ToString();
                    ws2.Cell(crow, 4).Value = c.Contact_ContypID?.ToString();
                    ws2.Cell(crow, 5).Value = c.Contact_Name;
                    ws2.Cell(crow, 6).Value = c.Contact_Email;
                    ws2.Cell(crow, 7).Value = c.Contact_Fax;
                    ws2.Cell(crow, 8).Value = c.Contact_Phone;
                    ws2.Cell(crow, 9).Value = c.Contact_Mobile;
                    ws2.Cell(crow, 10).Value = c.Designation;
                    ws2.Cell(crow, 11).Value = c.Contact_Type_desc;
                    crow++;
                }
                ws2.Columns().AdjustToContents();
                ws2.SheetView.FreezeRows(1);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Response.AddHeader("Content-Disposition",
                        $"attachment; filename=\"Customers_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx\"");
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }
        }

        private string Escape(string val) => string.IsNullOrEmpty(val) ? "" : val.Contains(",") ? $"\"{val}\"" : val;

        private void StyleHeader(IXLWorksheet ws, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = ws.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }
        }
        public ActionResult DownloadSampleCSV()
        {
            var filePath = Server.MapPath("~/Content/SampleFiles/Import_Customer_CSV.csv");

            if (!System.IO.File.Exists(filePath))
                return HttpNotFound("Sample CSV file not found.");

            return File(filePath, "text/csv", "CustomerSample.csv");
        }

        public ActionResult DownloadSampleExcel()
        {
            var filePath = Server.MapPath("~/Content/SampleFiles/Import_Customer_Template.xlsx");

            if (!System.IO.File.Exists(filePath))
                return HttpNotFound("Sample Excel file not found.");

            return File(
                filePath,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CustomerSample.xlsx"
            );
        }

        [HttpPost]
        public JsonResult SaveEstimate(SaleEstimateModel request)
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int? loginId = Session["LoginID"] as int?;
                request.Entity_ID = entityId;
                request.Login_ID = loginId;
                if (request == null || request.Items == null || request.Items.Count == 0)
                    return Json(new { status = false, message = "Invalid estimate data." });

                var bal = new fintech.BAL.SalesBAL();
                var result = bal.SaveEstimate(request);

                return Json(new
                {
                    status = result.Status,
                    message = result.Message

                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }

        public JsonResult SaveOrder(SaleEstimateModel request)
        {
            try
            {
                if (request == null || request.Items == null || request.Items.Count == 0)
                    return Json(new { status = false, message = "Invalid estimate data." });

                var bal = new fintech.BAL.SalesBAL();
                var result = bal.SaveOrder(request);

                return Json(new
                {
                    status = result,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }

        public JsonResult SaveInvoice(SaleInvoice request)
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int? loginId = Session["LoginID"] as int?;
                request.Entity_ID = entityId;
                request.Login_ID = loginId;
                if (request == null || request.Items == null || request.Items.Count == 0)
                    return Json(new { status = false, message = "Invalid estimate data." });

                var bal = new fintech.BAL.SalesBAL();
                var result = bal.SaveInvoice(request);

                return Json(new
                {
                    status = result,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }
        public JsonResult SaveRecurringInv(SaleRecInv request)
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int? loginId = Convert.ToInt32(Session["LoginID"]);
                request.Entity_ID = entityId;
                request.Login_ID = loginId;
                if (request == null || request.Items == null || request.Items.Count == 0)
                    return Json(new { status = false, message = "Invalid estimate data." });

                var bal = new fintech.BAL.SalesBAL();
                var result = bal.SaveRecurringInv(request);

                return Json(new
                {
                    status = result,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }


        public JsonResult CreditNote(SaleCreditNote request)
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                int? loginId = Session["LoginID"] as int?;
                request.Entity_ID = entityId;
                request.Login_ID = loginId;
                if (request == null || request.Items == null || request.Items.Count == 0)
                    return Json(new { status = false, message = "Invalid estimate data." });

                var bal = new fintech.BAL.SalesBAL();
                var result = bal.SaveCreditNote(request);

                return Json(new
                {
                    status = result,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error: " + ex.Message });
            }
        }
    }
}

using ClosedXML.Excel;
using fintech.BAL;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace fintech.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly PurchasesBAL _bal;

        public class FileLogger
        {
            public static void Log(string message)
            {
                string filePath = @"C:\Logs\Application.log";

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                // Write log
                System.IO.File.AppendAllText(
                    filePath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}"
                );
            }
        }
        public PurchasesController()
        {
            _bal = new PurchasesBAL();
        }
        // GET: Purchases

        public ActionResult set_up_vendor()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Partner_ID desc";
            var bal = new fintech.BAL.SalesBAL();
            var model = _bal.GetVendorList(entityId, 0, length, orderby, null, false, null);

            return View(model);
        }
        public JsonResult GetvendorList(int icolelngth)
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

            var vendorbal = new fintech.BAL.PurchasesBAL();
            VendorFullResult Vendors = vendorbal.GetVendorList(entityId, start, length, Orderby, search, false, statusFilter);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (Vendors.Vendors.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(Vendors.Vendors.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(Vendors.Vendors.First().Records);
            }
            dataTableData.data = Vendors.Vendors;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateVendorStatus(int partnerid, string status)
        {
            bool updated = _bal.UpdateVendorStatus(partnerid, status);

            if (updated)
                return Json(new { success = true, message = "Status updated successfully." });
            else
                return Json(new { success = false, message = "Failed to update status." });
        }

        [HttpPost]
        public ActionResult CreateVendor(SaveVendorList_Result model)
        {
            //if (ModelState.IsValid)
            //{
            var result = _bal.UpsertVendor(model);

            if (result != null)
            {
                //int entityId = Convert.ToInt32(Session["EntityID"]);
                int entityId = 10;
                if (model.Contacts != null && model.Contacts.Count > 0)
                    _bal.SaveContactDetails(model.Partner_Id, entityId, model.Contacts);

                string message = (model.Partner_Id == 0)
                    ? "Vendor created successfully."
                    : "Vendor updated successfully.";

                return Json(new
                {
                    success = true,
                    message,
                    Client_Id = result.Partner_Id
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
        public JsonResult ImportVendors(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return Json(new { status = false, message = "No file selected." });

            string extension = Path.GetExtension(file.FileName)?.ToLower();
            if (extension != ".csv" && extension != ".xlsx")
                return Json(new { status = false, message = "Invalid format. Only CSV and XLSX are allowed." });

            try
            {
                List<ImportVendor> vendors;
                List<Contact_Details> contacts;

                if (extension == ".csv")
                {
                    // single flat file — customer + contact columns on same row
                    ParseFlatCSV(file, out vendors, out contacts);
                }
                else
                {
                    ParseExcel(file, out vendors, out contacts);
                }

                if (vendors == null || vendors.Count == 0)
                    return Json(new { status = false, message = "File is empty or has no valid data." });

                int? entityId = Session["EntityID"] as int?;
                var result = _bal.ImportVendor(vendors, contacts, entityId);

                return Json(new { status = result.Success});
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "Error processing file: " + ex.Message });
            }
        }

        private void ParseFlatCSV(HttpPostedFileBase file, out List<ImportVendor> vendors, out List<Contact_Details> contacts)
        {
            vendors = new List<ImportVendor>();
            contacts = new List<Contact_Details>();

            // track which Client_IDs have already been added
            var seenClientIds = new HashSet<string>();

            using (var reader = new StreamReader(file.InputStream, Encoding.UTF8))
            {
                reader.ReadLine(); // skip header
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var cols = line.Split(',');
                    string rawId = cols.ElementAtOrDefault(0)?.Trim();

                    // add customer only once per Client_ID
                    if (!seenClientIds.Contains(rawId))
                    {
                        seenClientIds.Add(rawId);
                        try { vendors.Add(MapToVendor(cols)); } catch { }
                    }

                    // always add contact (cols 51–61)
                    try
                    {
                        int? clientId = TryParseNullableInt(rawId);
                        var contact = MapToContact(cols);
                        contacts.Add((contact));
                    }
                    catch { }
                }
            }
        }

        private void ParseExcel(HttpPostedFileBase file, out List<ImportVendor> vendors, out List<Contact_Details> contacts)
        {
            vendors = new List<ImportVendor>();
            contacts = new List<Contact_Details>();

            using (var wb = new XLWorkbook(file.InputStream))
            {
                // ── Sheet 1: CustomerData ──
                var custSheet = wb.Worksheet("Vendors");
                if (custSheet != null)
                {
                    foreach (var row in custSheet.RowsUsed().Skip(1))
                    {
                        try
                        {
                            var cols = Enumerable.Range(1, 50)
                                                 .Select(i => row.Cell(i).GetString())
                                                 .ToArray();
                            vendors.Add(MapToVendor(cols));
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
                                Contact_Name = row.Cell(6).GetString(),
                                Contact_Email = row.Cell(7).GetString(),
                                Contact_Fax = row.Cell(8).GetString(),
                                Contact_Phone = row.Cell(9).GetString(),
                                Contact_Mobile = row.Cell(10).GetString(),
                                Designation = row.Cell(11).GetString(),
                                Contact_Type = row.Cell(12).GetString()
                            };
                            contacts.Add((contact));
                        }
                        catch { }
                    }
                }
            }
        }

        private ImportVendor MapToVendor(string[] c)
        {
            return new ImportVendor
            {
                Name = c.ElementAtOrDefault(0),
                Partner_Code = c.ElementAtOrDefault(1),
                Email = c.ElementAtOrDefault(2),
                Telephone = c.ElementAtOrDefault(3),
                AmountOutstanding = TryParseDecimal(c.ElementAtOrDefault(4)),
                UnusedCredit = TryParseDecimal(c.ElementAtOrDefault(5)),
                Website = c.ElementAtOrDefault(6),
                PartnerStatus = c.ElementAtOrDefault(7),
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
        private Contact_Details MapToContact(string[] c)
        {
            return new Contact_Details
            {
                Contact_ID = TryParseNullableInt(c.ElementAtOrDefault(50)),
                Contact_Source = c.ElementAtOrDefault(51),
                Contact_SourceID = TryParseNullableInt(c.ElementAtOrDefault(52)),
                Contact_ContypID = TryParseNullableInt(c.ElementAtOrDefault(53)),
                Contact_Name = c.ElementAtOrDefault(54),
                Contact_Email = c.ElementAtOrDefault(55),
                Contact_Fax = c.ElementAtOrDefault(56),
                Contact_Phone = c.ElementAtOrDefault(57),
                Contact_Mobile = c.ElementAtOrDefault(58),
                Designation = c.ElementAtOrDefault(59),
                Contact_Type = c.ElementAtOrDefault(60)
            };
        }

        // ── Helpers ──
        private int TryParseInt(string val) => int.TryParse(val, out int r) ? r : 0;

        private int? TryParseNullableInt(string val) => int.TryParse(val, out int r) ? r : (int?)null;

        private decimal? TryParseDecimal(string val) => decimal.TryParse(val, out decimal r) ? r : (decimal?)null;

        private long? TryParselong(string val) => long.TryParse(val, out long r) ? r : (long?)null;
        private bool? TryParseBool(string val) => bool.TryParse(val, out bool r) ? r : (bool?)null;

        //[HttpGet]
        //public ActionResult Export(string moduleType, string format, int entityId)
        //{
        //    try
        //    {
        //        byte[] xlsxBytes = null;

        //        switch (moduleType)
        //        {
        //            case "Estimate":
        //                var estimateResult = _bal.GetSalesEstimateHistory(null, entityId, 0, 10, null, null, null, null);
        //                xlsxBytes = ConvertToExcel(estimateResult.SalesEstimateHistory);
        //                break;

        //            case "Invoice":
        //                var invoiceResult = _bal.GetSalesInvoiceHistory(null, entityId, 0, 10, null, null, null, null);
        //                xlsxBytes = ConvertToExcel(invoiceResult.SalesInvoiceHistory);
        //                break;

        //            case "SalesOrder":
        //                var orderResult = _bal.GetSalesOrderHistory(entityId, 0, 10, null, null, null, null, null);
        //                xlsxBytes = ConvertToExcel(orderResult.SalesOrderHistory);
        //                break;

        //            case "CreditNote":
        //                var creditResult = _bal.GetSalesCreditNoteHistory(null, entityId, 0, 10, null, null, null, null);
        //                xlsxBytes = ConvertToExcel(creditResult.SalesCreditNoteHistory);
        //                break;

        //            case "RecInvoice":
        //                var recurringResult = _bal.GetSalesRecInvHistory(null, entityId, 0, 10, null, null, null, null);
        //                xlsxBytes = ConvertToExcel(recurringResult.SalesRecInvHistory);
        //                break;

        //            default:
        //                return new HttpStatusCodeResult(400, "Unknown moduleType");
        //        }

        //        return File(xlsxBytes,
        //            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //            $"{moduleType}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpStatusCodeResult(500, ex.Message);
        //    }
        //}


        //private byte[] ConvertToExcel<T>(List<T> data)
        //{
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var sheet = workbook.Worksheets.Add("Export");
        //        var props = typeof(T).GetProperties();

        //        // ✅ Skip internal/pagination props not needed in export
        //        var excludedProps = new[] { "Records", "TotalItems" };
        //        var exportProps = props.Where(p => !excludedProps.Contains(p.Name)).ToArray();

        //        // Headers
        //        for (int i = 0; i < exportProps.Length; i++)
        //        {
        //            sheet.Cell(1, i + 1).Value = exportProps[i].Name;
        //            sheet.Cell(1, i + 1).Style.Font.Bold = true;
        //            sheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        //        }

        //        // Rows
        //        for (int row = 0; row < data.Count; row++)
        //        {
        //            for (int col = 0; col < exportProps.Length; col++)
        //            {
        //                var value = exportProps[col].GetValue(data[row]);
        //                sheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
        //            }
        //        }

        //        sheet.Columns().AdjustToContents();

        //        using (var ms = new MemoryStream())
        //        {
        //            workbook.SaveAs(ms);
        //            return ms.ToArray();
        //        }
        //    }
        //}
        [HttpPost]
        public ActionResult ExportVendors(string exportType)
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Partner_ID desc";
            var result = _bal.GetVendorList(entityId, 0, length, orderby, null, false, null);

            return exportType == "CSV" ? ExportVendorssCSV(result) : ExportVendorsExcel(result);
        }
        public ActionResult ExportVendorssCSV(VendorFullResult result)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Name,Partner_Code,Email,Telephone," +
                          "AmountOwed,UnusedCredit,Website,Partner_Status," +
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

            foreach (var c in result.Vendors)
            {
                // get contacts for this customer via Contact_SourceID = Client_ID
                var customerContacts = result.Contacts
                    .Where(x => x.Contact_SourceID == (int?)c.Partner_Id)
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
                            Escape(contact.Contact_Type)
                        ));
                    }
                }
            }

            Response.AddHeader("Content-Disposition",
                $"attachment; filename=\"Vendors_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv\"");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv");
        }

        private string BuildCustomerCSVRow(GetVendorList_Result c)
        {
            return string.Join(",",
                Escape(c.Name), Escape(c.Partner_Code),
                Escape(c.Email), Escape(c.Phone), c.AmountOwed, c.UnusedCredit,
                Escape(c.Website), Escape(c.PartnerStatus),
                Escape(c.Country), Escape(c.State),
                Escape(c.Department), Escape(c.DefaultCurrency),
                Escape(c.PAddress1), Escape(c.PAddress2),
                Escape(c.CAddress1), Escape(c.CAddress2),
                Escape(c.PCity), Escape(c.CCity),
                Escape(c.State), Escape(c.CCountry),
                Escape(c.ZipCode), Escape(c.ZipCode2),
                Escape(c.AccountNumber),
                Escape(c.Payee), Escape(c.BankName), Escape(c.IBAN_IFSC),
                Escape(c.SWIFTCode), Escape(c.SORTCode), c.IsDefaultBank,
                Escape(c.GST_VAT), Escape(c.PAN), Escape(c.TaxCode),
                c.BillPercentage, Escape(c.DueDateBasedOn),
                c.CreditDays, c.Records, c.CreditLimits, Escape(c.Status)
            );
        }


        public ActionResult ExportVendorsExcel(VendorFullResult result)
        {
            using (var wb = new XLWorkbook())
            {
                // ── Sheet 1: Customers ──
                var ws = wb.Worksheets.Add("VendorData");
                var custHeaders = new[] {
                "Name", "Partner_Code", "Email", "Telephone",
                "AmountOwed", "UnusedCredit", "Website", "Client_Status",
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
                foreach (var c in result.Vendors)
                {
                    ws.Cell(row, 1).Value = c.Name;
                    ws.Cell(row, 2).Value = c.Partner_Code;
                    ws.Cell(row, 3).Value = c.Email;
                    ws.Cell(row, 4).Value = c.Phone;
                    ws.Cell(row, 5).Value = c.AmountOwed?.ToString();
                    ws.Cell(row, 6).Value = c.UnusedCredit?.ToString();
                    ws.Cell(row, 7).Value = c.Website;
                    ws.Cell(row, 8).Value = c.PartnerStatus;
                    ws.Cell(row, 9).Value = c.Country;
                    ws.Cell(row, 10).Value = c.State;
                    ws.Cell(row, 11).Value = c.Department;
                    ws.Cell(row, 12).Value = c.DefaultCurrency;
                    ws.Cell(row, 13).Value = c.PAddress1;
                    ws.Cell(row, 14).Value = c.PAddress2;
                    ws.Cell(row, 15).Value = c.CAddress1;
                    ws.Cell(row, 16).Value = c.CAddress2;
                    ws.Cell(row, 17).Value = c.PCity;
                    ws.Cell(row, 18).Value = c.CCity;
                    ws.Cell(row, 19).Value = c.State;
                    ws.Cell(row, 20).Value = c.CCountry;
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
                "Contact_Phone","Contact_Mobile","Designation","Contact_Type"
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
                    ws2.Cell(crow, 11).Value = c.Contact_Type;
                    crow++;
                }
                ws2.Columns().AdjustToContents();
                ws2.SheetView.FreezeRows(1);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Response.AddHeader("Content-Disposition",
                        $"attachment; filename=\"Vendors_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx\"");
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
        public ActionResult NewRecurringPurchaseBillHistory()
        {
            return View("new-recurring-purchase-bill-history");
        }

        public ActionResult PurchaseBillPaid()
        {
            return View("purchase-bill(paid)");
        }

        public ActionResult PurchaseBillPaymentAwaited()
        {
            return View("purchase-bill(Payment-awaited)");
        }

        public ActionResult PurchaseBill01Paid()
        {
            return View("purchase-bill-01(paid)");
        }

        public ActionResult PurchaseBill01PaymentAwaited()
        {
            return View("purchase-bill-01(Payment-awaited)");
        }

        public ActionResult PurchaseCreditNotesHistory()
        {
            return View("purchase-credit-notes-history");
        }

        [HttpPost]
        public JsonResult GetPurchasesCreditNotesHistory(int icolelngth)
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
            var search = Request.Form.GetValues("search[value]").FirstOrDefault() ?? "";

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

            var purchasesCreditNotesBal = new fintech.BAL.PurchasesBAL();
            PurchasesCreditNotesHistoryFullResult result = purchasesCreditNotesBal.GetPurchasesCreditNotesHistory(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.PurchasesCreditNotesHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.PurchasesCreditNotesHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.PurchasesCreditNotesHistory.First().Records);
            }
            dataTableData.data = result.PurchasesCreditNotesHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendorCreditNote4PaymentAwaited()
        {
            return View("vendor-credit-note-4(Payment-awaited)");
        }

        public ActionResult CreateNewPurchaseCreditNotes()
        {
            return View("create-new-purchase-credit-notes");
        }

        public ActionResult PurchaseOrder04()
        {
            return View("purchase-order-04");
        }

        public ActionResult PurchaseOrderHistory()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "";
            var bal = new fintech.BAL.PurchasesBAL();
            var model = bal.GetPurchasesOrderHistory(entityId, 0, length, orderby, null, null, null, null);
            return View("purchase-order-history", model);
        }
        [HttpPost]
        public JsonResult GetPurchasesOrderHistory(int icolelngth)
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
            var statusAction = Request.Form.GetValues("statusFilter")?.FirstOrDefault() ?? "";
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

            var purchaseOrderBal = new fintech.BAL.PurchasesBAL();
            PurchasesOrderHistoryFullResult result = purchaseOrderBal.GetPurchasesOrderHistory(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.PurchasesOrderHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.PurchasesOrderHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.PurchasesOrderHistory.First().Records);
            }
            dataTableData.data = result.PurchasesOrderHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewPurchaseOrder()
        {
            return View("create-new-purchase-order");
        }

        public ActionResult RecurringBillsHistory()
        {
            return View("recurring-bills-history");
        }

        public JsonResult GetRecBillHistory(int icolelngth)
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

            var purchaseOrderBal = new fintech.BAL.PurchasesBAL();
            PurchasesRecBillHistoryFullResult result = purchaseOrderBal.GetRecBillHistory(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.PurchasesRecBillHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.PurchasesRecBillHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.PurchasesRecBillHistory.First().Records);
            }
            dataTableData.data = result.PurchasesRecBillHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewRecurringPurchaseBill()
        {
            return View("create-new-recurring-purchase-bill");
        }


        public ActionResult ViewVendor()
        {
            return View("view-vendor");
        }

        public ActionResult BillHistory()
        {
            try
            {
                FileLogger.Log("REACHED");
                return View("bill-history");
            }
            catch (Exception ex)
            {
                FileLogger.Log($"ERROR: {ex.Message}");
                FileLogger.Log($"STACKTRACE: {ex.StackTrace}");

                // Also log inner exception if present
                if (ex.InnerException != null)
                    FileLogger.Log($"INNER EXCEPTION: {ex.InnerException.Message}");

                return View("Error");
            }
        }

        public JsonResult GetBillHistory(int icolelngth)
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
            var statusAction = Request.Form.GetValues("statusFilter")?.FirstOrDefault() ?? "";
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

            var purchaseOrderBal = new fintech.BAL.PurchasesBAL();
            PurchasesBillHistoryFullResult result = purchaseOrderBal.GetBillHistory(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.PurchasesBillHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.PurchasesBillHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.PurchasesBillHistory.First().Records);
            }
            dataTableData.data = result.PurchasesBillHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateNewPurchaseBill()
        {
            return View("create-new-purchase-bill");
        }

        public ActionResult CreateNewPurchaseBillHistory()
        {
            return View("create-new-purchase-bill-history");
        }

        public ActionResult DownloadSampleCSV()
        {
            var filePath = Server.MapPath("~/Content/SampleFiles/Import_Vendor_CSV.csv");

            if (!System.IO.File.Exists(filePath))
                return HttpNotFound("Sample CSV file not found.");

            return File(filePath, "text/csv", "VendorSample.csv");
        }

        public ActionResult DownloadSampleExcel()
        {
            var filePath = Server.MapPath("~/Content/SampleFiles/Import_Vendor_Template.xlsx");

            if (!System.IO.File.Exists(filePath))
                return HttpNotFound("Sample Excel file not found.");

            return File(
                filePath,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "VendorSample.xlsx"
            );
        }

        public ActionResult ExpenseHistory()
        {
            return View("expense-history");
        }

        // Get the Expense History
        public JsonResult GetExpenseHistory(int icolelngth)
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

            var purchaseOrderBal = new fintech.BAL.PurchasesBAL();
            PurchasesExpenseHistoryFullResult result = purchaseOrderBal.GetExpenseHistory(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.PurchasesExpenseHistory.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.PurchasesExpenseHistory.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.PurchasesExpenseHistory.First().Records);
            }
            dataTableData.data = result.PurchasesExpenseHistory;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }


        // Saving Purchase Order

        [HttpPost]
        public JsonResult SavePurchaseOrder(PurchaseOrderCreate model)
        {
            int entityId = Convert.ToInt32(Session["EntityID"]);
            int? loginId = Session["LoginID"] as int?;

            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = new PurchasesBAL().SavePurchaseOrder(model, entityId, loginId, out messageCode);
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


        // Saving Purchase Bills
        [HttpPost]
        public JsonResult SavePurchaseBills(PurchaseBillsCreate model)
        {
            int entityId = Convert.ToInt32(Session["EntityID"]);
            int? loginId = Session["LoginID"] as int?;

            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = new PurchasesBAL().SavePurchaseBills(model, entityId, loginId, out messageCode);
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


        // Saving Vendor credit
        [HttpPost]
        public JsonResult SavePurchaseCreditNote(PurchaseCreditNoteCreate model)
        {
            int entityId = Convert.ToInt32(Session["EntityID"]);
            int? loginId = Session["LoginID"] as int?;

            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = new PurchasesBAL().SavePurchaseCreditNote(model, entityId, loginId, out messageCode);
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


        // Saving Recurring Bills
        [HttpPost]
        public JsonResult SavePurchaseRecurringBills(PurchaseRecurringBillsCreate model)
        {
            int entityId = Convert.ToInt32(Session["EntityID"]);
            int? loginId = Session["LoginID"] as int?;

            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = new PurchasesBAL().SavePurchaseRecurringBills(model, entityId, loginId, out messageCode);
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



        // Save Expense

        [HttpPost]
        public JsonResult SaveExpense(ExpenseCreate expenseModel)
        {
            try
            {
                int entityId = Convert.ToInt32(Session["EntityID"] ?? 0);
                int? loginId = Session["LoginID"] as int?;
                string recordType = expenseModel.RecordType ?? "";
                string messageCode;
                bool success = _bal.SaveExpense(expenseModel, entityId, loginId.Value, recordType, out messageCode);

                return Json(new SaveExpenseResponse
                {
                    Success = success,
                    Message = success ? "Expense saved successfully!" : "Error saving expense",
                    MessageCode = messageCode
                });
            }
            catch (Exception ex)
            {
                return Json(new SaveExpenseResponse
                {
                    Success = false,
                    Message = ex.Message,
                    MessageCode = "E0003"
                });
            }
        }
    }
}
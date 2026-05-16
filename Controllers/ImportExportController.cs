// ============================================================
// ImportExportController.cs
// Handles Import (Excel/CSV) and Export (Excel/CSV)
// for all 4 modules: Estimate, Invoice, SalesOrder, CreditNote
// Route: /ImportExport/{action}
// ============================================================

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
using YourApp.BAL;

namespace fintech.Controllers
{
    public class ImportExportController : Controller
    {
        private readonly ImportExportBAL _bal;
        private readonly SalesBAL Salesbal;
        private readonly PurchasesBAL Purchasesbal;


        public ImportExportController()
        {
            _bal = new ImportExportBAL();
            Salesbal = new SalesBAL();
            Purchasesbal = new PurchasesBAL();
        }



        // ── EXPORT ────────────────────────────────────────────
        // GET: /ImportExport/Export?moduleType=Estimate&format=xlsx&entityId=10
        //[HttpGet]
        //public ActionResult Export(string moduleType, string format, int entityId)
        //{
        //    try
        //    {
        //        if (!_IsValidModule(moduleType))
        //            return new HttpStatusCodeResult(400, "Invalid module type.");

        //        format = (format ?? "xlsx").ToLower();

        //        if (format == "csv")
        //        {
        //            byte[] csvBytes = _bal.ExportCsv(moduleType, entityId);
        //            return File(csvBytes, "text/csv",
        //                $"{moduleType}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        //        }
        //        else
        //        {
        //            byte[] xlsxBytes = _bal.ExportExcel(moduleType, entityId);
        //            return File(xlsxBytes,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                $"{moduleType}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new HttpStatusCodeResult(500, ex.Message);
        //    }
        //}


        [HttpGet]
        public ActionResult Export(string moduleType, string format, int entityId)
        {

            try
            {
                byte[] xlsxBytes = null;

                switch (moduleType)
                {
                    case "Estimate":
                        var estimateResult = Salesbal.GetSalesEstimateHistory(null, entityId, 0, 10, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(estimateResult.SalesEstimateHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(estimateResult.SalesEstimateHistory);
                        }
                        break;

                    case "Invoice":
                        var invoiceResult = Salesbal.GetSalesInvoiceHistory(null, entityId, 0, 10, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(invoiceResult.SalesInvoiceHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(invoiceResult.SalesInvoiceHistory);
                        }
                        break;

                    case "SalesOrder":
                        var orderResult = Salesbal.GetSalesOrderHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(orderResult.SalesOrderHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(orderResult.SalesOrderHistory);

                        }
                        break;

                    case "CreditNote":
                        var creditResult = Salesbal.GetSalesCreditNoteHistory(null, entityId, 0, 10, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(creditResult.SalesCreditNoteHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(creditResult.SalesCreditNoteHistory);
                        }
                        break;

                    case "RecInvoice":
                        var recurringResult = Salesbal.GetSalesRecInvHistory(null, entityId, 0, 10, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(recurringResult.SalesRecInvHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(recurringResult.SalesRecInvHistory);
                        }
                        break;
                    case "PurchaseOrder":
                        var purchaseOrderResult = Purchasesbal.GetPurchasesOrderHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(purchaseOrderResult.PurchasesOrderHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(purchaseOrderResult.PurchasesOrderHistory);
                        }
                        break;
                    case "PurchaseBills":
                        var purchaseBillResult = Purchasesbal.GetBillHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(purchaseBillResult.PurchasesBillHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(purchaseBillResult.PurchasesBillHistory);
                        }
                        break;
                    case "PurchaseCreditNote":
                        var PurchaseCreditNoteResult = Purchasesbal.GetPurchasesCreditNotesHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(PurchaseCreditNoteResult.PurchasesCreditNotesHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(PurchaseCreditNoteResult.PurchasesCreditNotesHistory);
                        }
                        break;
                    case "PurchaseRecurringBills":
                        var PurchaseRecurringBillsResult = Purchasesbal.GetRecBillHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(PurchaseRecurringBillsResult.PurchasesRecBillHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(PurchaseRecurringBillsResult.PurchasesRecBillHistory);
                        }
                        break;

                    case "ExpenseHistory":
                        var ExpenseHistoryResult = Purchasesbal.GetExpenseHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(ExpenseHistoryResult.PurchasesExpenseHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(ExpenseHistoryResult.PurchasesExpenseHistory);
                        }
                        break;

                    case "RecurringExpense":
                        var RecurringExpense = Purchasesbal.GetRecExpenseHistory(entityId, 0, 10, null, null, null, null, null);
                        if (format == "csv")
                        {
                            xlsxBytes = ConvertToCsv(RecurringExpense.PurchasesRecExpenseHistory);
                        }
                        else
                        {
                            xlsxBytes = ConvertToExcel(RecurringExpense.PurchasesRecExpenseHistory);
                        }
                        break;

                    default:
                        return new HttpStatusCodeResult(400, "Unknown moduleType");
                }
                string contentType;
                string fileExtension;

                if (format == "csv")
                {
                    contentType = "text/csv";
                    fileExtension = "csv";
                }
                else
                {
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileExtension = "xlsx";
                }

                return File(
                    xlsxBytes,
                    contentType,
                    $"{moduleType}_Export_{DateTime.Now:yyyyMMdd_HHmmss}.{fileExtension}"
                );
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }


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

        private byte[] ConvertToExcel<T>(List<T> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.Worksheets.Add("Export");

                if (data == null || !data.Any())
                    return new byte[0];

                var parentProps = typeof(T).GetProperties();

                // Detect child collection (List<> like Items)
                var childProp = parentProps.FirstOrDefault(p =>
                    p.PropertyType != typeof(string) &&
                    typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) &&
                    p.PropertyType.IsGenericType);

                var childType = childProp?.PropertyType.GetGenericArguments().FirstOrDefault();
                var childProps = childType?.GetProperties() ?? new System.Reflection.PropertyInfo[0];

                // Exclude unwanted props
                var excludedProps = new[] { "Records", "TotalItems", childProp?.Name };

                var parentColumns = parentProps
                    .Where(p => !excludedProps.Contains(p.Name))
                    .ToList();

                var childColumns = childProps.ToList();

                // =========================
                // HEADER ROW
                // =========================
                int colIndex = 1;

                foreach (var col in parentColumns)
                {
                    sheet.Cell(1, colIndex++).Value = col.Name;
                }

                foreach (var col in childColumns)
                {
                    sheet.Cell(1, colIndex++).Value = col.Name;
                }

                sheet.Row(1).Style.Font.Bold = true;
                sheet.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;

                // =========================
                // DATA ROWS (FLATTEN)
                // =========================
                int currentRow = 2;

                foreach (var parent in data)
                {
                    var childList = childProp?.GetValue(parent) as System.Collections.IEnumerable;

                    // No child items → single row
                    if (childList == null || !childList.Cast<object>().Any())
                    {
                        colIndex = 1;

                        foreach (var col in parentColumns)
                        {
                            var value = col.GetValue(parent);
                            SetCellValue(sheet.Cell(currentRow, colIndex++), value);
                        }

                        currentRow++;
                    }
                    else
                    {
                        // 🔥 MULTIPLE ITEMS → multiple rows
                        foreach (var child in childList)
                        {
                            colIndex = 1;

                            // Parent columns (repeated)
                            foreach (var col in parentColumns)
                            {
                                var value = col.GetValue(parent);
                                SetCellValue(sheet.Cell(currentRow, colIndex++), value);
                            }

                            // Child columns
                            foreach (var col in childColumns)
                            {
                                var value = col.GetValue(child);
                                SetCellValue(sheet.Cell(currentRow, colIndex++), value);
                            }

                            currentRow++;
                        }
                    }
                }

                // Auto adjust columns
                sheet.Columns().AdjustToContents();

                // Add filter
                sheet.RangeUsed().SetAutoFilter();

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        private void SetCellValue(IXLCell cell, object value)
        {
            if (value == null)
                cell.SetValue("");
            else if (value is DateTime dt)
                cell.SetValue(dt);
            else if (value is int i)
                cell.SetValue(i);
            else if (value is decimal d)
                cell.SetValue(d);
            else if (value is double db)
                cell.SetValue(db);
            else if (value is bool b)
                cell.SetValue(b);
            else
                cell.SetValue(value.ToString());
        }

        //private byte[] ConvertToCsv<T>(List<T> data)
        //{
        //    var sb = new StringBuilder();
        //    var props = typeof(T).GetProperties();

        //    // ✅ Skip internal/pagination props (same as Excel)
        //    var excludedProps = new[] { "Records", "TotalItems" };
        //    var exportProps = props.Where(p => !excludedProps.Contains(p.Name)).ToArray();

        //    // Headers
        //    sb.AppendLine(string.Join(",", exportProps.Select(p => EscapeCsv(p.Name))));

        //    // Rows
        //    for (int row = 0; row < data.Count; row++)
        //    {
        //        var values = new List<string>();

        //        for (int col = 0; col < exportProps.Length; col++)
        //        {
        //            var value = exportProps[col].GetValue(data[row]);
        //            values.Add(EscapeCsv(value?.ToString() ?? ""));
        //        }

        //        sb.AppendLine(string.Join(",", values));
        //    }

        //    return Encoding.UTF8.GetBytes(sb.ToString());
        //}

        private byte[] ConvertToCsv<T>(List<T> data)
        {
            var sb = new StringBuilder();

            if (data == null || !data.Any())
                return Encoding.UTF8.GetBytes("");

            var parentProps = typeof(T).GetProperties();

            // Detect child collection (List<> like Items)
            var childProp = parentProps.FirstOrDefault(p =>
                p.PropertyType != typeof(string) &&
                typeof(System.Collections.IEnumerable).IsAssignableFrom(p.PropertyType) &&
                p.PropertyType.IsGenericType);

            var childType = childProp?.PropertyType.GetGenericArguments().FirstOrDefault();
            var childProps = childType?.GetProperties() ?? new System.Reflection.PropertyInfo[0];

            // Exclude unwanted fields
            var excludedProps = new[] { "Records", "TotalItems", childProp?.Name };

            var parentColumns = parentProps
                .Where(p => !excludedProps.Contains(p.Name))
                .ToList();

            var childColumns = childProps.ToList();

            // =========================
            // HEADER
            // =========================
            var headers = parentColumns.Select(p => p.Name)
                .Concat(childColumns.Select(c => c.Name));

            sb.AppendLine(string.Join(",", headers.Select(EscapeCsv)));

            // =========================
            // DATA (FLATTEN)
            // =========================
            foreach (var parent in data)
            {
                var childList = childProp?.GetValue(parent) as System.Collections.IEnumerable;

                // No child items → single row
                if (childList == null || !childList.Cast<object>().Any())
                {
                    var row = new List<string>();

                    foreach (var col in parentColumns)
                    {
                        var value = col.GetValue(parent);
                        row.Add(EscapeCsv(value?.ToString() ?? ""));
                    }

                    // Empty child columns
                    foreach (var _ in childColumns)
                    {
                        row.Add("");
                    }

                    sb.AppendLine(string.Join(",", row));
                }
                else
                {
                    // 🔥 MULTIPLE ITEMS → multiple rows
                    foreach (var child in childList)
                    {
                        var row = new List<string>();

                        // Parent values (repeat)
                        foreach (var col in parentColumns)
                        {
                            var value = col.GetValue(parent);
                            row.Add(EscapeCsv(value?.ToString() ?? ""));
                        }

                        // Child values
                        foreach (var col in childColumns)
                        {
                            var value = col.GetValue(child);
                            row.Add(EscapeCsv(value?.ToString() ?? ""));
                        }

                        sb.AppendLine(string.Join(",", row));
                    }
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value;
        }

        // ── IMPORT ────────────────────────────────────────────
        // POST: /ImportExport/Import
        [HttpPost]
        public JsonResult Import(string moduleType, string format, int entityId, int createdBy)
        {
            try
            {
                if (!_IsValidModule(moduleType))
                    return Json(new { success = false, message = "Invalid module type." });

                if (Request.Files == null || Request.Files.Count == 0)
                    return Json(new { success = false, message = "No file uploaded." });

                HttpPostedFileBase file = Request.Files[0];
                if (file == null || file.ContentLength == 0)
                    return Json(new { success = false, message = "Uploaded file is empty." });

                format = (format ?? "xlsx").ToLower();

                ImportResultsales result = format == "csv"
                    ? _bal.ImportCsv(moduleType, file, entityId, createdBy)
                    : _bal.ImportExcel(moduleType, file, entityId, createdBy);

                return Json(new
                {
                    success = result.Success,
                    totalRows = result.TotalRows,
                    successCount = result.SuccessCount,
                    failCount = result.FailCount,
                    errors = result.Errors   // list of { row, message }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        // ── Download blank template ───────────────────────────
        // GET: /ImportExport/Template?moduleType=Estimate&format=xlsx
        // CHANGED: serve pre-built sample file instead of generating blank template
        [HttpGet]
        public ActionResult Template(string moduleType, string format)
        {
            try
            {
                if(moduleType == "ExpenseHistory" || moduleType == "ExpenseBulkBooking" || moduleType == "RecurringExpense")
                {
                    format = (format ?? "xlsx").ToLower();

                    // CHANGED: build path to your pre-built sample file
                    string fileName = $"{moduleType}_Sample.{format}";
                    string filePath = Server.MapPath($"~/Content/SampleFiles/{fileName}");

                    if (!System.IO.File.Exists(filePath))
                        return new HttpStatusCodeResult(404,
                            $"Sample file not found: {fileName}");

                    string mimeType = format == "csv"
                        ? "text/csv"
                        : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // CHANGED: read and return the actual sample file
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, mimeType, fileName);
                }
                else
                {
                    if (!_IsValidModule(moduleType))
                        return new HttpStatusCodeResult(400, "Invalid module type.");

                    format = (format ?? "xlsx").ToLower();

                    // CHANGED: build path to your pre-built sample file
                    string fileName = $"{moduleType}_Sample.{format}";
                    string filePath = Server.MapPath($"~/Content/SampleFiles/{fileName}");

                    if (!System.IO.File.Exists(filePath))
                        return new HttpStatusCodeResult(404,
                            $"Sample file not found: {fileName}");

                    string mimeType = format == "csv"
                        ? "text/csv"
                        : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // CHANGED: read and return the actual sample file
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, mimeType, fileName);
                }
                
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        private bool _IsValidModule(string moduleType)
        {
            var allowed = new[] { "Estimate", "Invoice", "SalesOrder", "CreditNote", "RecInvoice", "PurchaseOrder", "PurchaseBills", "PurchaseCreditNote", "PurchaseRecurringBills", "ExpenseHistory" };

            return !string.IsNullOrWhiteSpace(moduleType) &&
                   Array.IndexOf(allowed, moduleType) >= 0;
        }
    }
}

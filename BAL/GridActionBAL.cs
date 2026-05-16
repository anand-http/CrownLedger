// ============================================================
// GridActionBAL.cs — Business Access Layer
// ============================================================

using fintech.DAL;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace YourApp.BAL
{
    public class GridActionBAL
    {
        private readonly GridActionDAL _dal;

        public GridActionBAL()
        {
            _dal = new GridActionDAL();
        }

        // ── Generate PDF for Print / Download / Email ─────────
        // Plug in your preferred PDF library here.
        // iTextSharp and SelectPdf are both common in .NET MVC.
        public byte[] GeneratePdf(string moduleType, int[] ids)
        {
            List<Dictionary<string, object>> rows =
                _dal.ExecuteGridAction(moduleType, ids, "GET").Rows;

            if (rows == null || rows.Count == 0)
            {
                throw new Exception("No data found for the selected records.");
            }

            string html = BuildHtmlForModule(moduleType, rows);

            var converter = new SelectPdf.HtmlToPdf();
            converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
            converter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Landscape;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;

            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html);
            byte[] bytes = doc.Save();
            doc.Close();
            return bytes;
        }

        // ── Send Email with PDF attachment ────────────────────
        public void SendEmail(string moduleType, string toEmail,string emailsub,string emailbody, byte[] pdfBytes)
        {

            // CHANGED: reading SMTP config manually from AppSettings
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string username = ConfigurationManager.AppSettings["FromMailUserId"];
            string password = ConfigurationManager.AppSettings["FromMailPassword"];
            string fromMail = ConfigurationManager.AppSettings["FromMailId"];

            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(fromMail);
                msg.To.Add(toEmail);
                msg.Subject = emailsub == null ? $"{moduleType} Documents": emailsub ;
                msg.Body = emailbody == null ? $"Please find the attached {moduleType.ToLower()} document(s).": emailbody;
                msg.IsBodyHtml = false;

                var attachment = new Attachment(
                    new System.IO.MemoryStream(pdfBytes),
                    $"{moduleType}_{DateTime.Now:yyyyMMddHHmmss}.pdf",
                    "application/pdf");
                msg.Attachments.Add(attachment);

                // CHANGED: explicitly setting host, port, credentials
                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new System.Net.NetworkCredential(username, password);
                    client.EnableSsl = true;   // AWS SES requires TLS on port 587
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(msg);
                }
            }
        }

        // ── Soft Delete records ───────────────────────────────
        public int DeleteRecords(string moduleType, int[] ids)
        {
            return _dal.ExecuteGridAction(moduleType, ids, "DELETE").AffectedCount;
        }

        // ============================================================
        // Add this method inside GridActionBAL.cs
        // Builds a simple HTML string for PDF generation.
        // One method handles all 4 modules via moduleType switch.
        // ============================================================

        private string BuildHtmlForModule(string moduleType, List<Dictionary<string, object>> rows)
        {
            var sb = new System.Text.StringBuilder();

            // ── Common HTML header / styles ───────────────────────
            sb.Append(@"
        <html>
        <head>
            <style>
                body  { font-family: Arial, sans-serif; font-size: 13px; color: #333; }
                h2    { color: #1a1a2e; border-bottom: 2px solid #ADBFCD; padding-bottom: 6px; }
                table { width: 100%; border-collapse: collapse; margin-top: 16px; }
                th    { background-color: #1a1a2e; color: #fff; padding: 8px 10px; text-align: left; font-size: 12px; }
                td    { padding: 7px 10px; border-bottom: 1px solid #e0e0e0; font-size: 12px; }
                tr:nth-child(even) td { background-color: #f5f8fa; }
                .badge { padding: 3px 8px; border-radius: 4px; font-size: 11px; font-weight: bold; }
                .badge-approved { background:#d4edda; color:#155724; }
                .badge-pending  { background:#fff3cd; color:#856404; }
                .badge-draft    { background:#e2e3e5; color:#383d41; }
            </style>
        </head>
        <body>");

            // ── Title ─────────────────────────────────────────────
            sb.Append($"<h2>{moduleType} Report</h2>");
            sb.Append($"<p style='color:#888;font-size:11px;'>Generated on: {DateTime.Now:dd-MM-yyyy HH:mm}</p>");

            // ── Table columns per module ──────────────────────────
            switch (moduleType)
            {
                case "Estimate":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Estimate Date</th>
                    <th>Estimate Number</th>
                    <th>Customer Name</th>
                    <th>Reference</th>
                    <th>Expected Shipment</th>
                    <th>Amount</th>
                    <th>Status</th>
                    <th>Created By</th>
                </tr></thead><tbody>");

                    int estRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{estRow++}</td>
                        <td>{_FormatDate(row, "EstimateDate")}</td>
                        <td>{_Val(row, "EstimateNumber")}</td>
                        <td>{_Val(row, "Customer")}</td>
                        <td>{_Val(row, "ReferenceNumber")}</td>
                        <td>{_FormatDate(row, "ExpiryDate")}</td>
                        <td>{_FormatAmount(row, "Amount")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                    </tr>");
                    }
                    break;

                case "Invoice":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Invoice Date</th>
                    <th>Due Date</th>
                    <th>Customer Name</th>
                    <th>Invoice Number</th>
                    <th>Order Number</th>
                    <th>Amount</th>
                    <th>Status</th>
                    <th>Created By</th>
                </tr></thead><tbody>");

                    int invRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{invRow++}</td>
                        <td>{_FormatDate(row, "InvoiceDate")}</td>
                        <td>{_FormatDate(row, "DueDate")}</td>
                        <td>{_Val(row, "CustomerName")}</td>
                        <td>{_Val(row, "InvoiceNumber")}</td>
                        <td>{_Val(row, "Sale_OrderNo")}</td>
                        <td>{_FormatAmount(row, "TotalAmount")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                    </tr>");
                    }
                    break;

                case "SalesOrder":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Order Date</th>
                    <th>Expected Shipment</th>
                    <th>Customer Name</th>
                    <th>Order Number</th>
                    <th>Reference</th>
                    <th>Amount</th>
                    <th>Status</th>
                    <th>Created By</th>
                </tr></thead><tbody>");

                    int soRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{soRow++}</td>
                        <td>{_FormatDate(row, "Sale_OrderDate")}</td>
                        <td>{_FormatDate(row, "ShipmentDate")}</td>
                        <td>{_Val(row, "CustomerName")}</td>
                        <td>{_Val(row, "Sale_OrderNo")}</td>
                        <td>{_Val(row, "Sale_EstimateNo")}</td>
                        <td>{_FormatAmount(row, "Amount")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                    </tr>");
                    }
                    break;

                case "CreditNote":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Credit Note Date</th>
                    <th>Customer Name</th>
                    <th>Credit Note Number</th>
                    <th>Reference</th>
                    <th>Amount</th>
                    <th>Status</th>
                    <th>Created By</th>
                </tr></thead><tbody>");

                    int cnRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{cnRow++}</td>
                        <td>{_FormatDate(row, "CreditNoteDate")}</td>
                        <td>{_Val(row, "CustomerName")}</td>
                        <td>{_Val(row, "CreditNoteNumber")}</td>
                        <td>{_Val(row, "InvoiceNumber")}</td>
                        <td>{_FormatAmount(row, "TotalAmount")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                    </tr>");
                    }
                    break;

                case "PurchaseOrder":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>PO Date</th>
                    <th>PO Number</th>
                    <th>Vendor Name</th>
                    <th>PO Amount</th>
                    <th>PO Status</th>
                    <th>Billing Status</th>
                    <th>Created By</th>
                    <th>Created On</th>
                </tr></thead><tbody>");

                    int poRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{poRow++}</td>
                        <td>{_FormatDate(row, "PODate")}</td>
                        <td>{_Val(row, "PONumber")}</td>
                        <td>{_Val(row, "VendorName")}</td>
                        <td>{_Val(row, "POAmount")}</td>
                        <td>{_Val(row, "POStatus")}</td>
                        <td>{_Val(row, "BillingStatus")}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                        <td>{_FormatDate(row, "CreatedOn")}</td>
                    </tr>");
                    }
                    break;
                case "PurchaseBills":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Bill Date</th>
                    <th>Bill Number</th>
                    <th>Vendor Name</th>
                    <th>PO Number</th>
                    <th>Bill Amount</th>
                    <th>Due Date</th>
                    <th>Amount Outstanding</th>
                    <th>Status</th>
                    <th>Created By</th>
                    <th>Created On</th>
                </tr></thead><tbody>");

                    int pbRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{pbRow++}</td>
                        <td>{_FormatDate(row, "BillDate")}</td>
                        <td>{_Val(row, "BillNumber")}</td>
                        <td>{_Val(row, "VendorName")}</td>
                        <td>{_Val(row, "PONumber")}</td>
                        <td>{_Val(row, "BillAmount")}</td>
                        <td>{_FormatDate(row, "DueDate")}</td>
                        <td>{_Val(row, "AmountOutstanding")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                        <td>{_FormatDate(row, "CreatedOn")}</td>
                    </tr>");
                    }
                    break;

                case "PurchaseCreditNote":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Date</th>
                    <th>Credit Note Number</th>
                    <th>Invoice Number</th>
                    <th>Vendor Name</th>
                    <th>Credit Note Amount</th>
                    <th>Status</th>
                    <th>Created By</th>
                    <th>Created On</th>
                </tr></thead><tbody>");

                    int pcnRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{pcnRow++}</td>
                        <td>{_FormatDate(row, "Date")}</td>
                        <td>{_Val(row, "CreditNoteNumber")}</td>
                        <td>{_Val(row, "InvoiceNumber")}</td>
                        <td>{_Val(row, "VendorName")}</td>
                        <td>{_FormatAmount(row, "CreditNoteAmount")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                        <td>{_FormatDate(row, "CreatedOn")}</td>
                    </tr>");
                    }
                    break;

                case "PurchaseRecurringBills":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Vendor Name</th>
                    <th>Bill Number</th>
                    <th>Frequency</th>
                    <th>Previous Invoice Date</th>
                    <th>Next Invoice Date</th>
                    <th>End Date</th>
                    <th>Status</th>
                    <th>Amount</th>
                </tr></thead><tbody>");

                    int prbRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{prbRow++}</td>
                        <td>{_Val(row, "VendorName")}</td>
                        <td>{_Val(row, "BillNumber")}</td>
                        <td>{_Val(row, "Frequency")}</td>
                        <td>{_FormatAmount(row, "PreviousInvoiceDate")}</td>
                        <td>{_FormatDate(row, "NextInvoiceDate")}</td>
                        <td>{_FormatDate(row, "EndDate")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_FormatAmount(row, "Amount")}</td>
                    </tr>");
                    }
                    break;

                case "ExpenseHistory":
                    sb.Append(@"
                <table>
                <thead><tr>
                    <th>#</th>
                    <th>Date</th>
                    <th>Vendor Name</th>
                    <th>Ledger Account</th>
                    <th>Expense Amount</th>
                    <th>Paid Through</th>
                    <th>Status</th>
                    <th>Created By</th>
                    <th>Created On</th>
                </tr></thead><tbody>");

                    int ehRow = 1;
                    foreach (var row in rows)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{ehRow++}</td>
                        <td>{_FormatDate(row, "ExpenseDate")}</td>
                        <td>{_Val(row, "VendorName")}</td>
                        <td>{_Val(row, "LedgerAccount")}</td>
                        <td>{_FormatAmount(row, "ExpenseAmount")}</td>
                        <td>{_Val(row, "PaidFrom")}</td>
                        <td>{_StatusBadge(row)}</td>
                        <td>{_Val(row, "CreatedBy")}</td>
                        <td>{_FormatDate(row, "CreatedOn")}</td>
                    </tr>");
                    }
                    break;
            }

            sb.Append("</tbody></table></body></html>");
            return sb.ToString();
        }

        // ── Private helpers ───────────────────────────────────────

        // Safe value reader — returns empty string if key missing or null
        private string _Val(Dictionary<string, object> row, string key)
        {
            return row.ContainsKey(key) && row[key] != null
                ? row[key].ToString()
                : string.Empty;
        }

        // Format DateTime columns — returns dd-MM-yyyy or empty
        private string _FormatDate(Dictionary<string, object> row, string key)
        {
            if (row.ContainsKey(key) && row[key] != null &&
                DateTime.TryParse(row[key].ToString(), out DateTime dt))
                return dt.ToString("dd-MM-yyyy");
            return string.Empty;
        }

        // Format decimal/money columns — returns ₹ formatted or raw
        private string _FormatAmount(Dictionary<string, object> row, string key)
        {
            if (row.ContainsKey(key) && row[key] != null &&
                decimal.TryParse(row[key].ToString(), out decimal amt))
                return amt.ToString("N2");   // e.g. 5,460.00 — change to "C" for currency symbol
            return string.Empty;
        }

        // Render status as a coloured badge
        private string _StatusBadge(Dictionary<string, object> row)
        {
            string status = _Val(row, "Status");
            string css;

            if (status?.ToLower() == "approved")
                css = "badge-approved";
            else if (status?.ToLower() == "pending")
                css = "badge-pending";
            else
                css = "badge-draft";

            return $"<span class='badge {css}'>{status}</span>";
        }
    }
}

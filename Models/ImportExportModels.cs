// ============================================================
// ImportExportModels.cs — put in your Models folder
// ============================================================

using System.Collections.Generic;

namespace fintech.Models
{
    // ── Import result returned to JS ─────────────────────────
    public class ImportResultsales
    {
        public int TotalRows { get; set; }
        public bool Success { get; set;}
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public List<ImportRowError> Errors { get; set; } = new List<ImportRowError>();
    }

    public class ImportRowError
    {
        public int Row { get; set; }    // 1-based row number in file
        public string Message { get; set; }
    }

    // ── Flat import row — same fields across all modules ─────
    public class ImportRow
    {
        // Common across all modules
        public string Date { get; set; }
        public string ExpiryDate { get; set; }   // DueDate for Invoice
        public string CustomerName { get; set; }
        public string DocumentNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
    }

    public class ImportDocument
    {
        public string ModuleType { get; set; }
        public int EntityId { get; set; }
        public int CreatedBy { get; set; }

        // Header fields
        public string CustomerName { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public string SubTotal { get; set; }
        public string Total { get; set; }
        public string Discount { get; set; }
        public string OtherAdjustment { get; set; }
        public string Tax { get; set; }

        // Line items
        public List<ImportLineItem> Items { get; set; } = new List<ImportLineItem>();
    }

    public class ImportLineItem
    {
        public string ItemDetails { get; set; }
        public string ItemDescription { get; set; }
        public string Itemtype { get; set; }
        public string HSNCode { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string TaxType { get; set; }
        public string Amount { get; set; }
        public string Discount { get; set; }
        public string OtherAdjustment { get; set; }
        public string Tax { get; set; }
    }
}

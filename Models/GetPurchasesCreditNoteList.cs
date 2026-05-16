

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class PurchasesCreditNotesHistoryFullResult
    {
        public PurchasesCreditNotesHistoryFullResult()
        {
            PurchasesCreditNotesHistory = new List<PurchasesCreditNotesHistoryList>();
        }

        public List<PurchasesCreditNotesHistoryList> PurchasesCreditNotesHistory { get; set; }
    }

    public class PurchasesCreditNotesHistoryList
    {
        public DateTime? CreditDate { get; set; }
        public string CreditNoteNumber { get; set; }
        public string BillNumber { get; set; }
        public string VendorName { get; set; }
        public decimal? CreditAmount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public int? PurchaseCreditNoteId { get; set; }
        public List<PurchasesCreditNotesItems> Items { get; set; } = new List<PurchasesCreditNotesItems>();
    }

    public class PurchasesCreditNotesItems
    {
        public int? PurchaseCreditNoteId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get; set; }

        public int Quantity { get; set; }
        public decimal Rate { get; set; }

        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string TaxType { get; set; }
        public decimal BaseAmount { get; set; }
    }
}
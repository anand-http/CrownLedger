

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class SalesCreditNoteHistoryFullResult
    {
        public SalesCreditNoteHistoryFullResult()
        {
            SalesCreditNoteHistory = new List<SalesCreditNoteHistory>();
        }

        public List<SalesCreditNoteHistory> SalesCreditNoteHistory { get; set; }
    }

    public class SalesCreditNoteHistory
    {
        public int? Sale_CrNoteId { get; set; }
        public DateTime? Sale_CrNoteDate { get; set; }
        public string Sale_CrNoteNo { get; set; }
        public string CustomerName { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public string ReferenceNumber { get; set; }
        public int TotalItems { get; set; }

        public List<SalesCreditNoteItem> Items { get; set; } = new List<SalesCreditNoteItem>();
    }

    public class SalesCreditNoteItem
    {
        public int? Sale_CrNoteId { get; set; }
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
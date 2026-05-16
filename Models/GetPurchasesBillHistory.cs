

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class PurchasesBillHistoryFullResult
    {
        public PurchasesBillHistoryFullResult()
        {
            PurchasesBillHistory = new List<PurchasesBillHistoryList>();
        }

        public List<PurchasesBillHistoryList> PurchasesBillHistory { get; set; }
    }

    public class PurchasesBillHistoryList
    {
        public DateTime? BillDate { get; set; }
        public string BillNumber { get; set; }
        public string VendorName { get; set; }
        public string PONumber { get; set; }
        public decimal? BillAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? AmountOutstanding { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public int? PurchaseBillId { get; set; }

        public List<PurchaseBillItem> Items { get; set; } = new List<PurchaseBillItem>();
    }

    public class PurchaseBillItem
    {
        public int? PurchaseBillId { get; set; }
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
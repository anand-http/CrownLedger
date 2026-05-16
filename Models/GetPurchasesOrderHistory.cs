

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class PurchasesOrderHistoryFullResult
    {
        public PurchasesOrderHistoryFullResult()
        {
            PurchasesOrderHistory = new List<PurchasesOrderHistoryList>();
        }

        public List<PurchasesOrderHistoryList> PurchasesOrderHistory { get; set; }
    }

    public class PurchasesOrderHistoryList
    {
        public DateTime? Date { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string Number { get; set; }
        public string VendorName { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public string BillingStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public List<PurchaseOrderListItem> Items { get; set; } = new List<PurchaseOrderListItem>();
    }

    public class PurchaseOrderListItem
    {
        public int? PurchaseOrderId { get; set; }
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
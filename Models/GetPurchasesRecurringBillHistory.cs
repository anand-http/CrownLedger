

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class PurchasesRecBillHistoryFullResult
    {
        public PurchasesRecBillHistoryFullResult()
        {
            PurchasesRecBillHistory = new List<PurchasesRecBillHistoryList>();
        }

        public List<PurchasesRecBillHistoryList> PurchasesRecBillHistory { get; set; }
    }

    public class PurchasesRecBillHistoryList
    {
        public string VendorName { get; set; }
        public string BillNumber { get; set; }
        public string Frequency { get; set; }
        public DateTime? PreviousBillDate { get; set; }
        public DateTime? NextBillDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public decimal? Amount { get; set; }
        public int? Records { get; set; }
        public int? RecurringScheduleId { get; set; }

        public List<PurchasesRecBillItems> Items { get; set; } = new List<PurchasesRecBillItems>();
    }

    public class PurchasesRecBillItems
    {
        public int? RecurringScheduleId { get; set; }
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
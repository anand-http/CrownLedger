

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class SalesRecInvHistoryFullResult
    {
        public SalesRecInvHistoryFullResult()
        {
            SalesRecInvHistory = new List<SalesRecInvHistoryList>();
        }

        public List<SalesRecInvHistoryList> SalesRecInvHistory { get; set; }
    }

    public class SalesRecInvHistoryList
    {
        public int? InvoiceId { get; set; }
        public string PreviousInvoiceDate { get; set; }
        public DateTime? NextInvoiceDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string Frequency { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public int TotalItems { get; set; }

        public List<SalesRecInvListItem> Items { get; set; } = new List<SalesRecInvListItem>();
    }

    public class SalesRecInvListItem
    {
        public int? InvoiceId { get; set; }
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
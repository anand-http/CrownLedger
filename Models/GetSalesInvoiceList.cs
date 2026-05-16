

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class SalesInvoiceHistoryFullResult
    {
        public SalesInvoiceHistoryFullResult()
        {
            SalesInvoiceHistory = new List<SalesInvoiceHistoryList>();
        }

        public List<SalesInvoiceHistoryList> SalesInvoiceHistory { get; set; }
    }

    public class SalesInvoiceHistoryList
    {
        public int? Sale_InvoiceId { get; set; }
        public DateTime? Sale_InvoiceDate { get; set; }
        public string Sale_InvoiceNo { get; set; }
        public string CustomerName { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public string InvoiceNumber { get; set; }
        public int TotalItems { get; set; }

        public List<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
    }


    public class SalesInvoiceItem
    {
        public int? Sale_InvoiceId { get; set; }
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
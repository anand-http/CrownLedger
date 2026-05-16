

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;

    public class SalesEstimateHistoryFullResult
    {
        public SalesEstimateHistoryFullResult()
        {
            SalesEstimateHistory = new List<SalesEstimateHistoryList>();
        }

        public List<SalesEstimateHistoryList> SalesEstimateHistory { get; set; }
    }

    public class SalesEstimateHistoryList
    {
        public int? Sale_EstimateId { get; set; }
        public DateTime? EstimateDate { get; set; }
        public string Sale_EstimateNo { get; set; }
        public string CustomerName { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public string EstimateNumber { get; set; }
        public int TotalItems { get; set; }

        public List<SalesEstimateItem> Items { get; set; } = new List<SalesEstimateItem>();
    }

    public class SalesEstimateItem
    {
        public int? Sale_EstimateId { get; set; }
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
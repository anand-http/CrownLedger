

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class SalesOrderHistoryFullResult
    {
        public SalesOrderHistoryFullResult()
        {
            SalesOrderHistory = new List<SalesOrderHistoryList>();
        }

        public List<SalesOrderHistoryList> SalesOrderHistory { get; set; }
    }

    public class SalesOrderHistoryList
    {
        public int? Sale_OrderId { get; set; }
        public DateTime? Sale_OrderDate { get; set; }
        public string Sale_OrderNo { get; set; }
        public string CustomerName { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public string EstimateNumber { get; set; }
        public int TotalItems { get; set; }
        public List<SalesOrderListItem> Items { get; set; } = new List<SalesOrderListItem>();
    }

    public class SalesOrderListItem
    {
        public int? Sale_OrderId { get; set; }
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
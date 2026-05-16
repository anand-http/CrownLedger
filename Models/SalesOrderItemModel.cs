
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace fintech.Models
{
    public class SalesOrderItemModel
    {
        public string Sale_OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string ReferenceNumber { get; set; }
        public int? Payment_Term1 { get; set; }
        public string Payment_Term2 { get; set; }
        public int CurrencyId { get; set; }
        public string DeliveryMethod { get; set; }
        public string ShipmentDate { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public int LoginId { get; set; }
        public int Entity_ID { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }

        public int Sale_EstimateId { get; set; }

        public List<SalesOrderModel> Items { get; set; } = new List<SalesOrderModel>();
    }

    public class SalesOrderModel
    {
        public string ItemType { get; set; }
        public int? ItemId { get; set; }
        public string Description { get; set; }
        public string HSNCode { get; set; }

        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public int DiscountValue { get; set; }

        public string DiscountType { get; set; }

        public int? TaxGroupId { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal TaxRate_Percent { get; set; }
    }

}
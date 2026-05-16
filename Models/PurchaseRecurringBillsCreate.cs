
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace fintech.Models
{
    public class PurchaseRecurringBillsCreate
    {
        public int? LoginId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int VendorId { get; set; }
        public string Frequency { get; set; }
        public int? Payment_Term1 { get; set; }
        public string Payment_Term2 { get; set; }
        public int CurrencyId { get; set; }
        public int BranchId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }

        public List<PurchaseRecurringBillItem> Items { get; set; } = new List<PurchaseRecurringBillItem>();
    }

    public class PurchaseRecurringBillItem
    {
        public string ItemType { get; set; }
        public int? ItemId { get; set; }
        public int? GL_ID { get; set; }

        public int Quantity { get; set; }
        public decimal Rate { get; set; }

        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

        public int? TaxGroupId { get; set; }
        public decimal BaseAmount { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace fintech.Models
{
    public class PurchaseCreditNoteCreate
    {
        public int? LoginId { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public DateTime? AccountingDate { get; set; }
        public int VendorId { get; set; }
        public int PurchaseBillId { get; set; }
        public int CurrencyId { get; set; }
        public string Reason { get; set; }
        public int BranchId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }

        public List<PurchaseCreditItem> Items { get; set; } = new List<PurchaseCreditItem>();
    }

    public class PurchaseCreditItem
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
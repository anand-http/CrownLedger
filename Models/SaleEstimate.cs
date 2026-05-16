
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static fintech.Models.CommonModel;

namespace fintech.Models
{
        public class SaleEstimateModel
        {
            public int? Sale_EstimateId { get; set; }
            public DateTime? Sale_EstimateDate { get; set; }
            public int CustomerId { get; set; }
            public string ReferenceNumber { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public decimal SubTotal { get; set; }
            public decimal Discount { get; set; }
            public decimal OtherAdjustment { get; set; }
            public decimal Tax_Value { get; set; }
            public decimal TotalAmount { get; set; }
            public string Status { get; set; }
            public string Comments { get; set; }
            public string TermsAndConditions { get; set; }
            public List<ItemDetailModel> Items { get; set; }
            public int? Entity_ID { get; set; }
            public int? Login_ID { get; set; }
    }

        public class SaleInvoice
    {
        public int? Sale_InvoiceId { get; set; }
        public string Sale_InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string ReferenceNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string AccountingDate { get; set; }
        public int Currency { get; set; }
        public int PaymentTerm { get; set; }
        public int Branch { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<ItemDetailModel> Items { get; set; }
        public int? Entity_ID { get; set; }
        public int? Login_ID { get; set; }
    }

        public class SaleCreditNote
    {
        public int? Sale_CreditNoteId { get; set; }
        public string Sale_CreditNoteDate { get; set; }
        public int CustomerId { get; set; }
        public string ReferenceNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string AccountingDate { get; set; }
        public int Currency { get; set; }
        public string Reason { get; set; }
        public int Branch { get; set; }
        public int SalesPerson { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<ItemDetailModel> Items { get; set; }
        public int? Entity_ID { get; set; }
        public int? Login_ID { get; set; }
    }

    public class SaleRecInv
    {
        public string StartDate { get; set; }
        public int CustomerId { get; set; }
        public string ReferenceNumber { get; set; }
        public string EndDate { get; set; }
        public string Frequency { get; set; }
        public int Paymentterms { get; set; }
        public int Currency { get; set; }
        public string Reason { get; set; }
        public int Branch { get; set; }
        public int SalesPerson { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<ItemDetailModel> Items { get; set; }
        public int? Entity_ID { get; set; }
        public int? Login_ID { get; set; }
    }

    public class SaveResult
        {
            public bool Status { get; set; }
            public string Message { get; set; }
        }

}
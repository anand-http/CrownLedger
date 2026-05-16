using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fintech.Models
{
    // ── Common Base Model ──
    public class SalesDocumentBase
    {
        public long DocumentId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Entity_ID { get; set; }
        public int LoginId { get; set; }
    }

    // ── Sales Estimate ──
    public class SalesEstimate : SalesDocumentBase
    {
        public string ReferenceNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<EstimateItem> Items { get; set; }
    }

    // ── Sales Invoice ──
    public class SalesInvoice : SalesDocumentBase
    {
        public string ReferenceNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<InvoiceItem> Items { get; set; }
    }

    // ── Sales Order ──
    public class SalesOrder : SalesDocumentBase
    {
        public int EstimateId { get; set; }   // Reference: Estimate Number
        public string EstimateNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<SalesOrderItem> Items { get; set; }
    }

    // ── Invoice (from Sales Order) ──
    public class Invoice : SalesDocumentBase
    {
        public int SalesOrderId { get; set; }   // Sales Order Number
        public string SalesOrderNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<InvoiceItem> Items { get; set; }
    }

    // ── Credit Note ──
    public class CreditNote : SalesDocumentBase
    {
        public int InvoiceId { get; set; }   // Invoice Number
        public string InvoiceNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal OtherAdjustment { get; set; }
        public decimal Tax_Value { get; set; }
        public decimal TotalAmount { get; set; }
        public string Reason { get; set; }
        public string Comments { get; set; }
        public string TermsAndConditions { get; set; }
        public List<CreditNoteItem> Items { get; set; }
    }

    // ── Item Models ──
    public class EstimateItem
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }  // "P" = Product, "S" = Service
        public string Description { get; set; }
        public string HSNCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }  // "P" = Percent, "A" = Absolute
        public int TaxGroupId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal BaseAmount { get; set; }
    }

    public class SalesOrderItem
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string HSNCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public int TaxGroupId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal BaseAmount { get; set; }
        public int EstimateItemId { get; set; }  // reference back to estimate item
    }

    public class InvoiceItem
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string HSNCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public int TaxGroupId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public int SalesOrderItemId { get; set; } // reference back to order item
    }

    public class CreditNoteItem
    {
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public string Description { get; set; }
        public string HSNCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public int TaxGroupId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal BaseAmount { get; set; }
        public int InvoiceItemId { get; set; }  // reference back to invoice item
    }

    // ── List Result Models (for grids/listings) ──
    public class SalesEstimateList : SalesDocumentBase
    {
        public string ReferenceNumber { get; set; }
        public int TotalItems { get; set; }
    }

    public class SalesInvoiceList : SalesDocumentBase
    {
        public string ReferenceNumber { get; set; }
        public decimal Dueamount { get; set; }
        public int TotalItems { get; set; }
    }

    public class SalesOrderList : SalesDocumentBase
    {
        public int EstimateId { get; set; }
        public string EstimateNumber { get; set; }
        public int TotalItems { get; set; }
    }

    public class InvoiceList : SalesDocumentBase
    {
        public int SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; }
        public decimal DueAmount { get; set; }
        public int TotalItems { get; set; }
    }

    public class CreditNoteList : SalesDocumentBase
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime AccountingDate { get; set; }
        public int TotalItems { get; set; }
    }
}
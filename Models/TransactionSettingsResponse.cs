

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class TransactionSettingsResponse
    {
        public TransactionSettingsResponse()
        {
            TransactionSettings = new List<TransactionSettingsData>();
        }

        public List<TransactionSettingsData> TransactionSettings { get; set; }
    }

    public class TransactionSettingsData
    {
        public string CustomerInvoiceDueType { get; set; }
        public int CustomerInvoiceDueDays { get; set; }
        public string SalesLedger { get; set; }
        public int SalesLedgerId { get; set; }
        public string DiscountLedger { get; set; }
        public int DiscountLedgerId { get; set; }
        //vendor
        public string VendorInvoiceDueType { get; set; }
        public int VendorInvoiceDueDays { get; set; }
        public string VendorExpenseLedger { get; set; }
        public int VendorExpenseLedgerId { get; set; }
        public string VendorDiscountLedger { get; set; }
        public int VendorDiscountLedgerId { get; set; }
        //payment
        public int OtherReceiptLedgerId { get; set; }
        public int OtherPaymentLedgerId { get; set; }
        public int CustomerReceiptDiscountLedgerId { get; set; }
        public int VendorPaymentDiscountLedgerId { get; set; }
        public int BankInterestReceivedLedgerId { get; set; }
        public int BankInterestChargesLedgerId { get; set; }
        public string OtherReceiptLedger { get; set; }
        public string OtherPaymentLedger { get; set; }
        public string CustomerReceiptDiscountLedger { get; set; }
        public string VendorPaymentDiscountLedger { get; set; }
        public string BankInterestReceivedLedger { get; set; }
        public string BankInterestChargesLedger { get; set; }
        //product service
        public int NonStockSalesAccountId { get; set; }
        public int NonStockExpenseAccountId { get; set; }
        public int StockExpenseAccountId { get; set; }
        public int ServiceSalesAccountId { get; set; }
        public int ServiceExpenseAccountId { get; set; }
        public string NonStockSalesAccount { get; set; }
        public string NonStockExpenseAccount { get; set; }
        public string StockExpenseAccount { get; set; }
        public string ServiceSalesAccount { get; set; }
        public string ServiceExpenseAccount { get; set; }
        public bool AllowDuplicateItemNames { get; set; }
        //accounting
        public int ShowNegativeAssetsId { get; set; }
        public string ShowNegativeAssets { get; set; }
        public bool EnableMultiCurrency { get; set; }
        public List<TransactionSettingsAgingData> Aging { get; set; } = new List<TransactionSettingsAgingData>();
    }

    public class TransactionSettingsAgingData
    {
        public List<string> CustomerAgingFrom { get; set; }
        public List<string> CustomerAgingTo { get; set; }
        public List<string> VendorAgingFrom { get; set; }
        public List<string> VendorAgingTo { get; set; }
    }
}
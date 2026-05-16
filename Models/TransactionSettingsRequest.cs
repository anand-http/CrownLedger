using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace fintech.Models
{

    public class TransactionSettingsRequest
    {
        public CustomerSettingsModel CustomerSettings { get; set; }
        public VendorSettingsModel VendorSettings { get; set; }
        public PaymentReceiptSettingsModel PaymentReceiptSettings { get; set; }
        public ProductServiceSettingsModel ProductServiceSettings { get; set; }
        public AccountingSettingsModel AccountingSettings { get; set; }
        public CurrencySettingsModel CurrencySettings { get; set; }
    }

    public class CustomerSettingsModel
    {
        public string InvoiceDueType { get; set; }
        public int InvoiceDueDays { get; set; }
        public List<string> AgingFrom { get; set; }
        public List<string> AgingTo { get; set; }
        public int SalesLedger { get; set; }
        public int DiscountLedger { get; set; }
    }

    public class VendorSettingsModel
    {
        public string InvoiceDueType { get; set; }
        public string InvoiceDueDays { get; set; }
        public List<string> AgingFrom { get; set; }
        public List<string> AgingTo { get; set; }
        public int ExpenseLedger { get; set; }
        public int DiscountLedger { get; set; }
    }

    public class PaymentReceiptSettingsModel
    {
        public int OtherReceiptLedger { get; set; }
        public int OtherPaymentLedger { get; set; }
        public int CustomerReceiptDiscountLedger { get; set; }
        public int VendorPaymentDiscountLedger { get; set; }
        public int BankInterestReceivedLedger { get; set; }
        public int BankInterestChargesLedger { get; set; }
    }

    public class ProductServiceSettingsModel
    {
        public int NonStockSalesAccount { get; set; }
        public int NonStockExpenseAccount { get; set; }
        public int StockExpenseAccount { get; set; }
        public int ServiceSalesAccount { get; set; }
        public int ServiceExpenseAccount { get; set; }
        public bool AllowDuplicateItemNames { get; set; }
    }

    public class AccountingSettingsModel
    {
        public int ShowNegativeAssets { get; set; }
    }

    public class CurrencySettingsModel
    {
        public bool EnableMultiCurrency { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
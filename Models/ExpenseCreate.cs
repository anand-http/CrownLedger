using System;
using System.Collections.Generic;

namespace fintech.Models
{
    public class ExpenseCreate
    {
        public string RecordType { get; set; } // "Expense", "RecurringExpense", "BulkBooking"
        public string ExpenseType { get; set; } // "Goods" or "Service"
        public string HSNCode { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ExpenseLedgerId { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string Description { get; set; }
        public decimal? ExpenseAmount { get; set; }
        public int? VendorId { get; set; }
        public string PaidFrom { get; set; }
        public bool GstApplicable { get; set; }
        public int? GstTaxCode { get; set; }
        public decimal? TaxAmount { get; set; }
        public string ItcAvailable { get; set; }
        public bool TdsChargeable { get; set; }
        public int? TdsCode { get; set; }
        public decimal? TdsAmount { get; set; }
        public bool SaveAsDraft { get; set; }
        // For Recurring Expense
        public string Frequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // For Bulk Booking
        public List<BulkBookingItem> BulkBookingItems { get; set; }

        public ExpenseCreate()
        {
            BulkBookingItems = new List<BulkBookingItem>();
        }
    }

    public class BulkBookingItem
    {
        public DateTime? Date { get; set; }
        public int? ExpenseLedgerId { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string AmountCurrency { get; set; }
        public decimal? TaxAmount { get; set; }
        public string TaxCurrency { get; set; }
        public int? VendorId { get; set; }
        public string PaidFrom { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
    }

    public class SaveExpenseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
    }
}
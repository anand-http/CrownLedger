

namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class PurchasesRecExpenseHistoryFullResult
    {
        public PurchasesRecExpenseHistoryFullResult()
        {
            PurchasesRecExpenseHistory = new List<PurchasesRecExpenseHistoryList>();
        }

        public List<PurchasesRecExpenseHistoryList> PurchasesRecExpenseHistory { get; set; }
    }

    public class PurchasesRecExpenseHistoryList
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VendorName { get; set; }
        public string Frequency { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public int? TaxAmount { get; set; }
        public string GstApplicable { get; set; }
        public string ITCAvailable { get; set; }
        public int? TdsAmount { get; set; }
        public string Currency { get; set; }
        public string LedgerAccount { get; set; }
        public decimal? ExpenseAmount { get; set; }
        public string PaidFrom { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? Records { get; set; }
        public int? ExpenseId { get; set; }
        public int? HSNCode { get; set; }

    }
}


namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class PurchasesExpenseHistoryFullResult
    {
        public PurchasesExpenseHistoryFullResult()
        {
            PurchasesExpenseHistory = new List<PurchasesExpenseHistoryList>();
        }

        public List<PurchasesExpenseHistoryList> PurchasesExpenseHistory { get; set; }
    }

    public class PurchasesExpenseHistoryList
    {
        public DateTime? ExpenseDate { get; set; }
        public string VendorName { get; set; }
        public string ExpenseType { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string LedgerAccount { get; set; }
        public string Branch { get; set; }
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
using System.Collections.Generic;
using fintech;

namespace fintech.Models
{
    public class BankAccountViewModel
    {
        public List<usp_GetBankList_Result> Banks { get; set; }
        public List<GetBankAccounts_Result> Accounts { get; set; }
    }
}

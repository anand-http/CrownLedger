using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace fintech.Models
{
    public class BankAccountCreate
    {
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string SwiftCode { get; set; }
        public string SortCode { get; set; }
    }

}
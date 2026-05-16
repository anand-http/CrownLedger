using fintech;
using System.Collections.Generic;

namespace fintech.Models
{
    public class TaxDetailsViewModel
    {
        public mst_Tax NewTax { get; set; }
        public IEnumerable<mst_Tax> TaxList { get; set; }
        public List<mst_Tax> GSTList { get; set; } = new List<mst_Tax>();
        public List<mst_Tax> TDSList { get; set; } = new List<mst_Tax>();
        public List<mst_Tax> OtherList { get; set; } = new List<mst_Tax>();
    }
}
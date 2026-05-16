using System;
using System.ComponentModel.DataAnnotations;

namespace fintech.Models
{
    public class BranchViewModel
    {
        public int? BranchId { get; set; }
        [Required]
        public string BranchCode { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string GST { get; set; }
        public string Division { get; set; }
        public string BranchAddress1 { get; set; }
        public string BranchAddress2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public bool Active { get; set; }
        public string ActionType { get; set; }
    }
}
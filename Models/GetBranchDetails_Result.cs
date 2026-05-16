namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class GetBranchDetails_Result
    {
        public GetBranchDetails_Result()
        {
            BranchDetail = new List<BranchDetailList>();
        }

        public List<BranchDetailList> BranchDetail { get; set; }
    }

    public class BranchDetailList
    {
        public int Branch_ID { get; set; }
        public int Branch_CityId { get; set; }
        public string Branch_Code { get; set; }
        public string Branch_Desc { get; set; }
        public string GSTNO { get; set; }
        public string City { get; set; }
        public bool Active { get; set; }
        public string Branch_Address1 { get; set; }
        public string Branch_Address2 { get; set; }
        public string Branch_Address3 { get; set; }
        public int Branch_DivID { get; set; }
        public bool Branch_Group { get; set; }
        public bool Branch_Private { get; set; }
        public string Branch_Div { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int Records { get; set; }
    }
}
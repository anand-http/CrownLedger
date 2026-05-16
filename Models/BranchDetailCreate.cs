
namespace fintech.Models
{
    public class BranchDetailCreate
    {
        public int Branch_ID { get; set; }
        public string Branch_Desc { get; set; }
        public string ZipCode { get; set; }
        public int? CityId { get; set; }
        public bool Branch_Private { get; set; }
        public bool Branch_Group { get; set; }
        public string GSTNO { get; set; }
        public string Branch_Address1 { get; set; }
        public string Branch_Address2 { get; set; }
        public string Branch_Address3 { get; set; }
        public bool Active { get; set; }
        public int Branch_DivID { get; set; }

    }

}
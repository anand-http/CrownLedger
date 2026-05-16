using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fintech.Models
{
    public class LicenseModel
    {
        [Serializable]
        public class LicenceModel
        {
            public Header Headers { get; set; }
            public List<Entity_LicValid> Entity_LicValids { get; set; }
            public List<Entity_LicPaymentHistory> Entity_LicPaymentHistorys { get; set; }
        }
        [Serializable]
        public class Header
        {
            public int EntityId { get; set; }
            public string Entity_Code { get; set; }
            public string Entity_Name { get; set; }
            public decimal Entity_LicPrice { get; set; }
            public string Entity_Currency { get; set; }
            public string Entity_Ver { get; set; }
        }
        [Serializable]
        public class Entity_LicValid
        {
            public int Entity_Lic { get; set; }
            public string Entity_LicValidTill { get; set; }
        }
        [Serializable]
        public class Entity_LicPaymentHistory
        {
            public int Entity_Id { get; set; }
            public int Entity_Lic { get; set; }
            public string Entity_LicValidTill { get; set; }
            public string Entity_TransStatus { get; set; }
            public string CreateDate { get; set; }
            public int Entity_Month { get; set; }
            public string Entity_StartDate { get; set; }
            public decimal Entity_AmountPaid { get; set; }
        }
    }
}
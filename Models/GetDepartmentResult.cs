namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class GetDepartmentResult
    {
        public GetDepartmentResult()
        {
            DepartmentDetail = new List<DepartmenList>();
        }

        public List<DepartmenList> DepartmentDetail { get; set; }
    }

    public class DepartmenList
    {
        public int Deptmt_ID { get; set; }
        public string Deptmt_Code { get; set; }
        public string Deptmt_Desc { get; set; }
        public bool Deptmt_Private { get; set; }
        public bool Deptmt_Group { get; set; }
        public bool Active { get; set; }
        public int Records { get; set; }
        public byte[] timestamp { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace fintech.Models
{
    public class ConfigurationModel
    {
        public class Department
        {
            public int Deptmt_ID { get; set; }
            public string Deptmt_Code { get; set; }
            public string Deptmt_Desc { get; set; }
            public bool Deptmt_Private { get; set; }
            public bool Deptmt_Group { get; set; }
            public DateTime CreateDate { get; set; }
            public int CreatedBy { get; set; }
            public DateTime ChangeDate { get; set; }
            public int ChangeBy { get; set; }
            public bool Active { get; set; }
            public int Entity_ID { get; set; }
            public byte[] timestamp { get; set; }
        }

        public class PeriodModel
        {
            public int DocumentId { get; set; }
            //public string CalendarType { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string Period { get; set; }
            public bool IsActive { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedBy { get; set; }
            public int TotalRecord { get; set; }
        }

        public class SavePeriodModel
        {
            public int DocumentId { get; set; }
            public string CalendarType { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string Period { get; set; }
            public bool IsActive { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedBy { get; set; }
            public int TotalRecord { get; set; }
            public int? Entity_ID { get; set; }
            public byte[] timestamp { get; set; }
            public int? Login_ID { get; set; }
        }

        public class GetbusscalenResult
        {
            public GetbusscalenResult()
            {
                bussCalDetail = new List<PeriodModel>();
            }

            public List<PeriodModel> bussCalDetail { get; set; }
        }

        public class DocumentSequence
        {
            public int SeriesId { get; set; }
            public string SeriesName { get; set; }
            public int FiscalYear { get; set; }
            public string TypeName { get; set; }
            public string Prefix { get; set; }
            public string Value { get; set; }
            public string Restarting_Number { get; set; }
            public string Preview => string.Format("{0}{1}{2}", Prefix, Value, Restarting_Number);
        }

        public class DocumentSequenceResult
        {
            public List<DocumentSequence> Series { get; set; } = new List<DocumentSequence>();
        }
    }
}

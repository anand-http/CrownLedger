using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fintech.Models
{
    public class PasswordComplexity
    {
        public class PasswordComplexityModel
        {
            public Int32 LoginID { get; set; }
            public Boolean Active { get; set; }
            public long PassComp_ID { get; set; }
            public int PassComp_MinAge { get; set; }
            public int PassComp_MaxAge { get; set; }
            public int PassComp_HistorySize { get; set; }
            public int PassComp_ExpiryDays { get; set; }
            public int PassComp_MinLength { get; set; }
            public int PassComp_MaxLength { get; set; }
            public string PasswordComplexity_AppliesTo { get; set; }
            public int PassComp_MaxLoginAttempt { get; set; }
            public int PassComp_AccLockThreshold { get; set; }
            public int PassComp_IdleTimeout { get; set; }
            public int PassComp_AllowedSplCharlength { get; set; }
            public string PassComp_AllowedSplCharacters { get; set; }
            public bool PassComp_IncludeUppercase { get; set; }
            public bool PassComp_IncludeLowercase { get; set; }
            public bool PassComp_IncludeDigit { get; set; }
            public bool PassComp_IncludeSpecialCharacter { get; set; }
            public bool PassComp_EnforcePasswordChangeFirstLogin { get; set; }
            public bool PassComp_EnforcePolicyForAll { get; set; }
            public DateTime CreateDate { get; set; }
            public long CreatedBy { get; set; }
            public DateTime ChangeDate { get; set; }
            public long ChangeBy { get; set; }
            public bool ShowReminder { get; set; }
            public int RemainingDays { get; set; }
            public Int32 Entity_ID { get; set; }
            public int start { get; set; }
            public int length { get; set; }

            public string orderby { get; set; }
            public string search { get; set; }
        }
    }
}
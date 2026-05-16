// ============================================================
// GridActionModels.cs  — put in your Models folder
// ============================================================

namespace YourApp.Models
{
    public class GridActionRequest
    {
        public string ModuleType { get; set; }   // Estimate | Invoice | SalesOrder | CreditNote
        public int[]  Ids        { get; set; }   // selected row IDs — works for 1 to N rows
    }

    public class GridActionEmailRequest : GridActionRequest
    {
        public string ToEmail { get; set; }
        public string EmailBody { get; set; }
        public string EmailSub { get; set; }
    }
}

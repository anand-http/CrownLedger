using fintech;
using fintech.DAL;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using static fintech.Models.ConfigurationModel;

namespace fintech.BAL
{
    public class ConfigurationBAL
    {
        private ConfigurationDAL dal = new ConfigurationDAL();

        public GetBranchDetails_Result GetBranchesList(int? entityId,int id, int start, int length, string orderby)
        {
            return dal.GetBranchesList(entityId,id, start, length, orderby);
        }

        public bool UpdateBranch(bool isChecked, int? branchId, bool isDelete = false)
        {
            return dal.UpdateBranch(isChecked, branchId, isDelete);
        }
        public bool DeleteUser(int? userId)
        {
            return dal.DeleteUser(userId);
        }
        public bool UpdateDepartment(bool isChecked, int? departmentId, bool isDelete = false)
        {
            return dal.UpdateDepartment(isChecked, departmentId, isDelete);
        }
        public GetBranchDetails_Result GetBranchesList(int? entityId, int start, int length, string orderby)
        {
            return dal.GetBranchesList(entityId, start, length, orderby);
        }
        public bool SaveBranch(BranchDetailCreate model, int entityId, int? loginId, out string messageCode, string actionType)
        {
            return dal.SaveBranch(model, entityId, loginId, out messageCode, actionType);
        }
        public bool SaveUserAccess(SaveUserAccessRequest model, int? entityId, int? loginId, out string messageCode)
        {
            return dal.SaveUserAccess(model, entityId, loginId, out messageCode);
        }

        public GetDepartmentResult GetDepartmentList(int? entityId, int start, int length, string orderby, string search)
        {
            return dal.GetDepartmentList(entityId, start,length,orderby,search);
        }

        public DocumentSequenceResult GetNumberSeriesList()
        {
            return dal.GetNumberSeriesList();
        }

        public GetbusscalenResult GetBussinessCalendarList(int? entityId, int start, int length, string orderby, string search)
        {
            return dal.GetBussinessCalendarList(entityId, start, length, orderby, search);
        }
        public bool DepartmentAction(Department model,int?entityId,int? loginId, out string messageCode)
        {
            return dal.DepartmentAction(model, entityId,loginId, out messageCode);
        }
        // ── Save (Insert / Update) ──
        public string SavePeriod(SavePeriodModel objPeriod)
        {
            try
            {
                // ── Validate Calendar Type ──
                var validTypes = new List<string> { "Custom", "Yearly", "Quarterly", "HalfYearly", "Monthly" };
                //if (string.IsNullOrWhiteSpace(objPeriod.CalendarType) || !validTypes.Contains(objPeriod.CalendarType))
                //    return "invalid_calendartype";

                // ── Validate From Date ──
                if (string.IsNullOrWhiteSpace(objPeriod.FromDate))
                    return "invalid_fromdate";

                // ── Validate To Date ──
                if (string.IsNullOrWhiteSpace(objPeriod.ToDate))
                    return "invalid_todate";

                // ── Validate date range ──
                DateTime fromDate, toDate;
                if (!DateTime.TryParse(objPeriod.FromDate, out fromDate))
                    return "invalid_fromdate";
                if (!DateTime.TryParse(objPeriod.ToDate, out toDate))
                    return "invalid_todate";
                if (toDate < fromDate)
                    return "invalid_daterange";

                // ── Validate Period ──
                if (string.IsNullOrWhiteSpace(objPeriod.Period))
                    return "invalid_period";

                // ── Check duplicate Period (ignore current record on edit) ──
                bool isDuplicate = dal.IsDuplicatePeriod(objPeriod.Period, objPeriod.DocumentId);
                if (isDuplicate)
                    return "duplicate";

                // ── Set audit fields ──
                if (objPeriod.DocumentId == 0)
                    objPeriod.CreatedBy = HttpContext.Current.Session["UserId"]?.ToString() ?? "System";
                else
                    objPeriod.ModifiedBy = HttpContext.Current.Session["UserId"]?.ToString() ?? "System";

                return dal.SavePeriod(objPeriod);
            }
            catch (Exception ex)
            {
                // Log exception here
                return "error";
            }
        }

        // ── Delete ──
        public string DeletePeriod(int documentId)
        {
            try
            {
                if (documentId <= 0)
                    return "invalid";

                return dal.DeletePeriod(documentId);
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public GetUserAccess_Result GetUserAccess(int? entityId,int start, int length)
        {
            return dal.GetUserAccess(entityId, start, length);
        }

        public void SendEmail(string toEmail, string emailsub, string emailbody)
        {

            // CHANGED: reading SMTP config manually from AppSettings
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string username = ConfigurationManager.AppSettings["FromMailUserId"];
            string password = ConfigurationManager.AppSettings["FromMailPassword"];
            string fromMail = ConfigurationManager.AppSettings["FromMailId"];

            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(fromMail);
                msg.To.Add(toEmail);
                msg.Subject = emailsub == null ? "Invite User": emailsub;
                msg.Body = emailbody == null ? "" : emailbody;
                msg.IsBodyHtml = true;

                // CHANGED: explicitly setting host, port, credentials
                using (var client = new SmtpClient(host, port))
                {
                    client.Credentials = new System.Net.NetworkCredential(username, password);
                    client.EnableSsl = true;   // AWS SES requires TLS on port 587
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(msg);
                }
            }
        }

        public SaveUserAccessRequest GetUserDetails(int userId, int?entityId,int start, int length)
        {
            try
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("Invalid User ID");
                }

                var dal = new ConfigurationDAL();
                var userDetails = dal.GetUserDetails(userId,entityId,0,0);

                if (userDetails == null)
                {
                    throw new Exception("User not found");
                }

                return userDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetUserDetails: " + ex.Message);
            }
        }


        public bool SaveTransactionSettings(TransactionSettingsRequest request, int? entityId, int? loginId, out string messageCode)
        {

            string customerAgingFrom = JoinList(request.CustomerSettings.AgingFrom);
            string customerAgingTo = JoinList(request.CustomerSettings.AgingTo);
            string vendorAgingFrom = JoinList(request.VendorSettings.AgingFrom);
            string vendorAgingTo = JoinList(request.VendorSettings.AgingTo);

            return dal.SaveTransactionSettings(request, entityId, loginId, out messageCode,customerAgingFrom, customerAgingTo,vendorAgingFrom, vendorAgingTo);
        }


        public TransactionSettingsResponse TransactionSettings(int? entityId,int?loginId)
        {
            return dal.TransactionSettings(entityId, loginId);
        }

        private string JoinList(List<string> list)
        => list != null ? string.Join("|", list) : string.Empty;
    }
}

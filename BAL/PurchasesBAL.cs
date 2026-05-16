using fintech.DAL;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static fintech.Models.CommonModel;
namespace fintech.BAL
{
    public class PurchasesBAL
    {
        private PurchasesDAL dal = new PurchasesDAL();

        public VendorFullResult GetVendorList(int? entityId, int start, int length, string orderby, string search, bool cond, string statusFilter)
        {
            return dal.GetVendorList(entityId, start, length, orderby, search, cond, statusFilter);
        }
        public bool UpdateVendorStatus(int clientId, string status)
        {
            return dal.UpdateVendorStatus(clientId, status);
        }
        public UpsertVendor_Result UpsertVendor(SaveVendorList_Result model)
        {
            return dal.UpsertVendor(model);
        }
        public void SaveContactDetails(long? partnerid, int entityid, List<VendorContact_Details> contacts)
        {
            dal.SaveContactDetails(partnerid, entityid, contacts);
        }
        public PurchasesOrderHistoryFullResult GetPurchasesOrderHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetPurchasesOrderHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }

        public PurchasesBillHistoryFullResult GetBillHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetPurchasesBillHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }
        public PurchasesRecBillHistoryFullResult GetRecBillHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetPurchasesRecBillHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }
        public PurchasesExpenseHistoryFullResult GetExpenseHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetExpenseHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }
        public PurchasesRecExpenseHistoryFullResult GetRecExpenseHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetRecExpenseHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }
        public PurchasesCreditNotesHistoryFullResult GetPurchasesCreditNotesHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetPurchasesCreditNotesHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }

        public bool SavePurchaseOrder(PurchaseOrderCreate model, int entityId, int? loginId, out string messageCode)
        {
            return new PurchasesDAL().SavePurchaseOrder(model, entityId, loginId, out messageCode);
        }
        public bool SavePurchaseBills(PurchaseBillsCreate model, int entityId, int? loginId, out string messageCode)
        {
            return new PurchasesDAL().SavePurchaseBills(model, entityId, loginId, out messageCode);
        }
        public bool SavePurchaseCreditNote(PurchaseCreditNoteCreate model, int entityId, int? loginId, out string messageCode)
        {
            return new PurchasesDAL().SavePurchaseCreditNote(model, entityId, loginId, out messageCode);
        }
        public bool SavePurchaseRecurringBills(PurchaseRecurringBillsCreate model, int entityId, int? loginId, out string messageCode)
        {
            return new PurchasesDAL().SavePurchaseRecurringBills(model, entityId, loginId, out messageCode);
        }

        public bool SaveExpense(ExpenseCreate model, int entityId, int loginId, string recordType, out string messageCode)
        {
            try
            {
                messageCode = "";
                var dal = new PurchasesDAL();
                return dal.SaveExpense(model, entityId, loginId, recordType, out messageCode);
            }
            catch (Exception ex)
            {
                messageCode = "E0003";
                return false;
            }
        }

        public ImportResult ImportVendor(List<ImportVendor> vendors, List<Contact_Details> contacts, int? entityId)
        {
            var vendorXml = BuildCustomerXml(vendors, entityId);
            var contactXml = BuildContactXml(vendors, contacts);

            var dal = new fintech.DAL.PurchasesDAL();
            var success = dal.ImportVendor(vendorXml, contactXml,entityId);

            return new ImportResult
            {
                Success = success,
            };
        }

        private string BuildCustomerXml(List<ImportVendor> vendors, int? entityId)
        {
            var xml = new StringBuilder();
            xml.Append("<Vendors>");
            foreach (var c in vendors)
            {
                xml.Append("<Vendor>");
                xml.AppendFormat("<Entity_ID>{0}</Entity_ID>", entityId);
                xml.AppendFormat("<VendorName>{0}</VendorName>", Sanitize(c.Name));
                xml.AppendFormat("<Partner_Code>{0}</Partner_Code>", Sanitize(c.Partner_Code));
                xml.AppendFormat("<Email>{0}</Email>", Sanitize(c.Email));
                xml.AppendFormat("<Telephone>{0}</Telephone>", Sanitize(c.Telephone));
                xml.AppendFormat("<Website>{0}</Website>", Sanitize(c.Website));
                xml.AppendFormat("<Partner_Status>{0}</Partner_Status>", Sanitize(c.PartnerStatus));
                xml.AppendFormat("<Country>{0}</Country>", c.Country);
                xml.AppendFormat("<State>{0}</State>", c.State);
                xml.AppendFormat("<Department>{0}</Department>", c.Department);
                xml.AppendFormat("<DefaultCurrency>{0}</DefaultCurrency>", c.DefaultCurrency);
                xml.AppendFormat("<Address1>{0}</Address1>", Sanitize(c.Address1));
                xml.AppendFormat("<Address2>{0}</Address2>", Sanitize(c.Address2));
                xml.AppendFormat("<CorrespondenceAddress1>{0}</CorrespondenceAddress1>", Sanitize(c.CorrespondenceAddress1));
                xml.AppendFormat("<CorrespondenceAddress2>{0}</CorrespondenceAddress2>", Sanitize(c.CorrespondenceAddress2));
                xml.AppendFormat("<City>{0}</City>", c.City);
                xml.AppendFormat("<City2>{0}</City2>", Sanitize(c.City2));
                xml.AppendFormat("<State2>{0}</State2>", Sanitize(c.State2));
                xml.AppendFormat("<Country2>{0}</Country2>", Sanitize(c.Country2));
                xml.AppendFormat("<ZipCode>{0}</ZipCode>", Sanitize(c.ZipCode));
                xml.AppendFormat("<ZipCode2>{0}</ZipCode2>", Sanitize(c.ZipCode2));
                xml.AppendFormat("<BankCurrency>{0}</BankCurrency>", Sanitize(c.BankCurrency));
                xml.AppendFormat("<AccountNumber>{0}</AccountNumber>", Sanitize(c.AccountNumber));
                xml.AppendFormat("<Payee>{0}</Payee>", Sanitize(c.Payee));
                xml.AppendFormat("<BankName>{0}</BankName>", Sanitize(c.BankName));
                xml.AppendFormat("<IBAN_IFSC>{0}</IBAN_IFSC>", Sanitize(c.IBAN_IFSC));
                xml.AppendFormat("<SWIFTCode>{0}</SWIFTCode>", Sanitize(c.SWIFTCode));
                xml.AppendFormat("<SORTCode>{0}</SORTCode>", Sanitize(c.SORTCode));
                xml.AppendFormat("<IsDefaultBank>{0}</IsDefaultBank>", c.IsDefaultBank.HasValue ? (c.IsDefaultBank.Value ? 1 : 0).ToString() : "");
                xml.AppendFormat("<GST_VAT>{0}</GST_VAT>", Sanitize(c.GST_VAT));
                xml.AppendFormat("<PAN>{0}</PAN>", Sanitize(c.PAN));
                xml.AppendFormat("<TaxCode>{0}</TaxCode>", Sanitize(c.TaxCode));
                xml.AppendFormat("<BillPercentage>{0}</BillPercentage>", c.BillPercentage ?? 0);
                xml.AppendFormat("<DueDateBasedOn>{0}</DueDateBasedOn>", Sanitize(c.DueDateBasedOn));
                xml.AppendFormat("<CreditDays>{0}</CreditDays>", c.CreditDays ?? 0);
                xml.AppendFormat("<CreditLimits>{0}</CreditLimits>", c.CreditLimits ?? 0);
                xml.AppendFormat("<Status>{0}</Status>", Sanitize(c.Status));
                xml.Append("</Vendor>");
            }
            xml.Append("</Vendors>");
            return xml.ToString();
        }

        private string BuildContactXml(List<ImportVendor> vendors, List<Contact_Details> contacts)
        {
            // build lookup: Client_ID → Client_Code for linking
            var clientIdToCode = vendors.ToDictionary(
                x => x.Partner_Code
            );

            var xml = new StringBuilder();
            xml.Append("<Contacts>");
            foreach (var contact in contacts)
            {
                string clientCode = string.Empty;
                xml.Append("<Contact>");
                xml.AppendFormat("<Partner_Code>{0}</Partner_Code>", Sanitize(clientCode));
                xml.AppendFormat("<Contact_Source>{0}</Contact_Source>", Sanitize(contact.Contact_Source));
                xml.AppendFormat("<Contact_ContypID>{0}</Contact_ContypID>", contact.Contact_ContypID ?? 0);
                xml.AppendFormat("<Contact_Name>{0}</Contact_Name>", Sanitize(contact.Contact_Name));
                xml.AppendFormat("<Contact_Email>{0}</Contact_Email>", Sanitize(contact.Contact_Email));
                xml.AppendFormat("<Contact_Fax>{0}</Contact_Fax>", Sanitize(contact.Contact_Fax));
                xml.AppendFormat("<Contact_Phone>{0}</Contact_Phone>", Sanitize(contact.Contact_Phone));
                xml.AppendFormat("<Contact_Mobile>{0}</Contact_Mobile>", Sanitize(contact.Contact_Mobile));
                xml.AppendFormat("<Designation>{0}</Designation>", Sanitize(contact.Designation));
                xml.AppendFormat("<Contact_Type>{0}</Contact_Type>", Sanitize(contact.Contact_Type));
                xml.Append("</Contact>");
            }
            xml.Append("</Contacts>");
            return xml.ToString();
        }

        // Escapes special XML characters to prevent malformed XML
        private string Sanitize(string val)
        {
            if (string.IsNullOrEmpty(val)) return string.Empty;
            return val.Replace("&", "&amp;")
                      .Replace("<", "&lt;")
                      .Replace(">", "&gt;")
                      .Replace("\"", "&quot;")
                      .Replace("'", "&apos;");
        }

    }
}
using fintech.DAL;
using fintech.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static fintech.Models.CommonModel;

namespace fintech.BAL
{
    public class SalesBAL
    {
        private SalesDAL dal = new SalesDAL();

        public CustomerFullResult GetCustomerList(int? entityId, int start, int length, string orderby, string search, string statusFilter)
        {
            return dal.GetCustomerList(entityId, start, length, orderby, search, statusFilter);
        }
        public bool UpdateCustomerStatus(int clientId, string status)
        {
            return dal.UpdateCustomerStatus(clientId, status);
        }

        public bool SaveSalesOrder(SalesOrderItemModel model, int entityId, out string messageCode)
        {
            return new SalesDAL().SaveSalesOrder(model, entityId, out messageCode);
        }
        public SaveResult SaveEstimate(SaleEstimateModel request)
        {
            var dal = new fintech.DAL.SalesDAL();
            return dal.SaveEstimate(request);
        }
        public SaveResult SaveOrder(SaleEstimateModel request)
        {
            var dal = new fintech.DAL.SalesDAL();
            return dal.SaveEstimate(request);
        }
        public SaveResult SaveInvoice(SaleInvoice request)
        {
            var dal = new fintech.DAL.SalesDAL();
            return dal.SaveInvoice(request);
        }
        public SaveResult SaveRecurringInv(SaleRecInv request)
        {
            var dal = new fintech.DAL.SalesDAL();
            return dal.SaveRecurringInv(request);
        }
        public SaveResult SaveCreditNote(SaleCreditNote request)
        {
            var dal = new fintech.DAL.SalesDAL();
            return dal.SaveCreditNotes(request);
        }

        // ── Sales Estimate ──
        public SalesEstimateHistoryFullResult GetSalesEstimateHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            return dal.GetSalesEstimateHistory(statusFilter, entityId, start, length, orderby, search, startDate, endDate);
        }

        // ── Sales Invoice ──

        public SalesInvoiceHistoryFullResult GetSalesInvoiceHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            return dal.GetSalesInvoiceHistory(statusFilter, entityId, start, length, orderby, search, startDate, endDate);
        }
        // ── Sales Order ──
        public SalesOrderHistoryFullResult GetSalesOrderHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            return dal.GetSalesOrderHistory(entityId, start, length, orderby, search, statusFilter, startDate, endDate);
        }

        // ── Credit Note ──
        public SalesCreditNoteHistoryFullResult GetSalesCreditNoteHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            return dal.GetSalesCreditNoteHistory(statusFilter, entityId, start, length, orderby, search, startDate, endDate);
        }

        //--Recurring Invoice-- 

        public SalesRecInvHistoryFullResult GetSalesRecInvHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            return dal.GetSalesRecInvHistory(statusFilter, entityId, start, length, orderby, search, startDate, endDate);
        }
        public UpsertCustomer_Result UpsertCustomer(SaveCustomerList_Result model)
        {
            return dal.UpsertCustomer(model);
        }

        public void SaveContactDetails(long? clientId, int entityid, List<Contact_Details> contacts)
        {
            dal.SaveContactDetails(clientId, entityid, contacts);
        }

        public ImportResult ImportCustomers(List<ImportCustomer> customers, List<Contact_Details> contacts, int? entityId)
        {
            var customerXml = BuildCustomerXml(customers, entityId);
            var contactXml = BuildContactXml(customers, contacts);

            var dal = new fintech.DAL.SalesDAL();
            bool success = dal.ImportCustomers(customerXml, contactXml);

            return new ImportResult
            {
                Success = success,
            };
        }

        private string BuildCustomerXml(List<ImportCustomer> customers, int? entityId)
        {
            var xml = new StringBuilder();
            xml.Append("<Customers>");
            foreach (var c in customers)
            {
                xml.Append("<Customer>");
                xml.AppendFormat("<Entity_ID>{0}</Entity_ID>", entityId);
                xml.AppendFormat("<CustomerName>{0}</CustomerName>", Sanitize(c.CustomerName));
                xml.AppendFormat("<Client_Code>{0}</Client_Code>", Sanitize(c.Client_Code));
                xml.AppendFormat("<Email>{0}</Email>", Sanitize(c.Email));
                xml.AppendFormat("<Telephone>{0}</Telephone>", Sanitize(c.Telephone));
                xml.AppendFormat("<Website>{0}</Website>", Sanitize(c.Website));
                xml.AppendFormat("<Client_Status>{0}</Client_Status>", Sanitize(c.Client_Status));
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
                xml.Append("</Customer>");
            }
            xml.Append("</Customers>");
            return xml.ToString();
        }

        private string BuildContactXml(List<ImportCustomer> customers, List<Contact_Details> contacts)
        {
            // build lookup: Client_ID → Client_Code for linking
            var clientIdToCode = customers.ToDictionary(
                x => x.Client_Code
            );

            var xml = new StringBuilder();
            xml.Append("<Contacts>");
            foreach (var contact in contacts)
            {
                string clientCode = string.Empty;
                xml.Append("<Contact>");
                xml.AppendFormat("<Client_Code>{0}</Client_Code>", Sanitize(clientCode));
                xml.AppendFormat("<Contact_Source>{0}</Contact_Source>", Sanitize(contact.Contact_Source));
                xml.AppendFormat("<Contact_ContypID>{0}</Contact_ContypID>", contact.Contact_ContypID ?? 0);
                xml.AppendFormat("<Contact_Name>{0}</Contact_Name>", Sanitize(contact.Contact_Name));
                xml.AppendFormat("<Contact_Email>{0}</Contact_Email>", Sanitize(contact.Contact_Email));
                xml.AppendFormat("<Contact_Fax>{0}</Contact_Fax>", Sanitize(contact.Contact_Fax));
                xml.AppendFormat("<Contact_Phone>{0}</Contact_Phone>", Sanitize(contact.Contact_Phone));
                xml.AppendFormat("<Contact_Mobile>{0}</Contact_Mobile>", Sanitize(contact.Contact_Mobile));
                xml.AppendFormat("<Designation>{0}</Designation>", Sanitize(contact.Designation));
                xml.AppendFormat("<Contact_Type_desc>{0}</Contact_Type_desc>", Sanitize(contact.Contact_Type_desc));
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

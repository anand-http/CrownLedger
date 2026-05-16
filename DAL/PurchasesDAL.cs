using fintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace fintech.DAL
{
    public class PurchasesDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;


        public VendorFullResult GetVendorList(int? entityId, int start, int length, string orderby, string search, bool cond, string status)
        {
            var result = new VendorFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Partner.SP_GetPartnerList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Entity_ID", 10);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@length", length);
                cmd.Parameters.AddWithValue("@orderby", string.IsNullOrEmpty(orderby) ? "Order By Partner_ID asc" : orderby);
                cmd.Parameters.AddWithValue("@search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cond", cond);
                cmd.Parameters.AddWithValue("@StatusFilter", status ?? "A");

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    do
                    {
                        if (reader.FieldCount == 0) continue;

                        string firstCol = reader.GetName(0);

                        // ── Result Set 1: paging CTE — first col is "Row"
                        if (firstCol == "Row")
                        {
                            while (reader.Read())
                            {
                                result.Vendors.Add(new GetVendorList_Result
                                {
                                    Partner_Id = reader["Partner_Id"] == DBNull.Value ? (long?)null : TryParseLong(reader["Partner_Id"]),
                                    Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                                    Partner_Code = reader["Partner_Code"] == DBNull.Value ? null : reader["Partner_Code"].ToString(),
                                    Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                                    Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                                    PartnerStatus = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                                    Website = reader["Website"] == DBNull.Value ? null : reader["Website"].ToString(),
                                    Country = reader["Country"] == DBNull.Value ? null : reader["Country"].ToString(),
                                    Contry_Id = reader["Contry_Id"] == DBNull.Value ? null : TryParseInt(reader["Contry_Id"]),
                                    State_Id = reader["State_Id"] == DBNull.Value ? null : TryParseInt(reader["State_Id"]),
                                    DepartMentId = reader["DepartMentId"] == DBNull.Value ? null : TryParseInt(reader["DepartMentId"]),
                                    Currency_Id = reader["Currency_Id"] == DBNull.Value ? null : TryParseInt(reader["Currency_Id"]),
                                    City_Id = reader["City_Id"] == DBNull.Value ? null : TryParseInt(reader["City_Id"]),
                                    State = reader["State"] == DBNull.Value ? null : reader["State"].ToString(),
                                    Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString(),
                                    DefaultCurrency = reader["DefaultCurrency"] == DBNull.Value ? null : reader["DefaultCurrency"].ToString(),
                                    PAddress1 = reader["Address1"] == DBNull.Value ? null : reader["Address1"].ToString(),
                                    PAddress2 = reader["Address2"] == DBNull.Value ? null : reader["Address2"].ToString(),
                                    PCity = reader["City"] == DBNull.Value ? null : reader["City"].ToString(),
                                    PCountry = reader["Country"] == DBNull.Value ? null : reader["Country"].ToString(),
                                    ZipCode = reader["ZipCode"] == DBNull.Value ? null : reader["ZipCode"].ToString(),
                                    CAddress1 = reader["CorrespondenceAddress1"] == DBNull.Value ? null : reader["CorrespondenceAddress1"].ToString(),
                                    CAddress2 = reader["CorrespondenceAddress2"] == DBNull.Value ? null : reader["CorrespondenceAddress2"].ToString(),
                                    CCity = reader["City2"] == DBNull.Value ? null : reader["City2"].ToString(),
                                    CCountry = reader["Country"] == DBNull.Value ? null : reader["Country"].ToString(),
                                    ZipCode2 = reader["ZipCode2"] == DBNull.Value ? null : reader["ZipCode2"].ToString(),
                                    GST_VAT = reader["GST_VAT"] == DBNull.Value ? null : reader["GST_VAT"].ToString(),
                                    TaxCode = reader["TaxCode"] == DBNull.Value ? null : reader["TaxCode"].ToString(),
                                    PAN = reader["PAN"] == DBNull.Value ? null : reader["PAN"].ToString(),
                                    AccountNumber = reader["AccountNumber"] == DBNull.Value ? null : reader["AccountNumber"].ToString(),
                                    Payee = reader["Payee"] == DBNull.Value ? null : reader["Payee"].ToString(),
                                    BankName = reader["BankName"] == DBNull.Value ? null : reader["BankName"].ToString(),
                                    IBAN_IFSC = reader["IBAN_IFSC"] == DBNull.Value ? null : reader["IBAN_IFSC"].ToString(),
                                    SWIFTCode = reader["SwiftCode"] == DBNull.Value ? null : reader["SwiftCode"].ToString(),
                                    SORTCode = reader["SortCode"] == DBNull.Value ? null : reader["SortCode"].ToString(),
                                    Records = reader["Records"] == DBNull.Value ? (long?)null : TryParseLong(reader["Records"]),
                                    AmountOwed = reader["AmountOwed"] == DBNull.Value ? null : reader["AmountOwed"].ToString(),
                                    UnusedCredit = reader["UnusedCredit"] == DBNull.Value ? null : reader["UnusedCredit"].ToString(),
                                });
                            }
                        }

                        // ── Result Set 2: Address — first col is "PAdd_Address_ID"
                        else if (firstCol == "PAdd_Address_ID")
                        {
                            while (reader.Read())
                            {
                                result.Addresses.Add(new VendorAddress_Result
                                {
                                    PAdd_Address_ID = reader["PAdd_Address_ID"] == DBNull.Value ? (int?)null : TryParseInt(reader["PAdd_Address_ID"]),
                                    PAdd_RStreet1 = reader["PAdd_RStreet1"] == DBNull.Value ? null : reader["PAdd_RStreet1"].ToString(),
                                    PAdd_RStreet2 = reader["PAdd_RStreet2"] == DBNull.Value ? null : reader["PAdd_RStreet2"].ToString(),
                                    PAdd_RStreet3 = reader["PAdd_RStreet3"] == DBNull.Value ? null : reader["PAdd_RStreet3"].ToString(),
                                    PAdd_RCity = reader["PAdd_RCity"] == DBNull.Value ? null : reader["PAdd_RCity"].ToString(),
                                    PAdd_RCountry = reader["PAdd_RCountry"] == DBNull.Value ? null : reader["PAdd_RCountry"].ToString(),
                                    PAdd_RZip = reader["PAdd_RZip"] == DBNull.Value ? null : reader["PAdd_RZip"].ToString(),
                                    PAdd_RPhone = reader["PAdd_RPhone"] == DBNull.Value ? null : reader["PAdd_RPhone"].ToString(),
                                    PAdd_RFax = reader["PAdd_RFax"] == DBNull.Value ? null : reader["PAdd_RFax"].ToString(),
                                    PAdd_CStreet1 = reader["PAdd_CStreet1"] == DBNull.Value ? null : reader["PAdd_CStreet1"].ToString(),
                                    PAdd_CStreet2 = reader["PAdd_CStreet2"] == DBNull.Value ? null : reader["PAdd_CStreet2"].ToString(),
                                    PAdd_CStreet3 = reader["PAdd_CStreet3"] == DBNull.Value ? null : reader["PAdd_CStreet3"].ToString(),
                                    PAdd_CCity = reader["PAdd_CCity"] == DBNull.Value ? null : reader["PAdd_CCity"].ToString(),
                                    PAdd_CCountry = reader["PAdd_CCountry"] == DBNull.Value ? null : reader["PAdd_CCountry"].ToString(),
                                    PAdd_CZip = reader["PAdd_CZip"] == DBNull.Value ? null : reader["PAdd_CZip"].ToString(),
                                    PAdd_CPhone = reader["PAdd_CPhone"] == DBNull.Value ? null : reader["PAdd_CPhone"].ToString(),
                                    PAdd_CFax = reader["PAdd_CFax"] == DBNull.Value ? null : reader["PAdd_CFax"].ToString(),
                                    PAdd_REmail = reader["PAdd_REmail"] == DBNull.Value ? null : reader["PAdd_REmail"].ToString(),
                                    PAdd_CEmail = reader["PAdd_CEmail"] == DBNull.Value ? null : reader["PAdd_CEmail"].ToString(),
                                    Contact_Gender = reader["Contact_Gender"] == DBNull.Value ? null : reader["Contact_Gender"].ToString(),
                                    Contact_DOB = reader["Contact_DOB"] == DBNull.Value ? null : reader["Contact_DOB"].ToString(),
                                    Contact_BirthPlace = reader["Contact_BirthPlace"] == DBNull.Value ? null : reader["Contact_BirthPlace"].ToString(),
                                    Timestamp = reader["timestamp"] == DBNull.Value ? null : reader["timestamp"] as byte[],
                                });
                            }
                        }

                        // ── Result Set 3: Contacts — first col is "Contact_ID"
                        else if (firstCol == "Contact_ID")
                        {
                            while (reader.Read())
                            {
                                result.Contacts.Add(new VendorContact_Details
                                {
                                    Contact_ID = reader["Contact_ID"] == DBNull.Value ? (int?)null : TryParseInt(reader["Contact_ID"]),
                                    Contact_Source = reader["Contact_Source"] == DBNull.Value ? null : reader["Contact_Source"].ToString(),
                                    Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),
                                    Contact_SourceID = reader["Contact_SourceID"] == DBNull.Value ? (int?)null : TryParseInt(reader["Contact_SourceID"]),
                                    Contact_ContypID = reader["Contact_ContypID"] == DBNull.Value ? (int?)null : TryParseInt(reader["Contact_ContypID"]),
                                    Contact_Type = reader["Contacttype"] == DBNull.Value ? null : reader["Contacttype"].ToString(),
                                    Contact_Name = reader["Contact_Name"] == DBNull.Value ? null : reader["Contact_Name"].ToString(),
                                    Contact_Email = reader["Contact_Email"] == DBNull.Value ? null : reader["Contact_Email"].ToString(),
                                    Contact_Fax = reader["Contact_Fax"] == DBNull.Value ? null : reader["Contact_Fax"].ToString(),
                                    Contact_Phone = reader["Contact_Phone"] == DBNull.Value ? null : reader["Contact_Phone"].ToString(),
                                    Contact_Mobile = reader["Contact_Mobile"] == DBNull.Value ? null : reader["Contact_Mobile"].ToString(),
                                    Timestamp = reader["timestamp"] == DBNull.Value ? null : reader["timestamp"] as byte[],
                                });
                            }
                        }
                        else if (firstCol == "Partner_Id")
                        {
                            while (reader.Read())
                            {
                                result.PaymentTerms.Add(new VendorPaymentTerms
                                {
                                    Partner_Id = reader["Partner_Id"] == DBNull.Value ? (long?)null : TryParseLong(reader["Partner_Id"]),
                                    BillPercentage = reader["BillPercentage"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["BillPercentage"]),
                                    DueDateBasedOn = reader["DueDateBasedOn"] == DBNull.Value ? null : reader["DueDateBasedOn"].ToString(),
                                    CreditDays = reader["CreditDays"] == DBNull.Value ? (int?)null : TryParseInt(reader["CreditDays"]),
                                    CreditLimits = reader["CreditLimits"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["CreditLimits"]),
                                    IsDefaultBank = reader["IsDefaultBank"] == DBNull.Value ? (bool?)null : TryParseBool(reader["IsDefaultBank"]),
                                });
                            }
                        }

                    } while (reader.NextResult());
                }
            }

            return result;
        }
        public bool UpdateVendorStatus(int clientId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE CLIENT.mst_Client SET Client_Status = @Status WHERE Partner_Id = @Partner_Id", conn))
            {
                cmd.Parameters.AddWithValue("@Partner_Id", clientId);
                cmd.Parameters.AddWithValue("@Status", status);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public UpsertVendor_Result UpsertVendor(SaveVendorList_Result model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("PARTNER.Sp_mst_InsUpdPartnerGeneral", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Partner_Id", model.Partner_Id);
                cmd.Parameters.AddWithValue("@VendorName", model.Name);
                cmd.Parameters.AddWithValue("@Code", model.Partner_Code);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Telephone", model.Telephone);
                cmd.Parameters.AddWithValue("@Website", model.Website);
                cmd.Parameters.AddWithValue("@Country", model.Country);
                cmd.Parameters.AddWithValue("@State", model.State);
                cmd.Parameters.AddWithValue("@Department", model.Department);
                cmd.Parameters.AddWithValue("@DefaultCurrency", model.DefaultCurrency);
                cmd.Parameters.AddWithValue("@Address1", model.Address1);
                cmd.Parameters.AddWithValue("@Address2", model.Address2);
                cmd.Parameters.AddWithValue("@City", model.City);
                cmd.Parameters.AddWithValue("@ZipCode", model.ZipCode);
                cmd.Parameters.AddWithValue("@CorrespondenceAddress1", model.CorrespondenceAddress1);
                cmd.Parameters.AddWithValue("@CorrespondenceAddress2", model.CorrespondenceAddress2);
                cmd.Parameters.AddWithValue("@City2", model.City2);
                cmd.Parameters.AddWithValue("@State2", model.State2);
                cmd.Parameters.AddWithValue("@ZipCode2", model.ZipCode2);
                cmd.Parameters.AddWithValue("@GST_VAT", model.GST_VAT);
                cmd.Parameters.AddWithValue("@PAN", model.PAN);
                cmd.Parameters.AddWithValue("@TaxCode", model.TaxCode);
                cmd.Parameters.AddWithValue("@AccountNumber", model.AccountNumber);
                cmd.Parameters.AddWithValue("@BankName", model.BankName);
                cmd.Parameters.AddWithValue("@IBAN_IFSC", model.IBAN_IFSC);
                cmd.Parameters.AddWithValue("@SWIFTCode", model.SWIFTCode);
                cmd.Parameters.AddWithValue("@SORTCode", model.SORTCode);
                cmd.Parameters.AddWithValue("@IsDefaultBank", model.IsDefaultBank);
                cmd.Parameters.AddWithValue("@Status", model.PartnerStatus);

                var dt = new DataTable();
                dt.Columns.Add("BillPercentage", typeof(decimal));
                dt.Columns.Add("DueDateBasedOn", typeof(string));
                dt.Columns.Add("CreditDays", typeof(int));
                dt.Columns.Add("CreditLimits", typeof(decimal));
                dt.Columns.Add("IsDefaultBank", typeof(bool));
                foreach (var c in model.PaymentTerms)
                    dt.Rows.Add(c.BillPercentage, c.termsId, c.CreditDays, c.CreditLimits, c.IsDefaultBank);

                var param = cmd.Parameters.AddWithValue("@PaymentDetails", dt);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "CLIENT.TVP_PaymentDetails";
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UpsertVendor_Result
                        {
                            Partner_Id = Convert.ToInt32(reader["Partner_Id"]),
                            //StatusName = reader["StatusName"].ToString() // Uncomment if needed
                        };
                    }
                }
            }
            return null;
        }

        public void SaveContactDetails(long? partnerid, int entityid, List<VendorContact_Details> contacts)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Partner.Sp_mst_InsUpdPartnerContact", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PartnerID", partnerid);
                cmd.Parameters.AddWithValue("@Entity_ID", entityid);

                var dt = new DataTable();
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Department", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Mobile", typeof(string));
                dt.Columns.Add("Fax", typeof(string));
                dt.Columns.Add("Designation", typeof(string));
                foreach (var c in contacts)
                    dt.Rows.Add(c.Contact_Name, c.Contact_Type, c.Contact_Phone, c.Contact_Email, c.Contact_Mobile, c.Contact_Fax, c.Designation);

                var param = cmd.Parameters.AddWithValue("@Contacts", dt);
                param.SqlDbType = SqlDbType.Structured;
                param.TypeName = "client.udt_Contact";

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }


        public bool ImportVendor(string vendorXml, string contactXml,int? entityId)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Partner.Import_PartnerData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Entity_ID", entityId);

                    cmd.Parameters.Add("@VendorXml", SqlDbType.Xml).Value = vendorXml;
                    cmd.Parameters.Add("@ContactXml", SqlDbType.Xml).Value = contactXml;

                    var result = cmd.ExecuteScalar();
                    var resultText = result != null && result != DBNull.Value
                  ? Convert.ToString(result)
                  : "";

                    var isSuccess = resultText == "I0001" || resultText == "U0001";

                    return isSuccess;
                }
            }
        }

        public PurchasesOrderHistoryFullResult GetPurchasesOrderHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new PurchasesOrderHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.Get_PurchaseOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var lookup = new Dictionary<int, PurchasesOrderHistoryList>();
                    while (reader.Read())
                    {
                        int PurchaseOrderId = reader["PurchaseOrderId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PurchaseOrderId"]);
                        var PurchaseOrders = new PurchasesOrderHistoryList
                        {
                            PurchaseOrderId = PurchaseOrderId,
                            Date = reader["Date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["Date"]),
                            Number = reader["Number"] == DBNull.Value ? null : reader["Number"].ToString(),
                            VendorName = reader["VendorName"] == DBNull.Value ? null : reader["VendorName"].ToString(),
                            Amount = reader["Amount"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["Amount"]),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            BillingStatus = reader["BillingStatus"] == DBNull.Value ? null : reader["BillingStatus"].ToString(),
                            CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                            Items = new List<PurchaseOrderListItem>()
                        };
                        result.PurchasesOrderHistory.Add(PurchaseOrders);
                        lookup[PurchaseOrderId] = PurchaseOrders;
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {

                            int PurchaseOrderId = reader["PurchaseOrderId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PurchaseOrderId"]);

                            var item = new PurchaseOrderListItem
                            {
                                PurchaseOrderId = PurchaseOrderId,

                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDescription = reader["ItemDescription"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),

                                DiscountType = reader["DiscountType"]?.ToString(),
                                DiscountValue = reader["DiscountValue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DiscountValue"]),
                                TaxType = reader["TaxType"]?.ToString(),

                                Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
                                Rate = reader["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Rate"]),
                                BaseAmount = reader["BaseAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["BaseAmount"])
                            };

                            if (lookup.ContainsKey(PurchaseOrderId))
                            {
                                lookup[PurchaseOrderId].Items.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }


        public PurchasesCreditNotesHistoryFullResult GetPurchasesCreditNotesHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new PurchasesCreditNotesHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Purchase].[Get_PurchaseCredit]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var lookup = new Dictionary<int, PurchasesCreditNotesHistoryList>();
                    while (reader.Read())

                    {
                        int PurchaseCreditNoteId = reader["PurchaseCreditNoteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PurchaseCreditNoteId"]);
                        var PurchasesCreditNotes = new PurchasesCreditNotesHistoryList
                        {
                            CreditDate = reader["CreditDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreditDate"]),
                            CreditNoteNumber = reader["CreditNoteNumber"] == DBNull.Value ? null : reader["CreditNoteNumber"].ToString(),
                            BillNumber = reader["BillNumber"] == DBNull.Value ? null : reader["BillNumber"].ToString(),
                            VendorName = reader["VendorName"] == DBNull.Value ? null : reader["VendorName"].ToString(),
                            CreditAmount = reader["CreditAmount"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["CreditAmount"]),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                            PurchaseCreditNoteId = PurchaseCreditNoteId,
                            Items = new List<PurchasesCreditNotesItems>()
                        };
                        result.PurchasesCreditNotesHistory.Add(PurchasesCreditNotes);
                        lookup[PurchaseCreditNoteId] = PurchasesCreditNotes;
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            int PurchaseCreditNoteId = reader["PurchaseCreditNoteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PurchaseCreditNoteId"]);

                            var item = new PurchasesCreditNotesItems
                            {
                                PurchaseCreditNoteId = PurchaseCreditNoteId,

                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDescription = reader["ItemDescription"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),

                                DiscountType = reader["DiscountType"]?.ToString(),
                                DiscountValue = reader["DiscountValue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DiscountValue"]),
                                TaxType = reader["TaxType"]?.ToString(),

                                Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
                                Rate = reader["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Rate"]),
                                BaseAmount = reader["BaseAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["BaseAmount"])
                            };

                            if (lookup.ContainsKey(PurchaseCreditNoteId))
                            {
                                lookup[PurchaseCreditNoteId].Items.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public PurchasesBillHistoryFullResult GetPurchasesBillHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new PurchasesBillHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.[Get_PurchaseBills]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var lookup = new Dictionary<int, PurchasesBillHistoryList>();
                    while (reader.Read())
                    {
                        int PurchaseBillId = reader["PurchaseBillId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PurchaseBillId"]);
                        var PurchaseBill= new PurchasesBillHistoryList
                         {
                            PurchaseBillId = PurchaseBillId,
                            BillDate = reader["BillDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["BillDate"]),
                            BillNumber = reader["BillNumber"] == DBNull.Value ? null : reader["BillNumber"].ToString(),
                            VendorName = reader["VendorName"] == DBNull.Value ? null : reader["VendorName"].ToString(),
                            PONumber = reader["PONumber"] == DBNull.Value ? null : reader["PONumber"].ToString(),
                            BillAmount = reader["BillAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(reader["BillAmount"]),
                            DueDate = reader["DueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DueDate"]),
                            AmountOutstanding = reader["AmountOutstanding"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(reader["AmountOutstanding"]),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                            Items = new List<PurchaseBillItem>()
                        };
                        result.PurchasesBillHistory.Add(PurchaseBill);
                        lookup[PurchaseBillId] = PurchaseBill;
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            int PurchaseBillId = reader["PurchaseBillId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PurchaseBillId"]);

                            var item = new PurchaseBillItem
                            {
                                PurchaseBillId = PurchaseBillId,

                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDescription = reader["ItemDescription"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),

                                DiscountType = reader["DiscountType"]?.ToString(),
                                DiscountValue = reader["DiscountValue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DiscountValue"]),
                                TaxType = reader["TaxType"]?.ToString(),

                                Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
                                Rate = reader["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Rate"]),
                                BaseAmount = reader["BaseAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["BaseAmount"])
                            };

                            if (lookup.ContainsKey(PurchaseBillId))
                            {
                                lookup[PurchaseBillId].Items.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public PurchasesRecBillHistoryFullResult GetPurchasesRecBillHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new PurchasesRecBillHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Purchase].[Get_RecurringBills]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var lookup = new Dictionary<int, PurchasesRecBillHistoryList>();
                    while (reader.Read())

                    {
                         int RecurringScheduleId = reader["RecurringScheduleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RecurringScheduleId"]);
                        var PurchasesRecBill = new PurchasesRecBillHistoryList
                        {
                            VendorName = reader["VendorName"] == DBNull.Value ? null : reader["VendorName"].ToString(),
                            BillNumber = reader["BillNumber"] == DBNull.Value ? null : reader["BillNumber"].ToString(),
                            Frequency = reader["Frequency"] == DBNull.Value ? null : reader["Frequency"].ToString(),
                            PreviousBillDate = reader["PreviousBillDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["PreviousBillDate"]),
                            NextBillDate = reader["NextBillDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NextBillDate"]),
                            EndDate = reader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndDate"]),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            Amount = reader["Amount"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["Amount"]),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                            RecurringScheduleId = RecurringScheduleId,

                            Items = new List<PurchasesRecBillItems>()
                        };
                        result.PurchasesRecBillHistory.Add(PurchasesRecBill);
                        lookup[RecurringScheduleId] = PurchasesRecBill;
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            int RecurringScheduleId = reader["RecurringScheduleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RecurringScheduleId"]);

                            var item = new PurchasesRecBillItems
                            {
                                RecurringScheduleId = RecurringScheduleId,

                                ItemName = reader["ItemName"]?.ToString(),
                                ItemDescription = reader["ItemDescription"]?.ToString(),
                                ItemType = reader["ItemType"]?.ToString(),

                                DiscountType = reader["DiscountType"]?.ToString(),
                                DiscountValue = reader["DiscountValue"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["DiscountValue"]),
                                TaxType = reader["TaxType"]?.ToString(),

                                Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
                                Rate = reader["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Rate"]),
                                BaseAmount = reader["BaseAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["BaseAmount"])
                            };

                            if (lookup.ContainsKey(RecurringScheduleId))
                            {
                                lookup[RecurringScheduleId].Items.Add(item);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public PurchasesExpenseHistoryFullResult GetExpenseHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new PurchasesExpenseHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.SP_GetExpenseList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ExpenseId = reader["ExpenseId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ExpenseId"]);
                        result.PurchasesExpenseHistory.Add(new PurchasesExpenseHistoryList
                        {
                            ExpenseDate = reader["ExpenseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ExpenseDate"]),
                            VendorName = reader["VendorName"] == DBNull.Value ? null : reader["VendorName"].ToString(),
                            LedgerAccount = reader["LedgerAccount"] == DBNull.Value ? null : reader["LedgerAccount"].ToString(),
                            ExpenseAmount = reader["ExpenseAmount"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["ExpenseAmount"]),
                            PaidFrom = reader["PaidFrom"] == DBNull.Value ? null : reader["PaidFrom"].ToString(),
                            ExpenseType = reader["ExpenseType"] == DBNull.Value ? null : reader["ExpenseType"].ToString(),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                            Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            Branch = reader["Branch"] == DBNull.Value ? null : reader["Branch"].ToString(),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                            HSNCode = reader["HSNCode"] == DBNull.Value ? (int?)null : TryParseInt(reader["HSNCode"]),
                            ExpenseId = ExpenseId
                        });
                    }
                }
            }

            return result;
        }
        public PurchasesRecExpenseHistoryFullResult GetRecExpenseHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new PurchasesRecExpenseHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.SP_GetExpenseList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ExpenseId = reader["ExpenseId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ExpenseId"]);
                        result.PurchasesRecExpenseHistory.Add(new PurchasesRecExpenseHistoryList
                        {
                            StartDate = reader["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["StartDate"]),
                            EndDate = reader["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EndDate"]),
                            VendorName = reader["VendorName"] == DBNull.Value ? null : reader["VendorName"].ToString(),
                            Frequency = reader["Frequency"] == DBNull.Value ? null : reader["Frequency"].ToString(),
                            Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString(),
                            Branch = reader["Branch"] == DBNull.Value ? null : reader["Branch"].ToString(),
                            GstApplicable = reader["GstApplicable"] == DBNull.Value ? null : reader["GstApplicable"].ToString(),
                            ITCAvailable = reader["ITCAvailable"] == DBNull.Value ? null : reader["ITCAvailable"].ToString(),
                            LedgerAccount = reader["LedgerAccount"] == DBNull.Value ? null : reader["LedgerAccount"].ToString(),
                            ExpenseAmount = reader["ExpenseAmount"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["ExpenseAmount"]),
                            PaidFrom = reader["PaidFrom"] == DBNull.Value ? null : reader["PaidFrom"].ToString(),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                            TaxAmount = reader["TaxAmount"] == DBNull.Value ? (int?)null : TryParseInt(reader["TaxAmount"]),
                            TdsAmount = reader["TdsAmount"] == DBNull.Value ? (int?)null : TryParseInt(reader["TdsAmount"]),
                            HSNCode = reader["HSNCode"] == DBNull.Value ? (int?)null : TryParseInt(reader["HSNCode"]),
                            ExpenseId = ExpenseId
                        });
                    }
                }
            }

            return result;
        }


        public bool SavePurchaseOrder(PurchaseOrderCreate model, int? entityId, int? loginId, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.sp_SavePurchaseOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@LoginId", loginId);

                cmd.Parameters.AddWithValue("@VendorId", model.VendorId);
                cmd.Parameters.AddWithValue("@ReferenceNumber", model.ReferenceNumber ?? "");
                cmd.Parameters.AddWithValue("@PurchaseOrderDate", model.PurchaseOrderDate);
                cmd.Parameters.AddWithValue("@DeliveryAddress", model.DeliveryAddress ?? "");
                cmd.Parameters.AddWithValue("@CurrencyId", model.CurrencyId);
                cmd.Parameters.AddWithValue("@Payment_Term1", model.Payment_Term1);
                cmd.Parameters.AddWithValue("@Payment_Term2", model.Payment_Term2);
                cmd.Parameters.AddWithValue("@ExpectedDeliveryDate", model.ExpectedDeliveryDate);
                cmd.Parameters.AddWithValue("@BranchId", model.BranchId);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Tax_Value", model.Tax_Value);
                cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);
                cmd.Parameters.AddWithValue("@Discount", model.Discount);
                cmd.Parameters.AddWithValue("@OtherAdjustment", model.OtherAdjustment);
                cmd.Parameters.AddWithValue("@POStatus", model.POStatus);
                cmd.Parameters.AddWithValue("@BillingStatus", model.BillingStatus);
                cmd.Parameters.AddWithValue("@Comments", model.Comments ?? "");
                cmd.Parameters.AddWithValue("@TermsAndConditions", model.TermsAndConditions ?? "");


                var dt = new DataTable();
                dt.Columns.Add("ItemType", typeof(string));
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("GL_ID", typeof(int));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Rate", typeof(decimal));
                dt.Columns.Add("DiscountType", typeof(string));
                dt.Columns.Add("DiscountValue", typeof(decimal));
                dt.Columns.Add("TaxGroupId", typeof(string));
                dt.Columns.Add("BaseAmount", typeof(decimal));

                foreach (var item in model.Items)
                {
                    dt.Rows.Add(
                        item.ItemType,
                        item.ItemId,
                        item.GL_ID,
                        item.Quantity,
                        item.Rate,
                        item.DiscountType,
                        item.DiscountValue,
                        item.TaxGroupId,
                       item.BaseAmount
                    );
                }

                var tvpParam = cmd.Parameters.AddWithValue("@PODetails", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "Sales.Sale_InvoiceDetailType";

                var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                messageCode = outParam.Value?.ToString();
                isSuccess = messageCode == "I0001" || messageCode == "U0001";
            }
            return isSuccess;
        }

        public bool SavePurchaseBills(PurchaseBillsCreate model, int? entityId, int? loginId, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.sp_SavePurchaseBill", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@LoginId", loginId);

                cmd.Parameters.AddWithValue("@VendorId", model.VendorId);
                cmd.Parameters.AddWithValue("@PurchaseBillDate", model.PurchaseBillDate);
                cmd.Parameters.AddWithValue("@PurchaseOrderId", model.PurchaseOrderId);
                cmd.Parameters.AddWithValue("@AccountingDate", model.AccountingDate);
                cmd.Parameters.AddWithValue("@DueDate", model.DueDate);
                cmd.Parameters.AddWithValue("@Payment_Term1", model.Payment_Term1);
                cmd.Parameters.AddWithValue("@Payment_Term2", model.Payment_Term2);
                cmd.Parameters.AddWithValue("@CurrencyId", model.CurrencyId);
                cmd.Parameters.AddWithValue("@BranchId", model.BranchId);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Tds_Value", model.Tds_Value);
                cmd.Parameters.AddWithValue("@Amount", model.Amount);
                cmd.Parameters.AddWithValue("@Discount", model.Discount);
                cmd.Parameters.AddWithValue("@OtherAdjustment", model.OtherAdjustment);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Comments", model.Comments ?? "");
                cmd.Parameters.AddWithValue("@TermsAndConditions", model.TermsAndConditions ?? "");

                var dt = new DataTable();
                dt.Columns.Add("ItemType", typeof(string));
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("GL_ID", typeof(int));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Rate", typeof(decimal));
                dt.Columns.Add("DiscountType", typeof(string));
                dt.Columns.Add("DiscountValue", typeof(decimal));
                dt.Columns.Add("TaxId", typeof(int));
                dt.Columns.Add("BaseAmount", typeof(decimal));

                foreach (var item in model.Items)
                {
                    dt.Rows.Add(
                        item.ItemType,
                        item.ItemId,
                        item.GL_ID,
                        item.Quantity,
                        item.Rate,
                        item.DiscountType,
                        item.DiscountValue,
                        item.TaxId,
                       item.BaseAmount
                    );
                }

                var tvpParam = cmd.Parameters.AddWithValue("@BillDetails", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "Sales.Sale_InvoiceDetailType";

                var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                messageCode = outParam.Value?.ToString();
                isSuccess = messageCode == "I0001" || messageCode == "U0001";
            }
            return isSuccess;
        }


        public bool SavePurchaseCreditNote(PurchaseCreditNoteCreate model, int? entityId, int? loginId, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Purchase.sp_SavePurchaseCreditNote", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@LoginId", loginId);

                cmd.Parameters.AddWithValue("@VendorId", model.VendorId);
                cmd.Parameters.AddWithValue("@CreditNoteDate", model.CreditNoteDate);
                cmd.Parameters.AddWithValue("@PurchaseBillId", model.PurchaseBillId);
                cmd.Parameters.AddWithValue("@AccountingDate", model.AccountingDate);
                cmd.Parameters.AddWithValue("@Reason", model.Reason);
                cmd.Parameters.AddWithValue("@CurrencyId", model.CurrencyId);
                cmd.Parameters.AddWithValue("@BranchId", model.BranchId);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Tax_Value", model.Tax_Value);
                cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);
                cmd.Parameters.AddWithValue("@Discount", model.Discount);
                cmd.Parameters.AddWithValue("@OtherAdjustment", model.OtherAdjustment);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Comments", model.Comments ?? "");
                cmd.Parameters.AddWithValue("@TermsAndConditions", model.TermsAndConditions ?? "");

                var dt = new DataTable();
                dt.Columns.Add("ItemType", typeof(string));
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("GL_ID", typeof(int));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Rate", typeof(decimal));
                dt.Columns.Add("DiscountType", typeof(string));
                dt.Columns.Add("DiscountValue", typeof(decimal));
                dt.Columns.Add("TaxGroupId", typeof(int));
                dt.Columns.Add("BaseAmount", typeof(decimal));

                foreach (var item in model.Items)
                {
                    dt.Rows.Add(
                        item.ItemType,
                        item.ItemId,
                        item.GL_ID,
                        item.Quantity,
                        item.Rate,
                        item.DiscountType,
                        item.DiscountValue,
                        item.TaxGroupId,
                       item.BaseAmount
                    );
                }

                var tvpParam = cmd.Parameters.AddWithValue("@CreditNoteDetails", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "Sales.Sale_InvoiceDetailType";

                var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                messageCode = outParam.Value?.ToString();
                isSuccess = messageCode == "I0001" || messageCode == "U0001";
            }
            return isSuccess;
        }




        public bool SavePurchaseRecurringBills(PurchaseRecurringBillsCreate model, int? entityId, int? loginId, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Purchase].[sp_SaveRecurringBills]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@LoginId", loginId);

                cmd.Parameters.AddWithValue("@VendorId", model.VendorId);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@Payment_Term1", model.Payment_Term1);
                cmd.Parameters.AddWithValue("@Payment_Term2", model.Payment_Term2);
                cmd.Parameters.AddWithValue("@CurrencyId", model.CurrencyId);
                cmd.Parameters.AddWithValue("@BranchId", model.BranchId);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Tax_Value", model.Tax_Value);
                cmd.Parameters.AddWithValue("@Amount", model.Amount);
                cmd.Parameters.AddWithValue("@Discount", model.Discount);
                cmd.Parameters.AddWithValue("@OtherAdjustment", model.OtherAdjustment);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Comments", model.Comments ?? "");
                cmd.Parameters.AddWithValue("@TermsAndConditions", model.TermsAndConditions ?? "");

                var dt = new DataTable();
                dt.Columns.Add("ItemType", typeof(string));
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("GL_ID", typeof(int));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Rate", typeof(decimal));
                dt.Columns.Add("DiscountType", typeof(string));
                dt.Columns.Add("DiscountValue", typeof(decimal));
                dt.Columns.Add("TaxGroupId", typeof(int));
                dt.Columns.Add("BaseAmount", typeof(decimal));

                foreach (var item in model.Items)
                {
                    dt.Rows.Add(
                        item.ItemType,
                        item.ItemId,
                        item.GL_ID,
                        item.Quantity,
                        item.Rate,
                        item.DiscountType,
                        item.DiscountValue,
                        item.TaxGroupId,
                       item.BaseAmount
                    );
                }

                var tvpParam = cmd.Parameters.AddWithValue("@BillDetails", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "Sales.Sale_InvoiceDetailType";

                var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                messageCode = outParam.Value?.ToString();
                isSuccess = messageCode == "I0001" || messageCode == "U0001";
            }
            return isSuccess;
        }


        //Save Expense

        public bool SaveExpense(ExpenseCreate model, int entityId, int loginId, string recordType, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            try
            {
                string expenseXml = BuildExpenseXml(model);

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("Purchase.Sp_SaveExpense", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;
                    cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                    cmd.Parameters.AddWithValue("@LoginId", loginId);
                    cmd.Parameters.AddWithValue("@RecordType", recordType);
                    cmd.Parameters.AddWithValue("@xmlData", expenseXml);

                    var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    messageCode = outParam.Value?.ToString() ?? "E0004";
                    isSuccess = messageCode == "I0001" || messageCode == "U0001";
                }
            }
            catch (Exception ex)
            {
                messageCode = "E0003";
            }

            return isSuccess;
        }

        //XML helper
        private string BuildExpenseXml(ExpenseCreate model)
        {
            var xDoc = new XDocument(
                new XElement("Expense",
                    new XElement("RecordType", model.RecordType ?? ""),
                    new XElement("ExpenseType", model.ExpenseType ?? ""),
                    new XElement("HSNCode", model.HSNCode ?? ""),
                    new XElement("BranchId", model.BranchId ?? 0),
                    new XElement("DepartmentId", model.DepartmentId ?? 0),
                    new XElement("ExpenseLedgerId", model.ExpenseLedgerId ?? 0),
                    new XElement("ExpenseDate", model.ExpenseDate?.ToString("yyyy-MM-dd") ?? ""),
                    new XElement("Description", model.Description ?? ""),
                    new XElement("ExpenseAmount", model.ExpenseAmount ?? 0),
                    new XElement("VendorId", model.VendorId ?? 0),
                    new XElement("PaidFrom", model.PaidFrom ?? ""),
                    new XElement("GstApplicable", model.GstApplicable),
                    new XElement("GstTaxCode", model.GstTaxCode ?? 0),
                    new XElement("TaxAmount", model.TaxAmount ?? 0),
                    new XElement("ItcAvailable", model.ItcAvailable ?? ""),
                    new XElement("TdsChargeable", model.TdsChargeable),
                    new XElement("TdsCode", model.TdsCode ?? 0),
                    new XElement("TdsAmount", model.TdsAmount ?? 0),
                    new XElement("SaveAsDraft", model.SaveAsDraft),
                    new XElement("Frequency", model.Frequency ?? ""),
                    new XElement("StartDate", model.StartDate?.ToString("yyyy-MM-dd") ?? ""),
                    new XElement("EndDate", model.EndDate?.ToString("yyyy-MM-dd") ?? ""),
                    new XElement("Items",
                        model.BulkBookingItems.Select(item =>
                            new XElement("Item",
                                new XElement("Date", item.Date?.ToString("yyyy-MM-dd") ?? ""),
                                new XElement("ExpenseLedgerId", item.ExpenseLedgerId ?? 0),
                                new XElement("Description", item.Description ?? ""),
                                new XElement("Amount", item.Amount ?? 0),
                                new XElement("AmountCurrency", item.AmountCurrency ?? ""),
                                new XElement("TaxAmount", item.TaxAmount ?? 0),
                                new XElement("TaxCurrency", item.TaxCurrency ?? ""),
                                new XElement("VendorId", item.VendorId ?? 0),
                                new XElement("PaidFrom", item.PaidFrom ?? ""),
                                new XElement("BranchId", item.BranchId ?? 0),
                                new XElement("DepartmentId", item.DepartmentId ?? 0)
                            )
                        )
                    )
                )
            );

            return xDoc.ToString();
        }

        private long? TryParseLong(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (long.TryParse(value.ToString(), out var result)) return result;
            return null;
        }

        private int? TryParseInt(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (int.TryParse(value.ToString(), out var result)) return result;
            return null;
        }

        private decimal? TryParseDecimal(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (decimal.TryParse(value.ToString(), out var result)) return result;
            return null;
        }

        private bool? TryParseBool(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (bool.TryParse(value.ToString(), out var result)) return result;
            if (value.ToString() == "1") return true;
            if (value.ToString() == "0") return false;
            return null;
        }
    }
}

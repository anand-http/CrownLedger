using fintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static fintech.Models.CommonModel;

namespace fintech.DAL
{
    public class SalesDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;


        public CustomerFullResult GetCustomerList(int? entityId, int start, int length, string orderby, string search, string statusFilter)
        {
            var result = new CustomerFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CLIENT.SP_GetClientList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StatusFilter", statusFilter ?? (object)DBNull.Value);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    do
                    {
                        // Skip empty result sets (exec() can emit these)
                        if (reader.FieldCount == 0) continue;

                        string firstCol = reader.GetName(0);

                        // ── Result Set: Customers (CTE ROW_NUMBER → first col is "Row")
                        if (firstCol == "Row")
                        {
                            while (reader.Read())
                            {
                                result.Customers.Add(new GetCustomerList_Result
                                {
                                    Client_ID = reader["Client_ID"] == DBNull.Value ? (long?)null : TryParseLong(reader["Client_ID"]),
                                    CustomerName = reader["CustomerName"] == DBNull.Value ? null : reader["CustomerName"].ToString(),
                                    Client_Code = reader["Client_Code"] == DBNull.Value ? null : reader["Client_Code"].ToString(),
                                    Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                                    Telephone = reader["Telephone"] == DBNull.Value ? null : reader["Telephone"].ToString(),
                                    Country = reader["Country"] == DBNull.Value ? null : reader["Country"].ToString(),
                                    Country2 = reader["Country"] == DBNull.Value ? null : reader["Country"].ToString(),
                                    City = reader["City"] == DBNull.Value ? null : reader["City"].ToString(),
                                    City2 = reader["City2"] == DBNull.Value ? null : reader["City2"].ToString(),
                                    Website = reader["Website"] == DBNull.Value ? null : reader["Website"].ToString(),
                                    State = reader["State"] == DBNull.Value ? null : reader["State"].ToString(),
                                    Department = reader["Department"] == DBNull.Value ? null : reader["Department"].ToString(),
                                    DefaultCurrency = reader["DefaultCurrency"] == DBNull.Value ? null : reader["DefaultCurrency"].ToString(),
                                    BankCurrency = reader["DefaultCurrency"] == DBNull.Value ? null : reader["DefaultCurrency"].ToString(),
                                    BankCurrencyId = reader["Currency_Id"] == DBNull.Value ? null : TryParseInt(reader["Currency_Id"]),
                                    State_Id = reader["State_Id"] == DBNull.Value ? null : TryParseInt(reader["State_Id"]),
                                    City_Id2 = reader["City_Id2"] == DBNull.Value ? null : TryParseInt(reader["City_Id2"]),
                                    City_Id = reader["City_Id"] == DBNull.Value ? null : TryParseInt(reader["City_Id"]),
                                    Currency_Id = reader["Currency_Id"] == DBNull.Value ? null : TryParseInt(reader["Currency_Id"]),
                                    Contry_Id = reader["Contry_Id"] == DBNull.Value ? null : TryParseInt(reader["Contry_Id"]),
                                    DepartMentId = reader["DepartMentId"] == DBNull.Value ? null : TryParseInt(reader["DepartMentId"]),
                                    Address1 = reader["Address1"] == DBNull.Value ? null : reader["Address1"].ToString(),
                                    Address2 = reader["Address2"] == DBNull.Value ? null : reader["Address2"].ToString(),
                                    CorrespondenceAddress1 = reader["CorrespondenceAddress1"] == DBNull.Value ? null : reader["CorrespondenceAddress1"].ToString(),
                                    CorrespondenceAddress2 = reader["CorrespondeceAddress2"] == DBNull.Value ? null : reader["CorrespondeceAddress2"].ToString(),
                                    ZipCode = reader["ZipCode"] == DBNull.Value ? null : reader["ZipCode"].ToString(),
                                    ZipCode2 = reader["ZipCode2"] == DBNull.Value ? null : reader["ZipCode2"].ToString(),
                                    AccountNumber = reader["AccountNumber"] == DBNull.Value ? null : reader["AccountNumber"].ToString(),
                                    Payee = reader["Payee"] == DBNull.Value ? null : reader["Payee"].ToString(),
                                    BankName = reader["BankName"] == DBNull.Value ? null : reader["BankName"].ToString(),
                                    IBAN_IFSC = reader["IBAN_IFSC"] == DBNull.Value ? null : reader["IBAN_IFSC"].ToString(),
                                    SWIFTCode = reader["SWIFTCode"] == DBNull.Value ? null : reader["SWIFTCode"].ToString(),
                                    SORTCode = reader["SORTCode"] == DBNull.Value ? null : reader["SORTCode"].ToString(),
                                    GST_VAT = reader["GST_VAT"] == DBNull.Value ? null : reader["GST_VAT"].ToString(),
                                    PAN = reader["PAN"] == DBNull.Value ? null : reader["PAN"].ToString(),
                                    TaxCode = reader["TaxCode"] == DBNull.Value ? null : reader["TaxCode"].ToString(),
                                    Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),
                                    AmountOutstanding = reader["AmountOutStanding"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["AmountOutStanding"]), // ← capital S
                                    UnusedCredit = reader["UnusedCredit"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["UnusedCredit"]),
                                    Client_Status = reader["Client_Status"] == DBNull.Value ? null : reader["Client_Status"].ToString(),
                                    Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString()
                                });
                            }
                        }

                        // ── Result Set: Addresses
                        else if (firstCol == "CAdd_Address_ID")
                        {
                            while (reader.Read())
                            {
                                result.Addresses.Add(new CustomerAddress_Result
                                {
                                    CAdd_Address_ID = reader["CAdd_Address_ID"] == DBNull.Value ? (int?)null : TryParseInt(reader["CAdd_Address_ID"]),
                                    CAdd_ClientID = reader["CAdd_ClientID"] == DBNull.Value ? (int?)null : TryParseInt(reader["CAdd_ClientID"]),
                                    CAdd_RStreet1 = reader["CAdd_RStreet1"] == DBNull.Value ? null : reader["CAdd_RStreet1"].ToString(),
                                    CAdd_RStreet2 = reader["CAdd_RStreet2"] == DBNull.Value ? null : reader["CAdd_RStreet2"].ToString(),
                                    CAdd_RStreet3 = reader["CAdd_RStreet3"] == DBNull.Value ? null : reader["CAdd_RStreet3"].ToString(),
                                    CAdd_RCity = reader["CAdd_RCity"] == DBNull.Value ? null : reader["CAdd_RCity"].ToString(),
                                    CAdd_RCountry = reader["CAdd_RCountry"] == DBNull.Value ? null : reader["CAdd_RCountry"].ToString(),
                                    CAdd_RZip = reader["CAdd_RZip"] == DBNull.Value ? null : reader["CAdd_RZip"].ToString(),
                                    CAdd_RPhone = reader["CAdd_RPhone"] == DBNull.Value ? null : reader["CAdd_RPhone"].ToString(),
                                    CAdd_RFax = reader["CAdd_RFax"] == DBNull.Value ? null : reader["CAdd_RFax"].ToString(),
                                    CAdd_CStreet1 = reader["CAdd_CStreet1"] == DBNull.Value ? null : reader["CAdd_CStreet1"].ToString(),
                                    CAdd_CStreet2 = reader["CAdd_CStreet2"] == DBNull.Value ? null : reader["CAdd_CStreet2"].ToString(),
                                    CAdd_CStreet3 = reader["CAdd_CStreet3"] == DBNull.Value ? null : reader["CAdd_CStreet3"].ToString(),
                                    CAdd_CCity = reader["CAdd_CCity"] == DBNull.Value ? null : reader["CAdd_CCity"].ToString(),
                                    CAdd_CCountry = reader["CAdd_CCountry"] == DBNull.Value ? null : reader["CAdd_CCountry"].ToString(),
                                    CAdd_CZip = reader["CAdd_CZip"] == DBNull.Value ? null : reader["CAdd_CZip"].ToString(),
                                    CAdd_CPhone = reader["CAdd_CPhone"] == DBNull.Value ? null : reader["CAdd_CPhone"].ToString(),
                                    CAdd_CFax = reader["CAdd_CFax"] == DBNull.Value ? null : reader["CAdd_CFax"].ToString(),
                                    Timestamp = reader["timestamp"] == DBNull.Value ? null : reader["timestamp"] as byte[],
                                });
                            }
                        }

                        // ── Result Set: PaymentTerms
                        else if (firstCol == "Client_Id")
                        {
                            while (reader.Read())
                            {
                                result.PaymentTerms.Add(new ClientPaymentTerms
                                {
                                    Client_Id = reader["Client_Id"] == DBNull.Value ? (long?)null : TryParseLong(reader["Client_Id"]),
                                    BillPercentage = reader["BillPercentage"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["BillPercentage"]),
                                    DueDateBasedOn = reader["DueDateBasedOn"] == DBNull.Value ? null : reader["DueDateBasedOn"].ToString(),
                                    CreditDays = reader["CreditDays"] == DBNull.Value ? (int?)null : TryParseInt(reader["CreditDays"]),
                                    CreditLimits = reader["CreditLimits"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["CreditLimits"]),
                                    IsDefaultBank = reader["IsDefaultBank"] == DBNull.Value ? (bool?)null : TryParseBool(reader["IsDefaultBank"]),
                                });
                            }
                        }

                        // ── Result Set: Contacts
                        else if (firstCol == "Contact_ID")
                        {
                            while (reader.Read())
                            {
                                result.Contacts.Add(new Contact_Details
                                {
                                    Contact_ID = reader["Contact_ID"] == DBNull.Value ? (int?)null : TryParseInt(reader["Contact_ID"]),
                                    Contact_Source = reader["Contact_Source"] == DBNull.Value ? null : reader["Contact_Source"].ToString(),
                                    Contact_SourceID = reader["Contact_SourceID"] == DBNull.Value ? (int?)null : TryParseInt(reader["Contact_SourceID"]),
                                    Contact_ContypID = reader["Contact_ContypID"] == DBNull.Value ? (int?)null : TryParseInt(reader["Contact_ContypID"]),
                                    Contact_Name = reader["Contact_Name"] == DBNull.Value ? null : reader["Contact_Name"].ToString(),
                                    Contact_Email = reader["Contact_Email"] == DBNull.Value ? null : reader["Contact_Email"].ToString(),
                                    Contact_Fax = reader["Contact_Fax"] == DBNull.Value ? null : reader["Contact_Fax"].ToString(),
                                    Contact_Phone = reader["Contact_Phone"] == DBNull.Value ? null : reader["Contact_Phone"].ToString(),
                                    Contact_Mobile = reader["Contact_Mobile"] == DBNull.Value ? null : reader["Contact_Mobile"].ToString(),
                                    Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),
                                    Contact_Type_desc = reader["Contact_Type_desc"] == DBNull.Value ? null : reader["Contact_Type_desc"].ToString(),
                                    Timestamp = reader["timestamp"] == DBNull.Value ? null : reader["timestamp"] as byte[],
                                });
                            }
                        }

                        // ── Result Set: CLContacts
                        else if (firstCol == "CLContact_ID")
                        {
                            while (reader.Read())
                            {
                                result.CLContacts.Add(new CLContact_Result
                                {
                                    CLContact_ID = reader["CLContact_ID"] == DBNull.Value ? (int?)null : TryParseInt(reader["CLContact_ID"]),
                                    CLContact_ClientID = reader["CLContact_ClientID"] == DBNull.Value ? (int?)null : TryParseInt(reader["CLContact_ClientID"]),
                                    CLContact_Email = reader["CLContact_Email"] == DBNull.Value ? null : reader["CLContact_Email"].ToString(),
                                    CLContact_Quality = reader["CLContact_Quality"] == DBNull.Value ? null : reader["CLContact_Quality"].ToString(),
                                    CLContact_Purposes = reader["CLContact_Purposes"] == DBNull.Value ? null : reader["CLContact_Purposes"].ToString(),
                                    CLContact_Type = reader["CLContact_Type"] == DBNull.Value ? null : reader["CLContact_Type"].ToString(),
                                    CLContact_Phone = reader["CLContact_Phone"] == DBNull.Value ? null : reader["CLContact_Phone"].ToString(),
                                    CLContact_Ph_Type = reader["CLContact_Ph_Type"] == DBNull.Value ? null : reader["CLContact_Ph_Type"].ToString(),
                                });
                            }
                        }

                    } while (reader.NextResult()); // ← loops through ALL result sets automatically
                }
            }

            return result;
        }
        public bool UpdateCustomerStatus(int clientId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE CLIENT.mst_Client SET Client_Status = @Status WHERE Client_Id = @Client_Id", conn))
            {
                cmd.Parameters.AddWithValue("@Client_Id", clientId);
                cmd.Parameters.AddWithValue("@Status", status);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool SaveSalesOrder(SalesOrderItemModel model, int? entityId, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Sales].[sp_SaveSaleOrders]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);

                cmd.Parameters.AddWithValue("@Sale_OrderDate", model.Sale_OrderDate);
                cmd.Parameters.AddWithValue("@CustomerId", model.CustomerId);
                cmd.Parameters.AddWithValue("@ReferenceNumber", model.ReferenceNumber ?? "");
                cmd.Parameters.AddWithValue("@Payment_Term1", model.Payment_Term1);
                cmd.Parameters.AddWithValue("@Payment_Term2", model.Payment_Term2 ?? "");
                cmd.Parameters.AddWithValue("@CurrencyId", model.CurrencyId);
                cmd.Parameters.AddWithValue("@DeliveryMethod", model.DeliveryMethod ?? "");
                cmd.Parameters.AddWithValue("@ShipmentDate", model.ShipmentDate);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Discount", model.Discount);
                cmd.Parameters.AddWithValue("@OtherAdjustment", model.OtherAdjustment);
                cmd.Parameters.AddWithValue("@Tax_Value", model.Tax_Value);
                cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);
                cmd.Parameters.AddWithValue("@Status", model.Status ?? "");
                cmd.Parameters.AddWithValue("@Comments", model.Comments ?? "");
                cmd.Parameters.AddWithValue("@TermsAndConditions", model.TermsAndConditions ?? "");
                cmd.Parameters.AddWithValue("@Sale_EstimateId", model.Sale_EstimateId);

                // Table-Valued Parameter for OrderDetails
                var dt = new DataTable();
                dt.Columns.Add("ItemType", typeof(string));
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("Quantity", typeof(int));
                dt.Columns.Add("Rate", typeof(decimal));
                dt.Columns.Add("TaxGroupId", typeof(int));
                //dt.Columns.Add("TaxRate_Percent", typeof(decimal)); 
                dt.Columns.Add("BaseAmount", typeof(decimal));

                foreach (var item in model.Items)
                {
                    dt.Rows.Add(
                        item.ItemType,
                        item.ItemId,
                        item.Quantity,
                        item.Rate,
                        item.TaxGroupId,
                       //item.TaxRate_Percent,
                       item.BaseAmount
                    );
                }

                var tvpParam = cmd.Parameters.AddWithValue("@OrderDetails", dt);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "Sales.Sale_OrderDetails";

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

        public UpsertCustomer_Result UpsertCustomer(SaveCustomerList_Result model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CLIENT.Sp_mst_InsUpdClientGeneral", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Client_Id", model.Client_ID);
                cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                cmd.Parameters.AddWithValue("@Code", model.Client_Code);
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
                cmd.Parameters.AddWithValue("@Status", model.Client_Status);

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
                        return new UpsertCustomer_Result
                        {
                            Client_Id = Convert.ToInt32(reader["Client_Id"]),
                            //StatusName = reader["StatusName"].ToString() // Uncomment if needed
                        };
                    }
                }
            }
            return null;
        }

        public void SaveContactDetails(long? clientId, int entityid, List<Contact_Details> contacts)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CLIENT.Sp_mst_InsUpdClientContact", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Client_ID", clientId);
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


        public bool ImportCustomers(string customerXml, string contactXml)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("Client.Import_ClientData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CustomerXml", SqlDbType.Xml).Value = customerXml;
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

        public void InsertContact(Contact_Details c)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Client.Import_ClientContact", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Contact_Source", (object)c.Contact_Source ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_SourceID", (object)c.Contact_SourceID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_ContypID", (object)c.Contact_ContypID ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_Name", (object)c.Contact_Name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_Email", (object)c.Contact_Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_Fax", (object)c.Contact_Fax ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_Phone", (object)c.Contact_Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_Mobile", (object)c.Contact_Mobile ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Designation", (object)c.Designation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Contact_Type_desc", (object)c.Contact_Type_desc ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public CustomerFullResult GetAllCustomersWithContacts()
        {
            var result = new CustomerFullResult();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("sp_GetAllCustomersWithContacts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        // Result Set 1: Customers
                        while (reader.Read())
                        {
                            result.Customers.Add(new GetCustomerList_Result
                            {
                                Client_ID = reader["Client_ID"] as long?,
                                Entity_ID = Convert.ToInt32(reader["Entity_ID"]),
                                CustomerName = reader["CustomerName"]?.ToString(),
                                Client_Code = reader["Client_Code"]?.ToString(),
                                Email = reader["Email"]?.ToString(),
                                Telephone = reader["Telephone"]?.ToString(),
                                AmountOutstanding = reader["AmountOutstanding"] as decimal?,
                                UnusedCredit = reader["UnusedCredit"] as decimal?,
                                Website = reader["Website"]?.ToString(),
                                Client_Status = reader["Client_Status"]?.ToString(),
                                Country = reader["Country"]?.ToString(),
                                Contry_Id = reader["Contry_Id"] as int?,
                                State = reader["State"]?.ToString(),
                                State_Id = reader["State_Id"] as int?,
                                Department = reader["Department"]?.ToString(),
                                DepartMentId = reader["DepartMentId"] as int?,
                                DefaultCurrency = reader["DefaultCurrency"]?.ToString(),
                                Currency_Id = reader["Currency_Id"] as int?,
                                Address1 = reader["Address1"]?.ToString(),
                                Address2 = reader["Address2"]?.ToString(),
                                CorrespondenceAddress1 = reader["CorrespondenceAddress1"]?.ToString(),
                                CorrespondenceAddress2 = reader["CorrespondenceAddress2"]?.ToString(),
                                City = reader["City"]?.ToString(),
                                City_Id = reader["City_Id"] as int?,
                                City2 = reader["City2"]?.ToString(),
                                City_Id2 = reader["City_Id2"] as int?,
                                State2 = reader["State2"]?.ToString(),
                                State2Id = reader["State2Id"] as int?,
                                Country2 = reader["Country2"]?.ToString(),
                                Country2Id = reader["Country2Id"] as int?,
                                ZipCode = reader["ZipCode"]?.ToString(),
                                ZipCode2 = reader["ZipCode2"]?.ToString(),
                                BankCurrency = reader["BankCurrency"]?.ToString(),
                                BankCurrencyId = reader["BankCurrencyId"] as int?,
                                AccountNumber = reader["AccountNumber"]?.ToString(),
                                Payee = reader["Payee"]?.ToString(),
                                BankName = reader["BankName"]?.ToString(),
                                IBAN_IFSC = reader["IBAN_IFSC"]?.ToString(),
                                SWIFTCode = reader["SWIFTCode"]?.ToString(),
                                SORTCode = reader["SORTCode"]?.ToString(),
                                IsDefaultBank = reader["IsDefaultBank"] as bool?,
                                GST_VAT = reader["GST_VAT"]?.ToString(),
                                PAN = reader["PAN"]?.ToString(),
                                TaxCode = reader["TaxCode"]?.ToString(),
                                BillPercentage = reader["BillPercentage"] as decimal?,
                                DueDateBasedOn = reader["DueDateBasedOn"]?.ToString(),
                                CreditDays = reader["CreditDays"] as int?,
                                Records = reader["Records"] as int?,
                                CreditLimits = reader["CreditLimits"] as decimal?,
                                Status = reader["Status"]?.ToString()
                            });
                        }

                        // Result Set 2: Contacts
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                long clientId = Convert.ToInt64(reader["Client_ID"]);
                                var contact = new Contact_Details
                                {
                                    Contact_ID = reader["Contact_ID"] as int?,
                                    Contact_Source = reader["Contact_Source"]?.ToString(),
                                    Contact_SourceID = reader["Contact_SourceID"] as int?,
                                    Contact_ContypID = reader["Contact_ContypID"] as int?,
                                    Contact_Name = reader["Contact_Name"]?.ToString(),
                                    Contact_Email = reader["Contact_Email"]?.ToString(),
                                    Contact_Fax = reader["Contact_Fax"]?.ToString(),
                                    Contact_Phone = reader["Contact_Phone"]?.ToString(),
                                    Contact_Mobile = reader["Contact_Mobile"]?.ToString(),
                                    Designation = reader["Designation"]?.ToString(),
                                    Contact_Type_desc = reader["Contact_Type_desc"]?.ToString()
                                };

                                // attach contact to its customer
                                var customer = result.Customers.FirstOrDefault(x => x.Client_ID == clientId);
                                if (customer != null)
                                {
                                    if (customer.contactDetails == null)
                                        customer.contactDetails = new List<Contact_Details>();
                                    customer.contactDetails.Add(contact);
                                }

                                result.Contacts.Add(contact);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public SaveResult SaveEstimate(SaleEstimateModel request)
        {
            var result = new SaveResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.sp_SaveSaleEstimate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@Sale_EstimateId",
                    //    request.Sale_EstimateId > 0
                    //    ? (object)request.Sale_EstimateId
                    //    : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Sale_EstimateDate", request.Sale_EstimateDate);
                    cmd.Parameters.AddWithValue("@CustomerId", request.CustomerId);
                    cmd.Parameters.AddWithValue("@ReferenceNumber", request.ReferenceNumber ?? "");
                    cmd.Parameters.Add("@ExpiryDate", SqlDbType.Date).Value =
                            !string.IsNullOrEmpty(request.ExpiryDate?.ToString())
                        ? (object)Convert.ToDateTime(request.ExpiryDate)
                        : DBNull.Value;
                    cmd.Parameters.AddWithValue("@SubTotal", request.SubTotal);
                    cmd.Parameters.AddWithValue("@Discount", request.Discount);
                    cmd.Parameters.AddWithValue("@OtherAdjustment", request.OtherAdjustment);
                    cmd.Parameters.AddWithValue("@Tax_Value", request.Tax_Value);
                    cmd.Parameters.AddWithValue("@TotalAmount", request.TotalAmount);
                    cmd.Parameters.AddWithValue("@Status", request.Status ?? "Draft");
                    cmd.Parameters.AddWithValue("@Comments", request.Comments ?? "");
                    cmd.Parameters.AddWithValue("@TermsAndConditions", request.TermsAndConditions ?? "");
                    cmd.Parameters.AddWithValue("@LoginId", request.Login_ID);
                    cmd.Parameters.AddWithValue("@Entity_ID", request.Entity_ID);

                    // ── DataTable passed as TVP ──
                    var dt = BuildEstimateDetailsDataTable(request.Items);
                    var tvpParam = cmd.Parameters.AddWithValue("@EstimateDetails", dt);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "Sales.Sale_EstimateDetailsType"; // must exist in DB 

                    var msgParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();

                    var messageCode = msgParam.Value?.ToString();
                    result.Status = messageCode == "I0001" || messageCode == "U0001";
                    if (messageCode == "I0001")
                    {
                        result.Message = "Estimate Created Successfully";
                    }
                    else
                    {
                        result.Message = "There is some issue in saving Estimate";
                    }
                }
            }
            return result;
        }

        private DataTable BuildEstimateDetailsDataTable(List<ItemDetailModel> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("ItemType", typeof(string));
            dt.Columns.Add("ItemId", typeof(int));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("TaxGroupId", typeof(int));
            dt.Columns.Add("BaseAmount", typeof(decimal));
            dt.Columns.Add("DiscountType", typeof(string));
            dt.Columns.Add("DiscountValue", typeof(decimal));

            foreach (var item in items)
            {
                var row = dt.NewRow();
                row["ItemType"] = item.ItemType ?? (object)DBNull.Value;
                row["ItemId"] = item.ItemId;
                row["Rate"] = item.Rate;
                row["Quantity"] = item.Quantity;
                row["TaxGroupId"] = item.TaxGroupId == 0 ? (object)DBNull.Value : item.TaxGroupId;
                row["BaseAmount"] = item.BaseAmount;
                row["DiscountType"] = item.DiscountType ?? (object)DBNull.Value;
                row["DiscountValue"] = item.DiscountValue;
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable BuildGLItemsDataTable(List<ItemDetailModel> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("ItemType", typeof(string));
            dt.Columns.Add("ItemId", typeof(int));
            dt.Columns.Add("GL_ID", typeof(int));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("TaxGroupId", typeof(int));
            dt.Columns.Add("BaseAmount", typeof(decimal));
            dt.Columns.Add("DiscountType", typeof(string));
            dt.Columns.Add("DiscountValue", typeof(decimal));

            foreach (var item in items)
            {
                dt.Rows.Add(
                    item.ItemType,
                    item.ItemId,
                    item.GL_ID,
                    item.Quantity,
                    item.Rate,
                    item.TaxGroupId,
                    item.BaseAmount,
                    item.DiscountType,
                    item.DiscountValue
                );
            }
            return dt;
        }
        public SaveResult SaveOrder(SaleEstimateModel request)
        {
            var result = new SaveResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.sp_SaveSaleEstimate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Sale_EstimateId",
                        request.Sale_EstimateId > 0
                        ? (object)request.Sale_EstimateId
                        : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Sale_EstimateDate", request.Sale_EstimateDate);
                    cmd.Parameters.AddWithValue("@CustomerId", request.CustomerId);
                    cmd.Parameters.AddWithValue("@ReferenceNumber", request.ReferenceNumber ?? "");
                    //cmd.Parameters.AddWithValue("@ExpiryDate", request.ExpiryDate ?? "");
                    cmd.Parameters.AddWithValue("@SubTotal", request.SubTotal);
                    cmd.Parameters.AddWithValue("@Discount", request.Discount);
                    cmd.Parameters.AddWithValue("@OtherAdjustment", request.OtherAdjustment);
                    cmd.Parameters.AddWithValue("@Tax_Value", request.Tax_Value);
                    cmd.Parameters.AddWithValue("@TotalAmount", request.TotalAmount);
                    cmd.Parameters.AddWithValue("@Status", request.Status ?? "Draft");
                    cmd.Parameters.AddWithValue("@Comments", request.Comments ?? "");
                    cmd.Parameters.AddWithValue("@TermsAndConditions", request.TermsAndConditions ?? "");
                    cmd.Parameters.AddWithValue("@LoginId", request.Login_ID);
                    cmd.Parameters.AddWithValue("@Entity_ID", request.Entity_ID);

                    // TVP parameter
                    var tvpParam = cmd.Parameters.AddWithValue(
                        "@EstimateDetails", BuildEstimateDetailsDataTable(request.Items));
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "Sale_EstimateDetailsType";

                    // output parameter
                    var msgParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();

                    var messageCode = msgParam.Value?.ToString();
                    result.Status = messageCode == "I0001" || messageCode == "U0001";
                    result.Message = messageCode;
                }
            }
            return result;
        }
        public SaveResult SaveInvoice(SaleInvoice request)
        {
            var result = new SaveResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.sp_SaveSaleInvoice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@Sale_EstimateId",
                    //    request.Sale_EstimateId > 0
                    //    ? (object)request.Sale_EstimateId
                    //    : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Sale_InvoiceDate", request.Sale_InvoiceDate);
                    cmd.Parameters.AddWithValue("@AccountingDate", request.AccountingDate);
                    cmd.Parameters.AddWithValue("@CustomerId", request.CustomerId);
                    cmd.Parameters.AddWithValue("@Payment_Term1", request.PaymentTerm);
                    cmd.Parameters.AddWithValue("@CurrencyId", request.Currency);
                    cmd.Parameters.AddWithValue("@BranchId", request.Branch);
                    cmd.Parameters.AddWithValue("@OrderId", request.ReferenceNumber ?? "");
                    cmd.Parameters.AddWithValue("@DueDate", request.ExpiryDate ?? "");
                    cmd.Parameters.AddWithValue("@SubTotal", request.SubTotal);
                    cmd.Parameters.AddWithValue("@Discount", request.Discount);
                    cmd.Parameters.AddWithValue("@OtherAdjustment", request.OtherAdjustment);
                    cmd.Parameters.AddWithValue("@Tax_Value", request.Tax_Value);
                    cmd.Parameters.AddWithValue("@Amount", request.TotalAmount);
                    cmd.Parameters.AddWithValue("@Status", request.Status ?? "Draft");
                    //cmd.Parameters.AddWithValue("@Comments", request.Comments ?? "");
                    //cmd.Parameters.AddWithValue("@TermsAndConditions", request.TermsAndConditions ?? "");
                    cmd.Parameters.AddWithValue("@LoginId", request.Login_ID);
                    cmd.Parameters.AddWithValue("@Entity_ID", request.Entity_ID);

                    // TVP parameter
                    var dt = BuildGLItemsDataTable(request.Items);
                    var tvpParam = cmd.Parameters.AddWithValue("@InvoiceDetails", dt);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "Sales.Sale_InvoiceDetailType";

                    // output parameter
                    var msgParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();

                    var messageCode = msgParam.Value?.ToString();
                    result.Status = messageCode == "I0001" || messageCode == "U0001";
                    if (messageCode == "I0001")
                    {
                        result.Message = "Invoice Created Successfully";
                    }
                    else
                    {
                        result.Message = "There is some issue in saving Invoice";
                    }
                }
            }
            return result;
        }

        public SaveResult SaveRecurringInv(SaleRecInv request)
        {
            var result = new SaveResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.sp_SaveRecurringInvoice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@Sale_EstimateId",
                    //    request.Sale_EstimateId > 0
                    //    ? (object)request.Sale_EstimateId
                    //    : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CustomerId", request.CustomerId);
                    cmd.Parameters.AddWithValue("@Frequency", request.Frequency);
                    cmd.Parameters.AddWithValue("@StartDate", request.StartDate);
                    cmd.Parameters.AddWithValue("@Payment_Term1", request.Paymentterms);
                    cmd.Parameters.AddWithValue("@CurrencyId", request.Currency);
                    cmd.Parameters.AddWithValue("@BranchId", request.Branch);
                    cmd.Parameters.AddWithValue("@UserId", request.SalesPerson);
                    cmd.Parameters.AddWithValue("@EndDate", request.EndDate);
                    cmd.Parameters.AddWithValue("@SubTotal", request.SubTotal);
                    cmd.Parameters.AddWithValue("@Discount", request.Discount);
                    cmd.Parameters.AddWithValue("@OtherAdjustment", request.OtherAdjustment);
                    cmd.Parameters.AddWithValue("@Tax_Value", request.Tax_Value);
                    cmd.Parameters.AddWithValue("@Amount", request.TotalAmount);
                    cmd.Parameters.AddWithValue("@Status", request.Status ?? "Draft");
                    cmd.Parameters.AddWithValue("@Comments", request.Comments ?? "");
                    cmd.Parameters.AddWithValue("@TermsAndConditions", request.TermsAndConditions ?? "");
                    cmd.Parameters.AddWithValue("@LoginId", request.Login_ID);
                    cmd.Parameters.AddWithValue("@Entity_ID", request.Entity_ID);

                    // TVP parameter
                    var dt = BuildGLItemsDataTable(request.Items);
                    var tvpParam = cmd.Parameters.AddWithValue("@RecurringDetails", dt);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "Sales.Sale_InvoiceDetailType";

                    // output parameter
                    var msgParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();

                    var messageCode = msgParam.Value?.ToString();
                    result.Status = messageCode == "I0001" || messageCode == "U0001";
                    if (messageCode == "I0001")
                    {
                        result.Message = "Invoice Created Successfully";
                    }
                    else
                    {
                        result.Message = "There is some issue in saving Invoice";
                    }
                }
            }
            return result;
        }
        public SaveResult SaveCreditNotes(SaleCreditNote request)
        {
            var result = new SaveResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.Sp_SaveCreditNote", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CreditNoteDate", request.Sale_CreditNoteDate);
                    cmd.Parameters.AddWithValue("@CustomerId", request.CustomerId);
                    cmd.Parameters.AddWithValue("@InvoiceId", request.ReferenceNumber ?? "");
                    cmd.Parameters.AddWithValue("@AccountingDate", request.ExpiryDate ?? "");
                    cmd.Parameters.AddWithValue("@SubTotal", request.SubTotal);
                    cmd.Parameters.AddWithValue("@Discount", request.Discount);
                    cmd.Parameters.AddWithValue("@OtherAdjustment", request.OtherAdjustment);
                    cmd.Parameters.AddWithValue("@Tax_Value", request.Tax_Value);
                    cmd.Parameters.AddWithValue("@TotalAmount", request.TotalAmount);
                    cmd.Parameters.AddWithValue("@Status", request.Status ?? "Draft");
                    cmd.Parameters.AddWithValue("@Comments", request.Comments ?? "");
                    cmd.Parameters.AddWithValue("@TermsAndConditions", request.TermsAndConditions ?? "");
                    cmd.Parameters.AddWithValue("@LoginId", request.Login_ID);
                    cmd.Parameters.AddWithValue("@Entity_ID", request.Entity_ID);

                    // TVP parameter
                    var tvpParam = cmd.Parameters.AddWithValue(
                        "@CreditDetails", BuildGLItemsDataTable(request.Items));
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "Sales.Sale_CreditDetails";

                    // output parameter
                    var msgParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 100)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(msgParam);

                    cmd.ExecuteNonQuery();

                    var messageCode = msgParam.Value?.ToString();
                    result.Status = messageCode == "I0001" || messageCode == "U0001";
                    if (messageCode == "I0001")
                    {
                        result.Message = "Credit Note Created Successfully";
                    }
                    else
                    {
                        result.Message = "There is some issue in saving Credit Note";
                    }
                }
            }
            return result;
        }

        // ── Sales Estimate ──
        public SalesEstimateHistoryFullResult GetSalesEstimateHistory(string statusFilter,int? entityId,int start,int length,string orderby,string search,string startDate,string endDate)
        {
            var result = new SalesEstimateHistoryFullResult();

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("Sales.GetSaleEstimate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@Length", length);
                    cmd.Parameters.AddWithValue("@OrderBy", orderby ?? "");
                    cmd.Parameters.AddWithValue("@Search", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);
                    cmd.Parameters.AddWithValue("@StatusFilter", string.IsNullOrEmpty(statusFilter) ? (object)DBNull.Value : statusFilter);
                    cmd.Parameters.AddWithValue("@StartDate", startDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", endDate ?? (object)DBNull.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                       
                        var lookup = new Dictionary<int, SalesEstimateHistoryList>();

                        while (reader.Read())
                        {
                            int estimateId = reader["Sale_EstimateId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(reader["Sale_EstimateId"]);

                            var estimate = new SalesEstimateHistoryList
                            {
                                Sale_EstimateId = estimateId,
                                Sale_EstimateNo = reader["EstimateNumber"]?.ToString(),
                                EstimateDate = reader["EstimateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["EstimateDate"]),
                                CustomerName = reader["Customer"]?.ToString(),
                                EstimateNumber = reader["ReferenceNumber"]?.ToString(),
                                ExpiryDate = reader["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ExpiryDate"]),
                                Amount = reader["Amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Amount"]),
                                Status = reader["Status"]?.ToString(),
                                CreatedBy = reader["CreatedBy"]?.ToString(),
                                CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                                Records = reader["Records"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Records"]),

                                Items = new List<SalesEstimateItem>() 
                            };

                            result.SalesEstimateHistory.Add(estimate);
                            lookup[estimateId] = estimate;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                int estimateId = reader["Sale_EstimateId"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["Sale_EstimateId"]);

                                var item = new SalesEstimateItem
                                {
                                    Sale_EstimateId = estimateId,

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

                                if (lookup.ContainsKey(estimateId))
                                {
                                    lookup[estimateId].Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        // ── Sales Invoice ──

        public SalesInvoiceHistoryFullResult GetSalesInvoiceHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            var result = new SalesInvoiceHistoryFullResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.Get_SaleInvoice", conn))
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
                    using (var reader = cmd.ExecuteReader())
                    {
                        var lookup = new Dictionary<int, SalesInvoiceHistoryList>();
                        while (reader.Read())
                        {
                            int Sale_InvoiceId = reader["InvoiceId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                             var SaleInvoice = new SalesInvoiceHistoryList
                             {
                                Sale_InvoiceId = Sale_InvoiceId,
                                Sale_InvoiceNo = reader["InvoiceNumber"] == DBNull.Value ? null : reader["InvoiceNumber"].ToString(),
                                Sale_InvoiceDate = reader["InvoiceDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["InvoiceDate"]),
                                CustomerName = reader["Customer"] == DBNull.Value ? null : reader["Customer"].ToString(),
                                InvoiceNumber = reader["OrderNumber"] == DBNull.Value ? null : reader["OrderNumber"].ToString(),
                                ExpiryDate = reader["DueDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DueDate"]),
                                Amount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"]),
                                Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                                CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(), // not returned by SP as int — CreatedBy column appears empty
                                CreatedOn = reader["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"]),
                                Records = reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"]),
                                Items = new List<SalesInvoiceItem>()
                             };
                            result.SalesInvoiceHistory.Add(SaleInvoice);
                            lookup[Sale_InvoiceId] = SaleInvoice;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                int Sale_InvoiceId = reader["InvoiceId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["InvoiceId"]);

                                var item = new SalesInvoiceItem
                                {
                                    Sale_InvoiceId = Sale_InvoiceId,

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

                                if (lookup.ContainsKey(Sale_InvoiceId))
                                {
                                    lookup[Sale_InvoiceId].Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // ── Sales Order ──

        public SalesOrderHistoryFullResult GetSalesOrderHistory(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var result = new SalesOrderHistoryFullResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Sales.Get_SaleOrder", conn))
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
                    var lookup = new Dictionary<int, SalesOrderHistoryList>();
                    while (reader.Read())
                    {
                        int SaleOrderId = reader["Sale_OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Sale_OrderId"]);
                         var SaleOrder = new SalesOrderHistoryList
                         {
                            Sale_OrderId = reader["Sale_OrderId"] == DBNull.Value ? (int?)null : TryParseInt(reader["Sale_OrderId"]),
                            Sale_OrderNo = reader["OrderNumber"] == DBNull.Value ? null : reader["OrderNumber"].ToString(),
                            Sale_OrderDate = reader["OrderDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["OrderDate"]),
                            ShipmentDate = reader["ShipmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ShipmentDate"]),
                            CustomerName = reader["Customer"] == DBNull.Value ? null : reader["Customer"].ToString(),
                            Amount = reader["Amount"] == DBNull.Value ? (decimal?)null : TryParseDecimal(reader["Amount"]),
                            Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                            CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(),
                            CreatedOn = reader["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedOn"]),
                            Records = reader["Records"] == DBNull.Value ? (int?)null : TryParseInt(reader["Records"]),

                            Items = new List<SalesOrderListItem>()
                        };
                        result.SalesOrderHistory.Add(SaleOrder);
                        lookup[SaleOrderId] = SaleOrder;
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            int SaleOrderId = reader["Sale_OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Sale_OrderId"]);

                            var item = new SalesOrderListItem
                            {
                                Sale_OrderId = SaleOrderId,

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

                            if (lookup.ContainsKey(SaleOrderId))
                            {
                                lookup[SaleOrderId].Items.Add(item);
                            }
                        }
                    }

                }
            }

            return result;
        }


        // ── Credit Note ──

        public SalesCreditNoteHistoryFullResult GetSalesCreditNoteHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            var result = new SalesCreditNoteHistoryFullResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.Get_CreditNote", conn))
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
                    using (var reader = cmd.ExecuteReader())
                    {
                        var lookup = new Dictionary<int, SalesCreditNoteHistory>();
                        while (reader.Read())
                        {
                            int Sale_CrNoteId = reader["CreditNote_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreditNote_Id"]);
                            var SalesCreditNote = new SalesCreditNoteHistory
                             {
                                Sale_CrNoteId = reader["CreditNote_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreditNote_Id"]),
                                Sale_CrNoteNo = reader["CreditNoteNumber"] == DBNull.Value ? null : reader["CreditNoteNumber"].ToString(),
                                Sale_CrNoteDate = reader["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["Date"]),
                                CustomerName = reader["CustomerName"] == DBNull.Value ? null : reader["CustomerName"].ToString(),
                                ReferenceNumber = reader["InvoiceNumber"] == DBNull.Value ? null : reader["InvoiceNumber"].ToString(),
                                Amount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalAmount"]),
                                Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                                CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(), // not returned by SP as int — CreatedBy column appears empty
                                CreatedOn = reader["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"]),
                                Records = reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"]),

                                 Items = new List<SalesCreditNoteItem>()
                             };
                            result.SalesCreditNoteHistory.Add(SalesCreditNote);
                            lookup[Sale_CrNoteId] = SalesCreditNote;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                int Sale_CrNoteId = reader["CreditNote_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CreditNote_Id"]);

                                var item = new SalesCreditNoteItem
                                {
                                    Sale_CrNoteId = Sale_CrNoteId,

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

                                if (lookup.ContainsKey(Sale_CrNoteId))
                                {
                                    lookup[Sale_CrNoteId].Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }


        //--Recurring Invoice--

        public SalesRecInvHistoryFullResult GetSalesRecInvHistory(string statusFilter, int? entityId, int start, int length, string orderby, string search, string startDate, string endDate)
        {
            var result = new SalesRecInvHistoryFullResult();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.Get_RecurringInvoice", conn))
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
                    using (var reader = cmd.ExecuteReader())
                    {
                        var lookup = new Dictionary<int, SalesRecInvHistoryList>();
                        while (reader.Read())
                        {
                            int InvoiceId = reader["InvoiceId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            var SaleRecInv = new SalesRecInvHistoryList
                             {
                                InvoiceId = InvoiceId,
                                InvoiceNumber = reader["InvoiceNumber"] == DBNull.Value ? null : reader["InvoiceNumber"].ToString(),
                                PreviousInvoiceDate = reader["PreviousInvoiceDate"] == DBNull.Value ? null : reader["PreviousInvoiceDate"].ToString(),
                                NextInvoiceDate = reader["NextInvoiceDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["NextInvoiceDate"]),
                                EndDate = reader["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["EndDate"]),
                                CustomerName = reader["Customer"] == DBNull.Value ? null : reader["Customer"].ToString(),
                                Frequency = reader["Frequency"] == DBNull.Value ? null : reader["Frequency"].ToString(),
                                InvoiceAmount = reader["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Amount"]),
                                Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                                CreatedBy = reader["CreatedBy"] == DBNull.Value ? null : reader["CreatedBy"].ToString(), // not returned by SP as int — CreatedBy column appears empty
                                CreatedOn = reader["CreatedOn"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"]),
                                Records = reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"]),
                                Items = new List<SalesRecInvListItem>()
                             };
                            result.SalesRecInvHistory.Add(SaleRecInv);
                            lookup[InvoiceId] = SaleRecInv;
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                int InvoiceId = reader["InvoiceId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["InvoiceId"]);

                                var item = new SalesRecInvListItem
                                {
                                    InvoiceId = InvoiceId,

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

                                if (lookup.ContainsKey(InvoiceId))
                                {
                                    lookup[InvoiceId].Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}

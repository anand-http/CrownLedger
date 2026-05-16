using fintech;
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
    public class HomeDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;
        public string SaveOrUpdateCompany(GetCompanyDetails_Result model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.SaveOrUpdateCompany", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_Id);
                cmd.Parameters.AddWithValue("@Entity_ID", 10);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Industry", (object)model.Industry ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Website", (object)model.Website ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactPerson", (object)model.ContactPerson ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Telephone", (object)model.Telephone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PrimaryEmail", (object)model.PrimaryEmail ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TaxNumber", model.TaxNumber);
                cmd.Parameters.AddWithValue("@PAN", (object)model.PAN ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VAT", (object)model.VAT ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TimeZone", (object)model.TimeZone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Language", (object)model.LanguageId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address1", (object)model.Address1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address2", (object)model.Address2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CountryId", (object)model.Contry_Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StateId", (object)model.State_Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CityId", (object)model.City_Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ZipCode", (object)model.Zipcode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FiscalYearStart", (object)model.FiscalYearStart ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FiscalYearEnd", (object)model.FiscalYearEnd ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountingBasis", model.AccountingBasis);
                cmd.Parameters.AddWithValue("@BaseCurrencyId", (object)model.Currency_Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LogoPath", (object)model.LogoUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                //cmd.Parameters.AddWithValue("@LoginId", model.Login_Id);
                cmd.Parameters.AddWithValue("@LoginId", 1);

                // Output parameter
                SqlParameter messageCode = new SqlParameter("@Message_Code", SqlDbType.VarChar, 10);
                messageCode.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(messageCode);

                conn.Open();
                cmd.ExecuteNonQuery();

                return messageCode.Value?.ToString();
            }
        }

        public Company GetCompanyDetails(int id)
        {
            Company company = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Core.GetCompanyDetails", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        company = new Company
                        {
                            Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString(),
                            Address = reader["Address"] == DBNull.Value ? "" : reader["Address"].ToString(),
                            TaxNumber = reader["TaxNumber"] == DBNull.Value ? "" : reader["TaxNumber"].ToString(),
                            Industry = reader["Industry"] == DBNull.Value ? "" : reader["Industry"].ToString(),
                            Website = reader["Website"] == DBNull.Value ? "" : reader["Website"].ToString(),
                            ContactPerson = reader["ContactPerson"] == DBNull.Value ? "" : reader["ContactPerson"].ToString(),
                            Telephone = reader["Telephone"] == DBNull.Value ? "" : reader["Telephone"].ToString(),
                            PrimaryEmail = reader["PrimaryEmail"] == DBNull.Value ? "" : reader["PrimaryEmail"].ToString(),
                            PAN = reader["PAN"] == DBNull.Value ? "" : reader["PAN"].ToString(),
                            VAT = reader["VAT"] == DBNull.Value ? "" : reader["VAT"].ToString(),
                            TimeZone = reader["TimeZone"] == DBNull.Value ? "" : reader["TimeZone"].ToString(),
                            Language = reader["Language"] == DBNull.Value ? "" : reader["Language"].ToString(),
                            LanguageId = reader["LanguageId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LanguageId"]),
                            Address1 = reader["Address1"] == DBNull.Value ? "" : reader["Address1"].ToString(),
                            Address2 = reader["Address2"] == DBNull.Value ? "" : reader["Address2"].ToString(),
                            Zipcode = reader["ZipCode"] == DBNull.Value ? "" : reader["ZipCode"].ToString(),
                            Country = reader["Country"] == DBNull.Value ? "" : reader["Country"].ToString(),
                            Contry_Id = reader["CountryId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CountryId"]),
                            State = reader["State"] == DBNull.Value ? "" : reader["State"].ToString(),
                            State_Id = reader["StateId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["StateId"]),
                            City = reader["City"] == DBNull.Value ? "" : reader["City"].ToString(),
                            City_Id = reader["CityId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CityId"]),
                            AccountingBasis = reader["AccountingBasis"] == DBNull.Value ? "" : reader["AccountingBasis"].ToString(),
                            Accounting_Id = reader["AccountingBasis"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AccountingBasis"]),
                            DefaultCurrency = reader["Currency"] == DBNull.Value ? "" : reader["Currency"].ToString(),
                            Currency_Id = reader["CurrencyId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CurrencyId"]),
                            CreatedAt = reader["CreatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedAt"]),
                            FiscalYearStart = reader["FiscalYearStart"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FiscalYearStart"]),  // ✅ fixed wrong column
                            FiscalYearEnd = reader["FiscalYearEnd"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["FiscalYearEnd"]),      // ✅ fixed wrong column
                            Status = reader["Status"] == DBNull.Value ? "" : reader["Status"].ToString(),   // ✅ fixed wrong column
                            LogoUrl = reader["LogoPath"] == DBNull.Value ? "" : reader["LogoPath"].ToString()
                        };
                    }
                }
            }
            return company;
        }
        public List<sp_GetOpeningBalanceGridData_Result> GetOpeningBalanceGridData(DateTime date)
        {
            var balances = new List<sp_GetOpeningBalanceGridData_Result>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetOpeningBalanceGridData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MigrationDate", date);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        balances.Add(new sp_GetOpeningBalanceGridData_Result
                        {
                            OpeningBalanceId = Convert.ToInt32(reader["OpeningBalanceId"]),
                            LedgerAccountId = Convert.ToInt32(reader["LedgerAccountId"]),
                            LedgerAccountName = reader["LedgerAccountName"].ToString(),
                            AccountType = reader["AccountType"].ToString(),
                            Debit = reader["Debit"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Debit"]),
                            Credit = reader["Credit"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Credit"]),
                            MigrationDate = Convert.ToDateTime(reader["MigrationDate"])
                        });
                    }
                }
            }
            return balances;
        }

        public List<usp_GetBankList_Result> GetBankList()
        {
            var banks = new List<usp_GetBankList_Result>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("usp_GetBankList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        banks.Add(new usp_GetBankList_Result
                        {
                            BankId = Convert.ToInt32(reader["BankId"]),
                            BankName = reader["BankName"].ToString(),
                            ShortName = reader["ShortName"].ToString(),
                            BankType = reader["BankType"].ToString(),
                            IFSCCodePrefix = reader["IFSCCodePrefix"].ToString(),
                            Headquarters = reader["Headquarters"].ToString(),
                            Website = reader["Website"].ToString(),
                            LogoUrl = reader["LogoUrl"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                        });
                    }
                }
            }
            return banks;
        }

        public List<GetBankAccounts_Result> GetBankAccounts()
        {
            var accounts = new List<GetBankAccounts_Result>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("GetBankAccounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        accounts.Add(new GetBankAccounts_Result
                        {
                            AccountId = Convert.ToInt32(reader["AccountId"]),
                            BankName = reader["BankName"].ToString(),
                            AccountType = reader["AccountType"].ToString(),
                            AccountNumber = reader["AccountNumber"].ToString(),
                            AccountName = reader["AccountName"].ToString(),
                            Description = reader["Description"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            ModifiedDate = reader["ModifiedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ModifiedDate"])
                        });
                    }
                }
            }
            return accounts;
        }

        public int DeleteBankAccount(int accountId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("DeleteBankAccount", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountId", accountId);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public List<sp_GetProductAndService_Result> GetProductAndService(int? entityId, int start, int length, string orderby, string search, string statusFilter, string startDate, string endDate)
        {
            var products = new List<sp_GetProductAndService_Result>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.sp_GetProductAndService", conn))
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
                    while (reader.Read())
                    {
                        products.Add(new sp_GetProductAndService_Result
                        {
                            Id = Convert.ToInt32(reader["ItemId"]),
                            //Type = reader["ItemType"].ToString(),
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            ItemDescription = reader["ItemDescription"].ToString(),
                            UnitOfMeasurement = reader["Unit_Of_Measurement"].ToString(),
                            HSNCode = reader["HSNCode"].ToString(),
                            SellingPrice = reader["Selling_Price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Selling_Price"]),
                            CostPrice = reader["Cost_Price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["Cost_Price"]),
                            Records = Convert.ToInt32(reader["Records"])
                        });
                    }
                }
            }
            return products;
        }

        public List<GetAllChartOfAccounts_Result> GetAllChartOfAccounts(int? entityId, int start, int length, string orderby, string search, string statusFilter)
        {
            try
            {
                var accounts = new List<GetAllChartOfAccounts_Result>();
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("ACCT.Sp_GetCoreGeneralLedger", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@length", length);
                    cmd.Parameters.AddWithValue("@orderby", string.IsNullOrEmpty(orderby) ? "Order By GL_CODE asc" : orderby);
                    cmd.Parameters.AddWithValue("@search", search ?? (object)DBNull.Value);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accounts.Add(new GetAllChartOfAccounts_Result
                            {
                                GL_ID = Convert.ToInt32(reader["GL_ID"]),
                                GL_CODE = reader["GL_CODE"].ToString(),
                                GL_DESC = reader["GL_DESC"].ToString(),
                                GL_CLASS = reader["GL_CLASS"].ToString(),
                                Active = reader["Active"].ToString(),
                                Branch = reader["Branch"].ToString(),
                                Department = reader["Department"].ToString(),
                                Group_desc = reader["Group_desc"].ToString(),
                                GL_BRANCHID = Convert.ToInt32(reader["GL_BRANCHID"]),
                                GL_DeptmtID = Convert.ToInt32(reader["GL_DeptmtID"]),
                                Currency = reader["Currency"].ToString(),
                                GL_IC = Convert.ToBoolean(reader["GL_IC"]),
                                GL_Control = Convert.ToBoolean(reader["GL_Control"]),
                                GL_Bank = Convert.ToBoolean(reader["GL_Bank"]),
                                GL_R_Invoice = reader["GL_R_Invoice"].ToString(),
                                GL_R_Credit = reader["GL_R_Credit"].ToString(),
                                GL_R_Reciept = reader["GL_R_Reciept"].ToString(),
                                GL_P_Invoice = reader["GL_P_Invoice"].ToString(),
                                GL_P_Debit = reader["GL_P_Debit"].ToString(),
                                GL_P_Payment = reader["GL_P_Payment"].ToString(),
                                GL_G_Posting = reader["GL_G_Posting"].ToString(),
                                Curncy_ID = reader["Curncy_ID"].ToString(),
                                TotalRecord = Convert.ToInt32(reader["TotalRecord"]),
                            });
                        }
                    }
                }
                return accounts;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveOrUpdateGLCode(GetAllChartOfAccounts_Result model, string userName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("ACCT.Sp_mst_InsupdGeneralLedger", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Serialize model to XML
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(GetAllChartOfAccounts_Result));
                string xmlString = string.Empty;
                using (var stringWriter = new System.IO.StringWriter())
                {
                    xmlSerializer.Serialize(stringWriter, model);
                    xmlString = stringWriter.ToString();
                }

                cmd.Parameters.AddWithValue("@GL_XMLDATA ", xmlString);
                cmd.Parameters.AddWithValue("@GL_ID", model.GL_ID);
                cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_ID);
                cmd.Parameters.AddWithValue("@Login_ID", model.Login_ID);
                cmd.Parameters.AddWithValue("@Active", model.Active);

                SqlParameter outputParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 5);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                string messageCode = outputParam.Value?.ToString() ?? "E0001";
                System.Diagnostics.Debug.WriteLine("Message_code: " + messageCode);
                return messageCode;
            }
        }

        public string SaveOrUpdateProductAndService(sp_GetProductAndService_Result model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.sp_mst_InsUpdProduct_Service", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Product_ID", model.Id == 0 ? (object)DBNull.Value : model.Id);
                cmd.Parameters.AddWithValue("@Product_name", model.ItemName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Product_Desc", model.ItemDescription ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Product_UOM", model.UnitOfMeasurement ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Product_HSNCode", model.HSNCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Product_Taxability", string.IsNullOrEmpty(model.Taxability) ? null : model.Taxability);
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_Id);
                cmd.Parameters.AddWithValue("@Vendor_Id", string.IsNullOrEmpty(model.Vendor) ? 0 : Convert.ToInt32(model.Vendor));

                cmd.Parameters.AddWithValue("@InventoryAccount", string.IsNullOrEmpty(model.InventoryAccount) ? 0 : Convert.ToInt32(model.InventoryAccount));
                cmd.Parameters.AddWithValue("@As_On_Date", model.AsOnDate.HasValue ? (object)model.AsOnDate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Initial_Qty", model.InitialQtyOnHand ?? 0);
                cmd.Parameters.AddWithValue("@Reorder_Point", model.ReorderPoint ?? 0);
                cmd.Parameters.AddWithValue("@Consumption_Qty", 0);

                cmd.Parameters.AddWithValue("@Selling_Price", model.SellingPrice ?? 0);
                cmd.Parameters.AddWithValue("@Income_Account", string.IsNullOrEmpty(model.IncomeAccount) ? 0 : Convert.ToInt32(model.IncomeAccount));
                cmd.Parameters.AddWithValue("@Tax_GroupId", string.IsNullOrEmpty(model.SalesTaxGroup) ? 0 : Convert.ToInt32(model.SalesTaxGroup));
                cmd.Parameters.AddWithValue("@Cost_Price", model.CostPrice ?? 0);
                cmd.Parameters.AddWithValue("@Expense_Account", string.IsNullOrEmpty(model.ExpenseAccount) ? 0 : Convert.ToInt32(model.ExpenseAccount));
                cmd.Parameters.AddWithValue("@Input_TaxCredit", model.InputTaxCredit ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Purchase_TaxGroupId", string.IsNullOrEmpty(model.PurchaseTaxGroup) ? 0 : Convert.ToInt32(model.PurchaseTaxGroup));

                cmd.Parameters.AddWithValue("@Goods", model.Type == "G" ? 1 : 0);
                cmd.Parameters.AddWithValue("@Service", model.Type == "S" ? 1 : 0);
                cmd.Parameters.AddWithValue("@Login_ID", model.Login_Id);

                SqlParameter msgParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 5);
                msgParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(msgParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                return msgParam.Value?.ToString() ?? "500";
            }
        }
        public void AddNewTax(mst_Tax model)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_mst_InsupdTax", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Tax_ID", model.Tax_ID > 0 ? (object)model.Tax_ID : DBNull.Value);
                cmd.Parameters.AddWithValue("@Tax_Desc", (object)model.Tax_Desc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tax_factor", (object)model.Tax_factor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tax_type", (object)model.Tax_type ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Tax_Code", (object)model.Tax_Code ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_ID); // Replace with actual entity id
                cmd.Parameters.AddWithValue("@Login_ID", model.Login_ID); // Replace with actual login id
                cmd.Parameters.AddWithValue("@Active", model.Tax_ID > 0 ? model.Active : true); // Replace with actual login id
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<mst_Tax> GetTaxList(int? entityId, int start, int length, string orderby, string search, int gst, int tds, int other)
        {
            var taxes = new List<mst_Tax>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_GetTaxList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                cmd.Parameters.AddWithValue("@Entity_ID", entityId.HasValue ? (object)entityId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@length", length);
                cmd.Parameters.AddWithValue("@gst", gst);    // BIT
                cmd.Parameters.AddWithValue("@tds", tds);    // BIT
                cmd.Parameters.AddWithValue("@other", other); // BIT
                cmd.Parameters.AddWithValue("@orderby", string.IsNullOrEmpty(orderby) ? (object)DBNull.Value : orderby);
                cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    do
                    {
                        // Detect which result set we are reading
                        // by checking which columns are present in the current result set
                        var schema = reader.GetSchemaTable();
                        var columnNames = schema.Rows
                                                .Cast<System.Data.DataRow>()
                                                .Select(r => r["ColumnName"].ToString())
                                                .ToHashSet(StringComparer.OrdinalIgnoreCase);

                        while (reader.Read())
                        {
                            // ── Result Set 1: SGST (has Tax_Desc, Tax_type, no TDS_Section / Threshold_Limit / LedgerAccount) ──
                            if (columnNames.Contains("Tax_type") && !columnNames.Contains("TDS_Section"))
                            {
                                taxes.Add(new mst_Tax
                                {
                                    Tax_ID = reader["Tax_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Tax_ID"]),
                                    Tax_Code = reader["Tax_Code"] == DBNull.Value ? "" : reader["Tax_Code"].ToString(),
                                    Tax_Desc = reader["Tax_Desc"] == DBNull.Value ? "" : reader["Tax_Desc"].ToString(),
                                    Tax_factor = reader["Tax_Factor"] == DBNull.Value ? "" : reader["Tax_Factor"].ToString(),
                                    Tax_Value = FormatTaxValue(reader["Tax_factor"], reader["Tax_Value"]),
                                    Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                                    Records = reader.GetSchemaTable().Rows.Cast<System.Data.DataRow>()
                                                    .Any(r => r["ColumnName"].ToString() == "Records")
                                                    ? (reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"]))
                                                    : 0,
                                    TaxCategory = "GST"   // Tag so caller knows the source
                                });
                            }
                            // ── Result Set 2: TDS-Contractors (has TDS_Section, Threshold_Limit, LedgerAccount) ──
                            else if (columnNames.Contains("TDS_Section"))
                            {
                                taxes.Add(new mst_Tax
                                {
                                    Tax_ID = reader["Tax_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Tax_ID"]),
                                    Tax_Code = reader["Tax_Code"] == DBNull.Value ? "" : reader["Tax_Code"].ToString(),
                                    Tax_factor = reader["Tax_Factor"] == DBNull.Value ? "" : reader["Tax_Factor"].ToString(),
                                    TDS_Section = reader["TDS_Section"] == DBNull.Value ? "" : reader["TDS_Section"].ToString(),
                                    Tax_Value = FormatTaxValue(reader["Tax_Factor"], reader["Tax_Value"]),
                                    Threshold_Limit = reader["Threshold_Limit"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Threshold_Limit"]),
                                    LedgerAccount = reader["LedgerAccount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["LedgerAccount"]),
                                    Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                                    Records = reader.GetSchemaTable().Rows.Cast<System.Data.DataRow>()
                                                        .Any(r => r["ColumnName"].ToString() == "Records")
                                                        ? (reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"]))
                                                        : 0,
                                    TaxCategory = "TDS"  // Tag so caller knows the source
                                });
                            }
                            // ── Result Set 3: Other/GST (has Tax_type but Tax_Value can be NULL) ──
                            else if (columnNames.Contains("Tax_type") && !columnNames.Contains("TDS_Section"))
                            {
                                taxes.Add(new mst_Tax
                                {
                                    Tax_ID = reader["Tax_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Tax_Id"]),
                                    Tax_Code = reader["Tax_Code"] == DBNull.Value ? "" : reader["Tax_Code"].ToString(),
                                    Tax_Desc = reader["Tax_Desc"] == DBNull.Value ? "" : reader["Tax_Desc"].ToString(),
                                    Tax_factor = reader["Tax_Factor"] == DBNull.Value ? "" : reader["Tax_Factor"].ToString(),
                                    Tax_Value = reader["Tax_Value"] == DBNull.Value ? "N/A" : FormatTaxValue(reader["Tax_Factor"], reader["Tax_Value"]),
                                    Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                                    Records = reader.GetSchemaTable().Rows.Cast<System.Data.DataRow>()
                                                    .Any(r => r["ColumnName"].ToString() == "Records")
                                                    ? (reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"]))
                                                    : 0,
                                    TaxCategory = "OTHER"
                                });
                            }
                        }

                    } while (reader.NextResult()); // Move to the next result set
                }
            }

            return taxes;
        }

        private string FormatTaxValue(object taxFactor, object taxValue)
        {
            if (taxValue == DBNull.Value || taxValue == null)
                return "";

            // Parse the raw numeric value
            if (!decimal.TryParse(taxValue.ToString(), out decimal numericValue))
                return taxValue.ToString();

            string factor = (taxFactor == DBNull.Value || taxFactor == null)
                ? ""
                : taxFactor.ToString().Trim().ToLower();

            // If percentage → strip decimals and append %
            if (factor == "percentage")
                return ((int)numericValue).ToString() + "%";    // 50.0000 → "50%"

            // For all other factors → show as plain decimal (no trailing zeros)
            return numericValue.ToString("G29");                // 9.0000 → "9"
        }

        public mst_Currency GetCurrencyList(int? entityId, int start, int length, string orderby, string search)
        {
            var result = new mst_Currency
            {
                CurrencyList = new List<CurrencyList>(),
                ExchangeRate = new List<CurrencyExchangeRate>()
            };

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_GetCurrencyList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Entity_ID", entityId.HasValue ? (object)entityId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@length", length);
                cmd.Parameters.AddWithValue("@orderby", string.IsNullOrEmpty(orderby) ? (object)DBNull.Value : orderby);
                cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // First result set: CurrencyList
                    while (reader.Read())
                    {
                        result.CurrencyList.Add(new CurrencyList
                        {
                            Curncy_ID = reader["Curncy_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Curncy_ID"]),
                            Curncy_Code = reader["Curncy_Code"] == DBNull.Value ? "" : reader["Curncy_Code"].ToString(),
                            Curncy_Desc = reader["Curncy_Desc"] == DBNull.Value ? "" : reader["Curncy_Desc"].ToString(),
                            Curncy_Major = reader["Curncy_Major"] == DBNull.Value ? "" : reader["Curncy_Major"].ToString(),
                            Curncy_Minor = reader["Curncy_Minor"] == DBNull.Value ? "" : reader["Curncy_Minor"].ToString(),
                            Curncy_Symbol = reader["Curncy_Symbol"] == DBNull.Value ? "" : reader["Curncy_Symbol"].ToString(),
                            Curncy_Seperator = reader["Curncy_Seperator"] == DBNull.Value ? "" : reader["Curncy_Seperator"].ToString(),
                            Curncy_Format = reader["Curncy_Format"] == DBNull.Value ? "" : reader["Curncy_Format"].ToString(),
                            Curncy_Decimal = reader["Curncy_Decimal"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Curncy_Decimal"]),
                            Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                            timestamp = reader["timestamp"] == DBNull.Value ? null : (byte[])reader["timestamp"],
                            TotalRecord = reader["TotalRecord"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalRecord"])
                        });
                    }

                    // Second result set: CurrencyExchangeRate
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            result.ExchangeRate.Add(new CurrencyExchangeRate
                            {
                                ID = reader["Row"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Row"]),
                                FromCurrency = reader["FromCurrency"] == DBNull.Value ? "" : reader["FromCurrency"].ToString(),
                                ToCurrency = reader["ToCurrency"] == DBNull.Value ? "" : reader["ToCurrency"].ToString(),
                                RateType = reader["RateType"] == DBNull.Value ? "" : reader["RateType"].ToString(),
                                EffectiveDate = reader["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["EffectiveDate"]),
                                MultDiv = reader["Mult_Div"] == DBNull.Value ? "" : reader["Mult_Div"].ToString(),
                                ExchangeRate = reader["ExchangeRate"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["ExchangeRate"]),
                                RateReciprocal = reader["RateReciprocal"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["RateReciprocal"]),
                                CurrencyUnitEquivalents = reader["CurrencyUnitEquivalents"] == DBNull.Value ? "" : reader["CurrencyUnitEquivalents"].ToString(),
                                TotalRecord = reader["TotalRecord"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalRecord"])
                            });
                        }
                    }
                }
            }

            return result;
        }

        public bool SaveTax(mst_Tax model, List<TaxDetail> details)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // ── Convert Details List to XML ──
                        // SP expects: <root><row AssociatedTax="" TaxRate="" TaxAccount="" TaxAccountCreditNote=""/></root>
                        XElement xml = new XElement("root",
                            details.Select(d => new XElement("row",
                                new XAttribute("AssociatedTax", d.AssociatedTax ?? ""),
                                new XAttribute("TaxRate", d.TaxRate ?? ""),
                                new XAttribute("TaxAccount", d.TaxAccount ?? ""),
                                new XAttribute("TaxAccountCreditNote", d.TaxAccountCreditNote ?? "")
                            ))
                        );

                        using (SqlCommand cmd = new SqlCommand("CORE.Sp_mst_InsupdTax", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Tax_ID", model.Tax_ID);
                            cmd.Parameters.AddWithValue("@Tax_Desc", model.Tax_Desc ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Tax_Rate", model.Tax_Value ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ComputationBasis", model.ComputationBasis);
                            cmd.Parameters.AddWithValue("@Tax_Type", model.Tax_type ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@TdsType", model.TdsType);
                            cmd.Parameters.AddWithValue("@TdsSection", model.TDS_Section ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ThresholdLimit", model.Threshold_Limit.HasValue ? (object)model.Threshold_Limit.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@LtdcApplied", model.Tax_Include);                          // extend if needed
                            cmd.Parameters.AddWithValue("@Ltdc_EffectiveDate", model.LTDC_EffectiveDate);                   // extend if needed
                            cmd.Parameters.AddWithValue("@Ltdc_EffectiveAmount", model.EffectiveAmount);                           // extend if needed
                            cmd.Parameters.AddWithValue("@Ltdc_number", model.CertificateNo);                             // extend if needed
                            cmd.Parameters.AddWithValue("@Tax_GLIN", model.Tax_GLIN.HasValue ? (object)model.Tax_GLIN.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Tax_GLOUT", model.Tax_GLOUT.HasValue ? (object)model.Tax_GLOUT.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@Tax_Code", model.Tax_Code ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@EffectiveFrom", model.EffectiveFrom);
                            cmd.Parameters.AddWithValue("@Tax_factor", model.Tax_factor ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@Tax_appliedon", model.Tax_appliedon);
                            cmd.Parameters.AddWithValue("@Tax_Include", model.Tax_Include);
                            cmd.Parameters.AddWithValue("@Is_InterState", model.Is_InterState);
                            cmd.Parameters.AddWithValue("@Tax_Seq", model.Tax_Seq);
                            cmd.Parameters.AddWithValue("@withHold", model.Tax_WithHold);
                            cmd.Parameters.AddWithValue("@Active", model.Active);
                            cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_ID);
                            cmd.Parameters.AddWithValue("@Login_ID", model.Login_ID);
                            cmd.Parameters.AddWithValue("@gst", model.TaxCategory == "GST" ? 1 : 0);
                            cmd.Parameters.AddWithValue("@tds", model.TaxCategory == "TDS" ? 1 : 0);
                            cmd.Parameters.AddWithValue("@others", model.TaxCategory == "OTHER" ? 1 : 0);
                            cmd.Parameters.AddWithValue("@Isgroup", model.Isgroup);
                            cmd.Parameters.AddWithValue("@xml_data", xml.ToString());

                            // ── Output parameter ──
                            SqlParameter messageCode = new SqlParameter("@Message_code", SqlDbType.VarChar, 5);
                            messageCode.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(messageCode);

                            cmd.ExecuteNonQuery();

                            // ── Check SP output message ──
                            string resultCode = messageCode.Value?.ToString();
                            if (resultCode != "I0001")
                                throw new Exception($"SP returned error code: {resultCode}");
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool SaveExchangeRate(ExchangeRateViewModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("core.sp_mst_InsupdExchangeRate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Exrate_SourceID", model.FromCurrencyId);
                cmd.Parameters.AddWithValue("@Exrate_targetID", model.ToCurrencyId);
                cmd.Parameters.AddWithValue("@Exrate_ExTypeID", model.CurrencyRateTypeId);
                cmd.Parameters.AddWithValue("@Exrate_FExDate", model.EffectivefromDate);
                cmd.Parameters.AddWithValue("@Exrate_TExDate", model.EffectivefromDate);
                cmd.Parameters.AddWithValue("@Exrate_ExRate", model.CurrencyRate);
                cmd.Parameters.AddWithValue("@Exrate_ExFactor", model.MultDiv);
                cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_ID);
                cmd.Parameters.AddWithValue("@Login_ID", model.Login_ID);

                SqlParameter outParam = new SqlParameter("@Message_code", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                con.Open();
                cmd.ExecuteNonQuery();

                return (int)outParam.Value == 1;
            }
        }

        public string SaveRateType(CurrencyRateType model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[CORE].[Sp_mst_InsupdExchangeType]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExType_Code", model.RateTypeCode);
                cmd.Parameters.AddWithValue("@ExType_Desc", model.Description);
                cmd.Parameters.AddWithValue("@Active", model.RefreshOnline);
                cmd.Parameters.AddWithValue("@Entity_ID", model.Entity_ID);
                cmd.Parameters.AddWithValue("@Login_ID", model.Login_ID);

                SqlParameter outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 5)
                { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outParam);

                con.Open();
                cmd.ExecuteNonQuery();
                return Convert.ToString(outParam.Value);
            }
        }
    }
}

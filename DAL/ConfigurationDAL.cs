using DocumentFormat.OpenXml.EMMA;
using fintech;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static fintech.Models.ConfigurationModel;

namespace fintech.DAL
{
    public class ConfigurationDAL
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;

        public GetBranchDetails_Result GetBranchesList(int? entityId,int id, int start, int length, string orderby)
        {
            var branches = new GetBranchDetails_Result();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_GetBranchesList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BranchId", id);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        branches.BranchDetail.Add(new BranchDetailList
                        {
                            Branch_ID = Convert.ToInt32(reader["Branch_ID"]),
                            Branch_Code = reader["Branch_Code"].ToString(),
                            Branch_Desc = reader["Branch_Desc"].ToString(),
                            Branch_CityId = reader["Branch_CityId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Branch_CityId"]),
                            Branch_Private = reader["Branch_Private"] == DBNull.Value ? false : Convert.ToBoolean(reader["Branch_Private"]),
                            Branch_Group = reader["Branch_Group"] == DBNull.Value ? false : Convert.ToBoolean(reader["Branch_Group"]),
                            GSTNO = reader["GSTNO"].ToString(),
                            Branch_Address1 = reader["Branch_Address1"].ToString(),
                            City = reader["City"].ToString(),
                            Country = reader["Country"].ToString(),
                            State = reader["State"].ToString(),
                            ZipCode = reader["ZipCode"].ToString(),
                            Branch_Address2 = reader["Branch_Address2"].ToString(),
                            Branch_Address3 = reader["Branch_Address3"].ToString(),
                            Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                            Branch_DivID = reader["Branch_DivID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Branch_DivID"]),
                            Branch_Div = reader["Branch_Div"].ToString(),
                        });
                    }
                }
            }
            return branches;
        }

        public bool UpdateBranch(bool isChecked, int? branchId, bool isDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                if (isDelete)
                {
                    cmd.CommandText = "UPDATE core.mst_Branch SET isDeleted = @IsChecked WHERE Branch_ID = @BranchId";
                    cmd.Parameters.AddWithValue("@IsChecked", isChecked);
                }
                else
                {
                    cmd.CommandText = "UPDATE core.mst_Branch SET Active = @IsChecked WHERE Branch_ID = @BranchId";
                    cmd.Parameters.AddWithValue("@IsChecked", isChecked);
                }

                cmd.Parameters.AddWithValue("@BranchId", branchId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public bool DeleteUser(int? userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                cmd.CommandText = "update UMS.mst_User set Active = 0 where User_Id = @UserId";

                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public bool UpdateDepartment(bool isChecked, int? departmentId, bool isDelete = false)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                if (isDelete)
                {
                    cmd.CommandText = "UPDATE core.mst_Department SET IsDeleted= @IsChecked WHERE Deptmt_ID= @DepartmentId";
                    cmd.Parameters.AddWithValue("@IsChecked", isChecked);
                }
                else
                {
                    cmd.CommandText = "UPDATE core.mst_Department SET Active = @IsChecked WHERE Deptmt_ID= @DepartmentId";
                    cmd.Parameters.AddWithValue("@IsChecked", isChecked);
                }

                cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public GetBranchDetails_Result GetBranchesList(int? entityId, int start, int length, string orderby)
        {
            var branches = new GetBranchDetails_Result();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_GetBranchesList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        branches.BranchDetail.Add(new BranchDetailList
                        {
                            Branch_ID = Convert.ToInt32(reader["Branch_ID"]),
                            Branch_Code = reader["Branch_Code"].ToString(),
                            Branch_Desc = reader["Branch_Desc"].ToString(),
                            Branch_Address1 = reader["Branch_Address1"].ToString(),
                            Branch_Address2 = reader["Branch_Address2"].ToString(),
                            Branch_Address3 = reader["Branch_Address3"].ToString(),
                            City = reader["City"].ToString(),
                            Country = reader["Country"].ToString(),
                            State = reader["State"].ToString(),
                            ZipCode = reader["ZipCode"].ToString(),
                            Branch_Div = reader["Branch_Div"].ToString(),
                            GSTNO = reader["GSTNO"].ToString(),
                            Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                            Records = reader["TotalRecord"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalRecord"])
                        });
                    }
                }
            }
            return branches;
        }

        public DocumentSequenceResult GetNumberSeriesList()
        {
            var result = new DocumentSequenceResult();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("core.sp_GetSeries", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Series.Add(new DocumentSequence
                        {
                            SeriesId = reader["SeriesId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SeriesId"]),
                            SeriesName = reader["SeriesName"].ToString(),
                            FiscalYear = reader["FiscalYear"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FiscalYear"]),
                            TypeName = reader["TypeName"].ToString(),
                            Prefix = reader["Prefix"].ToString(),
                            Value = reader["Value"].ToString(),
                            Restarting_Number = reader["Restarting_Number"].ToString()
                        });
                    }
                }
            }

            return result;
        }

        public bool SaveBranch(BranchDetailCreate model, int? entityId, int? loginId, out string messageCode, string actionType)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_mst_InsupdBranch", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);

                cmd.Parameters.AddWithValue("@Branch_ID", model.Branch_ID == 0 ? (object)DBNull.Value : model.Branch_ID);
                cmd.Parameters.AddWithValue("@Branch_Desc", model.Branch_Desc);
                cmd.Parameters.AddWithValue("@CityId", model.CityId);
                cmd.Parameters.AddWithValue("@Branch_Private", model.Branch_Private ? 1 : 0);
                cmd.Parameters.AddWithValue("@Branch_Group", model.Branch_Group ? 1 : 0);
                cmd.Parameters.AddWithValue("@GSTNO", model.GSTNO);
                cmd.Parameters.AddWithValue("@ZipCode", model.ZipCode);
                cmd.Parameters.AddWithValue("@Branch_Address1", model.Branch_Address1);
                cmd.Parameters.AddWithValue("@Branch_Address2", model.Branch_Address2);
                cmd.Parameters.AddWithValue("@Branch_Address3", model.Branch_Address3);
                cmd.Parameters.AddWithValue("@CreatedBy", loginId);
                cmd.Parameters.AddWithValue("@ChangeBy", DBNull.Value);
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Branch_DivID", model.Branch_DivID == 1 ? (object)DBNull.Value : model.Branch_DivID);

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
            return isSuccess;
        }

        public bool SaveUserAccess(SaveUserAccessRequest model, int? entityId, int? loginId, out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("UMS.sp_SaveUserPermission", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                    cmd.Parameters.AddWithValue("@LoginId", loginId);

                    cmd.Parameters.AddWithValue("@UserName", model.UserName?? "");
                    cmd.Parameters.AddWithValue("@Email", model.Email ?? "");


                    var dt = new DataTable();
                    dt.Columns.Add("ModuleDesc", typeof(string));
                    dt.Columns.Add("IsRead", typeof(bool));
                    dt.Columns.Add("IsWrite", typeof(bool));
            

                    foreach (var item in model.ModuleAccess)
                    {
                        dt.Rows.Add(
                            item.ModuleDesc,
                            item.IsRead,
                            item.IsWrite
                        );
                    }

                    var tvpParam = cmd.Parameters.AddWithValue("@UserPermission", dt);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "ums.Permission";

                    var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    messageCode = outParam.Value?.ToString();
                    isSuccess = messageCode == "I0001" || messageCode == "U0001";
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in SaveUserAccess: " + ex.Message);
            }
        }
        public GetDepartmentResult GetDepartmentList(int? entityId, int start, int length, string orderby, string search)
        {
            var departments = new GetDepartmentResult();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_GetDepartmentList", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departments.DepartmentDetail.Add(new DepartmenList
                        {
                            Deptmt_ID = Convert.ToInt32(reader["Deptmt_ID"]),
                            Deptmt_Desc = reader["Deptmt_Desc"].ToString(),
                            Deptmt_Code = reader["Deptmt_Code"].ToString(),
                            Deptmt_Group = reader["Deptmt_Group"] == DBNull.Value ? false : Convert.ToBoolean(reader["Deptmt_Group"]),
                            Deptmt_Private = reader["Deptmt_Private"] == DBNull.Value ? false : Convert.ToBoolean(reader["Deptmt_Private"]),
                            Active = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                            Records = reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"])
                        });
                    }
                }
            }
            return departments;
        }

        public GetbusscalenResult GetBussinessCalendarList(int? entityId, int start, int length, string orderby, string search)
        {
            var bussinesscalendar = new GetbusscalenResult();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[CORE].[Sp_GetBusinessCalendarList]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                cmd.Parameters.AddWithValue("@OrderBy", orderby);
                cmd.Parameters.AddWithValue("@Search", search ?? (object)DBNull.Value);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bussinesscalendar.bussCalDetail.Add(new PeriodModel
                        {
                            DocumentId = reader["BusCal_ID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BusCal_ID"]),
                            //CalendarType = reader["BusCal_Period"]?.ToString(),
                            FromDate = reader["BusCal_From"]?.ToString(),
                            ToDate = reader["BusCal_To"]?.ToString(),
                            Period = reader["BusCal_Period"]?.ToString(),
                            IsActive = reader["Active"] == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                            CreatedBy = reader["timestamp"]?.ToString(),
                            ModifiedBy = string.Empty  // No matching column; set default or map as needed
                        });
                    }
                }
            }
            return bussinesscalendar;
        }

        // ── Save (Insert / Update) ──
        public string SavePeriod(SavePeriodModel objPeriod)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("[CORE].[Sp_mst_InsupdBusinessCalendar]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@BusCal_ID", objPeriod.DocumentId);
                        cmd.Parameters.AddWithValue("@BusCal_Type", objPeriod.CalendarType);
                        cmd.Parameters.AddWithValue("@BusCal_From", objPeriod.FromDate);
                        cmd.Parameters.AddWithValue("@BusCal_To", objPeriod.ToDate);
                        cmd.Parameters.AddWithValue("@BusCal_Period", objPeriod.Period);
                        cmd.Parameters.AddWithValue("@Active", objPeriod.IsActive);
                        cmd.Parameters.AddWithValue("@Entity_ID", objPeriod.Entity_ID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Login_ID", objPeriod.Login_ID ?? (object)DBNull.Value);
                        //cmd.Parameters.AddWithValue("@timestamp", DBNull.Value);

                        // ✅ Output parameter
                        var messageCode = new SqlParameter("@Message_code", SqlDbType.VarChar, 5);
                        messageCode.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(messageCode);

                        cmd.ExecuteNonQuery();

                        // ✅ Read output
                        string result = messageCode.Value?.ToString();
                        return result == "S0001" ? "success" : "error";
                    }
                }
            }
            catch (Exception ex)
            {
                return "error";
            }
        }


        // ── Check Duplicate ──
        public  bool IsDuplicatePeriod(string period, int documentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("[CORE].[Sp_mst_InsupdBusinessCalendar]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Period", period);
                        cmd.Parameters.AddWithValue("@DocumentId", documentId);

                        var count = cmd.ExecuteScalar();
                        return Convert.ToInt32(count) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // ── Delete ──
        public  string DeletePeriod(int documentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("[CORE].[Sp_mst_InsupdBusinessCalendar]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DocumentId", documentId);

                        var result = cmd.ExecuteScalar()?.ToString();
                        return result == "1" ? "success" : "error";
                    }
                }
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public bool DepartmentAction(Department model, int?entityId,int? loginId,out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("CORE.Sp_mst_InsupdDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Login_ID", loginId);
                cmd.Parameters.AddWithValue("@Deptmt_ID", model.Deptmt_ID);
                cmd.Parameters.AddWithValue("@Deptmt_Desc", model.Deptmt_Desc);
                cmd.Parameters.AddWithValue("@Deptmt_Code", model.Deptmt_Code);
                cmd.Parameters.AddWithValue("@Deptmt_Private", model.Deptmt_Private);
                cmd.Parameters.AddWithValue("@Deptmt_Group", model.Deptmt_Group);
                cmd.Parameters.AddWithValue("@Active", model.Active);

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
            return isSuccess;
        }

        public GetUserAccess_Result GetUserAccess(int? entityId, int start, int length)
        {
            var userAccess = new GetUserAccess_Result();
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("UMS.sp_GetUserPermission", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Start", start);
                cmd.Parameters.AddWithValue("@Length", length);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userAccess.UserAccessDetail.Add(new UserAccessDetailList
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            UserName = reader["UserName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Role = reader["Role"].ToString(),
                            Records = reader["Records"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Records"])
                        });
                    }
                }
            }
            return userAccess;
        }

        public SaveUserAccessRequest GetUserDetails(int userId ,int? entityId, int start,int length)
        {
            SaveUserAccessRequest user = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("UMS.sp_GetUserPermission", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30;
                    cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Start", start);
                    cmd.Parameters.AddWithValue("@Length", length);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Read all module access records for this user
                        while (reader.Read())
                        {
                            // Initialize user object on first read
                            if (user == null)
                            {
                                user = new SaveUserAccessRequest
                                {
                                    UserName = reader["User_DName"].ToString(),
                                    Email = reader["User_Email"].ToString(),
                                    ModuleAccess = new List<UserModuleAccessItem>()
                                };
                            }

                            // Add module access
                            user.ModuleAccess.Add(new UserModuleAccessItem
                            {
                                ModuleDesc = reader["Module_Desc"].ToString(),
                                IsRead = Convert.ToBoolean(reader["IsRead"]),
                                IsWrite = Convert.ToBoolean(reader["IsWrite"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetUserDetails: " + ex.Message);
            }

            return user;
        }

        public bool SaveTransactionSettings(TransactionSettingsRequest request, int? entityId, int? loginId, out string messageCode, string customerAgingFrom, string customerAgingTo,string vendorAgingFrom, string vendorAgingTo)
        {
            bool isSuccess = false;
            messageCode = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Acct.Sp_InsUpdTransactionSetting", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@LoginId", loginId);

                // Customer Settings
                cmd.Parameters.AddWithValue("@CustInvoiceDueType", request.CustomerSettings.InvoiceDueType);
                cmd.Parameters.AddWithValue("@CustInvoiceDueDays", request.CustomerSettings.InvoiceDueDays);
                cmd.Parameters.AddWithValue("@CustAgingFrom", customerAgingFrom);
                cmd.Parameters.AddWithValue("@CustAgingTo", customerAgingTo);
                cmd.Parameters.AddWithValue("@CustSalesLedger", request.CustomerSettings.SalesLedger);
                cmd.Parameters.AddWithValue("@CustDiscountLedger", request.CustomerSettings.DiscountLedger);

                // Vendor Settings
                cmd.Parameters.AddWithValue("@VendInvoiceDueType", request.VendorSettings.InvoiceDueType);
                cmd.Parameters.AddWithValue("@VendInvoiceDueDays", request.VendorSettings.InvoiceDueDays);
                cmd.Parameters.AddWithValue("@VendAgingFrom", vendorAgingFrom);
                cmd.Parameters.AddWithValue("@VendAgingTo", vendorAgingTo);
                cmd.Parameters.AddWithValue("@VendExpenseLedger", request.VendorSettings.ExpenseLedger);
                cmd.Parameters.AddWithValue("@VendDiscountLedger", request.VendorSettings.DiscountLedger);
                // Payment & Receipt Settings
                var pr = request.PaymentReceiptSettings;
                cmd.Parameters.AddWithValue("@OtherReceiptLedger", pr?.OtherReceiptLedger);
                cmd.Parameters.AddWithValue("@OtherPaymentLedger", pr?.OtherPaymentLedger);
                cmd.Parameters.AddWithValue("@CustReceiptDiscountLedger", pr?.CustomerReceiptDiscountLedger);
                cmd.Parameters.AddWithValue("@VendPaymentDiscountLedger", pr?.VendorPaymentDiscountLedger);
                cmd.Parameters.AddWithValue("@BankInterestReceivedLedger", pr?.BankInterestReceivedLedger);
                cmd.Parameters.AddWithValue("@BankInterestChargesLedger", pr?.BankInterestChargesLedger);

                // Product & Service Settings
                var ps = request.ProductServiceSettings;
                cmd.Parameters.AddWithValue("@NonStockSalesAccount", ps?.NonStockSalesAccount);
                cmd.Parameters.AddWithValue("@NonStockExpenseAccount", ps?.NonStockExpenseAccount);
                cmd.Parameters.AddWithValue("@StockExpenseAccount", ps?.StockExpenseAccount);
                cmd.Parameters.AddWithValue("@ServiceSalesAccount", ps?.ServiceSalesAccount);
                cmd.Parameters.AddWithValue("@ServiceExpenseAccount", ps?.ServiceExpenseAccount);
                cmd.Parameters.AddWithValue("@AllowDuplicateItemNames", ps?.AllowDuplicateItemNames ?? false);

                // Accounting Settings
                cmd.Parameters.AddWithValue("@ShowNegativeAssets", request.AccountingSettings?.ShowNegativeAssets);

                // Currency Settings
                cmd.Parameters.AddWithValue("@EnableMultiCurrency", request.CurrencySettings?.EnableMultiCurrency ?? false);

                // Output param
                var outParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 20)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);

                con.Open();
                cmd.ExecuteNonQuery();

                messageCode = outParam.Value?.ToString();
                isSuccess = messageCode == "I0001" || messageCode == "U0001";
                return isSuccess;
            }
        }

        public TransactionSettingsResponse TransactionSettings(int? entityId, int? loginId)
        {
            var result = new TransactionSettingsResponse();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[ACCT].[Sp_GetTransactionSetting]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    TransactionSettingsData settings = null;

                    if (reader.Read())
                    {
                        settings = new TransactionSettingsData
                        {
                            // Customer
                            CustomerInvoiceDueType = reader["C_InvDue_Isday"]?.ToString(),
                            CustomerInvoiceDueDays = reader["C_InvDue_days"] == DBNull.Value ? 0 : Convert.ToInt32(reader["C_InvDue_days"]),

                            SalesLedgerId = reader["C_Sale_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["C_Sale_GLID"]),
                            SalesLedger = reader["C_Sale_Ledger"]?.ToString(),

                            DiscountLedgerId = reader["C_SaleDiscnt_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["C_SaleDiscnt_GLID"]),
                            DiscountLedger = reader["C_SaleDiscnt_Ledger"]?.ToString(),

                            OtherReceiptLedgerId = reader["C_Rec_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["C_Rec_GLID"]),
                            OtherReceiptLedger = reader["C_Rec_Ledger"]?.ToString(),

                            CustomerReceiptDiscountLedgerId = reader["C_Rec_Discnt_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["C_Rec_Discnt_GLID"]),
                            CustomerReceiptDiscountLedger = reader["C_Rec_Discnt_Ledger"]?.ToString(),

                            // Vendor
                            VendorInvoiceDueType = reader["P_InvDue_Isday"]?.ToString(),
                            VendorInvoiceDueDays = reader["P_InvDue_days"] == DBNull.Value ? 0 : Convert.ToInt32(reader["P_InvDue_days"]),

                            VendorExpenseLedgerId = reader["P_Sale_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["P_Sale_GLID"]),
                            VendorExpenseLedger = reader["P_Sale_Ledger"]?.ToString(),

                            VendorDiscountLedgerId = reader["P_SaleDiscnt_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["P_SaleDiscnt_GLID"]),
                            VendorDiscountLedger = reader["P_SaleDiscnt_Ledger"]?.ToString(),

                            OtherPaymentLedgerId = reader["P_Pay_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["P_Pay_GLID"]),
                            OtherPaymentLedger = reader["P_Pay_Ledger"]?.ToString(),

                            VendorPaymentDiscountLedgerId = reader["P_Pay_Discnt_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["P_Pay_Discnt_GLID"]),
                            VendorPaymentDiscountLedger = reader["P_Pay_Discnt_Ledger"]?.ToString(),

                            // Bank
                            BankInterestReceivedLedgerId = reader["BA_IntrstRec_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BA_IntrstRec_GLID"]),
                            BankInterestReceivedLedger = reader["BA_IntrstRec_Ledger"]?.ToString(),

                            BankInterestChargesLedgerId = reader["BA_IntrstPaid_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BA_IntrstPaid_GLID"]),
                            BankInterestChargesLedger = reader["BA_IntrstPaid_Ledger"]?.ToString(),

                            // Product / Service
                            NonStockSalesAccountId = reader["NonStock_Sales_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NonStock_Sales_GLID"]),
                            NonStockSalesAccount = reader["NonStock_Sales_Ledger"]?.ToString(),

                            NonStockExpenseAccountId = reader["NonStock_Expense_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NonStock_Expense_GLID"]),
                            NonStockExpenseAccount = reader["NonStock_Expense_Ledger"]?.ToString(),

                            StockExpenseAccountId = reader["Stock_Expense_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Stock_Expense_GLID"]),
                            StockExpenseAccount = reader["Stock_Expense_Ledger"]?.ToString(),

                            ServiceSalesAccountId = reader["Service_Sales_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Service_Sales_GLID"]),
                            ServiceSalesAccount = reader["Service_Sales_Ledger"]?.ToString(),

                            ServiceExpenseAccountId = reader["Service_Expense_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Service_Expense_GLID"]),
                            ServiceExpenseAccount = reader["Service_Expense_Ledger"]?.ToString(),

                            // Accounting
                            AllowDuplicateItemNames = reader["IsAllow_Dupl_Item"] != DBNull.Value && Convert.ToBoolean(reader["IsAllow_Dupl_Item"]),

                            ShowNegativeAssetsId = reader["Negative_Asset_GLID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Negative_Asset_GLID"]),
                            ShowNegativeAssets = reader["Negative_Asset_Ledger"]?.ToString(),

                            EnableMultiCurrency = reader["Is_MultiCurncy"] != DBNull.Value && Convert.ToBoolean(reader["Is_MultiCurncy"]),

                            Aging = new List<TransactionSettingsAgingData>()
                        };

                        result.TransactionSettings.Add(settings);
                    }

                    if (reader.NextResult() && settings != null)
                    {
                        var aging = new TransactionSettingsAgingData
                        {
                            CustomerAgingFrom = new List<string>(),
                            CustomerAgingTo = new List<string>(),
                            VendorAgingFrom = new List<string>(),
                            VendorAgingTo = new List<string>()
                        };

                        while (reader.Read())
                        {
                            string type = reader["AgingType"]?.ToString()?.Trim().ToUpper();
                            string from = reader["FromDays"]?.ToString()?.Trim().ToUpper();
                            string to = reader["ToDays"]?.ToString()?.Trim().ToUpper();

                            if (type == "C")
                            {
                                aging.CustomerAgingFrom.Add(from);
                                aging.CustomerAgingTo.Add(to);
                            }
                            else if(type == "P")
                            {
                                aging.VendorAgingFrom.Add(from);
                                aging.VendorAgingTo.Add(to);
                            }
                        }

                        settings.Aging.Add(aging);
                    }
                }
            }

            return result;
        }

    }
}

using DocumentFormat.OpenXml.Wordprocessing;
using fintech;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using static fintech.Models.CommonModel;

namespace fintech.DAL
{
    public class BankingDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;

        public bool SaveBankAccount(BankAccountCreate model, int? entityId,int? loginId ,out string messageCode)
        {
            bool isSuccess = false;
            messageCode = "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Acct.sp_SaveBankAccount", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Entity_ID", entityId);
                cmd.Parameters.AddWithValue("@Login_ID", loginId);

                cmd.Parameters.AddWithValue("@AccountType ", model.AccountType);
                cmd.Parameters.AddWithValue("@AccountName ", model.AccountName ?? "");
                cmd.Parameters.AddWithValue("@AccountNumber", model.AccountNumber ?? "");
                cmd.Parameters.AddWithValue("@BankName", model.BankName?? "");
                cmd.Parameters.AddWithValue("@IFSCCode", model.IFSCCode ?? "");
                cmd.Parameters.AddWithValue("@SwiftCode", model.SwiftCode);
                cmd.Parameters.AddWithValue("@SortCode", model.SortCode);

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

    }
}

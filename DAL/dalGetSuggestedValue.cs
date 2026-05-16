using fintech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static fintech.Models.CommonModel;

namespace fintech.DAL
{
    public class dalGetSuggestedValue:FintechDbContext
    {
        public List<AutoCompleteModel> GetSuggestedValue(AutoCompleteRequestModel request)
        {
            try
            {
                    OpenStoredPorcedure("COMMON.Sp_GetSuggestValue");
                    AddInParameter("@Table", request.Table);
                    AddInParameter("@Prefix", request.Prefix);
                    AddInParameter("@Text", request.Text);
                    AddInParameter("@Entity_ID", request.Entity_ID);
                    AddInParameter("@WhereCond", request.WhereCond);
                    return ExecuteDataSet().Tables[0].ConvertDataTableToList<AutoCompleteModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<itemList> GetItemDetails(AutoCompleteRequestModel request)
        //{
        //    var item = new List<itemList>();
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        using (var cmd = new SqlCommand("COMMON.Sp_GetSuggestValue", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Table", request.Table);
        //            cmd.Parameters.AddWithValue("@Prefix", request.Prefix);
        //            cmd.Parameters.AddWithValue("@Text", request.Text);
        //            cmd.Parameters.AddWithValue("@Entity_ID", request.Entity_ID)
        //            cmd.Parameters.AddWithValue("@WhereCond", request.WhereCond)
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    item.Product_Id = Convert.ToInt32(reader["Product_Id"]);
        //                    item.Type = Convert.ToInt32(reader["Type"]);
        //                    item.Product_Name = reader["Product_Name"]?.ToString();
        //                    item.Product_Desc = reader["Product_Desc"]?.ToString();
        //                    item.Product_HSNCode = reader["Product_HSNCode"]?.ToString();
        //                    item.Selling_Price = Convert.ToDecimal(reader["Selling_Price"]);
        //                    item.Cost_Price = Convert.ToDecimal(reader["Cost_Price"]);
        //                }
        //            }
        //        }
        //    }
        //    return item;
        //}
        public List<AutoMulitSelectModel> GetMulitSelectSuggestedValue(AutoCompleteRequestModel request)
        {
            try
            {
                OpenStoredPorcedure("COMMON.Sp_GetMultiSelectSuggestValue");
                AddInParameter("@Table", request.Table);
                AddInParameter("@Prefix", request.Prefix);
                AddInParameter("@Text", request.Text);
                AddInParameter("@Entity_ID", request.Entity_ID);
                AddInParameter("@WhereCond", request.WhereCond);
                AddInParameter("@PageSize", request.PageSize);
                AddInParameter("@PageNum", request.PageNum);
                return ExecuteDataSet().Tables[0].ConvertDataTableToList<AutoMulitSelectModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AutoCompleteWithChildModel> GetSuggestedValuewithChild(AutoCompleteRequestModel request)
        {
            try
            {
                OpenStoredPorcedure("COMMON.Sp_GetSuggestValue");
                AddInParameter("@Table", request.Table);
                AddInParameter("@Prefix", request.Prefix);
                AddInParameter("@Text", request.Text);
                AddInParameter("@Entity_ID", request.Entity_ID);
                AddInParameter("@WhereCond", request.WhereCond);
                return ExecuteDataSet().Tables[0].ConvertDataTableToList<AutoCompleteWithChildModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<AutoCompleteFlexModel> GetSuggestedFlexValue(AutoCompleteRequestModel request)
        {
            try
            {
                OpenStoredPorcedure("[COMMON].[Sp_GetSuggestFlexValue]");
                AddInParameter("@Module", request.Module);
                AddInParameter("@Table", request.Table);
                AddInParameter("@Text", request.Text);
                AddInParameter("@Entity_ID", request.Entity_ID);
                //AddInParameter("@WhereCond", request.WhereCond);
                return ExecuteDataSet().Tables[0]
                    .AsEnumerable()
                    .Select(s => new AutoCompleteFlexModel()
                    {
                        ID = s.Field<string>("ID"),
                        Text = s.Field<string>("Name")
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
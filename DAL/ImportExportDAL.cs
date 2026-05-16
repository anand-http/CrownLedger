// ============================================================
// ImportExportDAL.cs
// ============================================================

using fintech.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace YourApp.DAL
{
    public class ImportExportDAL
    {
        private readonly string _connStr =
            ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;

        // ════════════════════════════════════════════════════
        // EXPORT — fetch all rows for a module
        // ════════════════════════════════════════════════════
        public List<Dictionary<string, object>> GetExportData(string moduleType, int entityId)
        {
            var result = new List<Dictionary<string, object>>();
            // Single SP handles all 4 modules
            string spName = "usp_ImportExport_GetData";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(spName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModuleType", moduleType);
                cmd.Parameters.AddWithValue("@EntityId", entityId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        result.Add(row);
                    }
                }
            }
            return result;
        }

        // ════════════════════════════════════════════════════
        // IMPORT — save one row at a time
        // Called per row; errors are caught in BAL and skipped
        // ════════════════════════════════════════════════════
        //public void ImportRow(string moduleType, Dictionary<string, string> row,
        //    int entityId, int createdBy)
        //{
        //    string spName = "usp_ImportExport_ImportRow";

        //    using (var conn = new SqlConnection(_connStr))
        //    using (var cmd  = new SqlCommand(spName, conn))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ModuleType", moduleType);
        //        cmd.Parameters.AddWithValue("@EntityId",   entityId);
        //        cmd.Parameters.AddWithValue("@CreatedBy",  createdBy);

        //        // Pass entire row as XML so SP can handle all modules
        //        cmd.Parameters.AddWithValue("@RowXml", _BuildRowXml(row));

        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public bool ImportAll(string moduleType, string xmlData)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("Sales.sp_ImportDocument", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@XmlData", SqlDbType.Xml)
                    {
                        Value = xmlData
                    });
                    cmd.Parameters.AddWithValue("@ModuleType", moduleType);

                    var messageParam = new SqlParameter("@Message_code", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(messageParam);

                    cmd.ExecuteNonQuery();

                    var messageCode = messageParam.Value?.ToString();

                    var isSuccess = messageCode == "I0001" || messageCode == "U0001";

                    return isSuccess;

                }
            }
        }

        private DataTable _BuildImportItemsTable(List<ImportLineItem> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("ItemDetails", typeof(string));
            dt.Columns.Add("ItemDescription", typeof(string));
            dt.Columns.Add("HSNCode", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));
            dt.Columns.Add("Rate", typeof(string));
            dt.Columns.Add("TaxType", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            dt.Columns.Add("Discount", typeof(string));
            dt.Columns.Add("OtherAdjustment", typeof(string));
            dt.Columns.Add("Tax", typeof(string));

            foreach (var item in items)
            {
                dt.Rows.Add(
                    item.ItemDetails,
                    item.ItemDescription,
                    item.HSNCode,
                    item.Quantity,
                    item.Rate,
                    item.TaxType,
                    item.Amount,
                    item.Discount,
                    item.OtherAdjustment,
                    item.Tax
                );
            }
            return dt;
        }

        // ── Build XML from row dictionary ─────────────────────
        // Produces: <Row><Field name="CustomerName">ABC</Field>...</Row>
        private string _BuildRowXml(Dictionary<string, string> row)
        {
            var root = new XElement("Row");
            foreach (var kv in row)
            {
                root.Add(new XElement("Field",
                    new XAttribute("name", kv.Key),
                    kv.Value ?? ""));
            }
            return root.ToString();
        }
    }
}

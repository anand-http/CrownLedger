// ============================================================
// GridActionDAL.cs — Data Access Layer
// Uses XML parameter to pass N ids to SQL (same pattern as
// your existing TVP / XML SP approach in this project).
// ============================================================

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace fintech.DAL
{
    public class GridActionDAL
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;

        // ── Build XML from int[] ids ──────────────────────────
        // Produces: <Ids><Id>1</Id><Id>2</Id></Ids>
        private string _BuildIdsXml(int[] ids)
        {
            var root = new XElement("Ids");
            foreach (var id in ids)
                root.Add(new XElement("Id", id));
            return root.ToString();
        }

        // ── Get rows by IDs (for PDF generation) ─────────────
        // ── GridActionDAL.cs ──────────────────────────────────────
        // CHANGED: 3 methods replaced with 1 universal method

        // Only method needed in GridActionDAL — handles GET, COPY, DELETE
        public (List<Dictionary<string, object>> Rows, int AffectedCount)ExecuteGridAction(string moduleType, int[] ids, string action = "GET")
        {
            var result = new List<Dictionary<string, object>>();
            int affectedCount = 0;

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("COMMON.usp_GridAction_GetByIds", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModuleType", moduleType);
                cmd.Parameters.AddWithValue("@IdsXml", _BuildIdsXml(ids));
                cmd.Parameters.AddWithValue("@Action", action);

                var outParam = new SqlParameter("@AffectedCount", SqlDbType.Int)
                { Direction = ParameterDirection.Output, Value = 0 };
                cmd.Parameters.Add(outParam);

                conn.Open();

                if (action == "GET")
                {
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
                else
                {
                    cmd.ExecuteNonQuery();
                    affectedCount = Convert.ToInt32(outParam.Value);
                }
            }

            return (result, affectedCount);
        }
    }
}

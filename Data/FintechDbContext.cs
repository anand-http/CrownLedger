using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

namespace fintech.Data
{
    public class FintechDbContext : DbContext
    {

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        #region Private Variables

        //private string Connectionstring = Get_Configvalue(HttpContext.Current.Session["ECL"] as string, "DB");
        private string Connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["FintechEntities1"].ConnectionString;

        private DataSet _db;
        private SqlCommand Command;
        private SqlConnection con;
        private SqlDataAdapter DataAdapter;
        private SqlTransaction trans = null;
        #endregion

        #region Private Methods

        private void BeginTransaction()
        {
            trans = con.BeginTransaction();
        }

        private void CommitTransaction()
        {
            trans.Commit();
            con.Close();
        }

        private void RollbackTransaction()
        {
            trans.Commit();
            con.Close();
        }
        #endregion

        #region Protected Methods
        protected void OpenStoredPorcedure(string StoredProcedureName)
        {
            con = new SqlConnection(Connectionstring);
            Command = new SqlCommand();

            Command.CommandText = StoredProcedureName;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = 120;
            Command.Connection = con;
            con.Open();
        }

        protected void OpenStoredPorcedurewithcon(string StoredProcedureName, string Mode, string Entitykey)
        {
            string Connectionstring = "";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Models\\xmlFiles\\xmlConfig.xml");
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Categories/Category");
            foreach (XmlNode node in nodeList)
            {
                if (node.SelectSingleNode("Entity").InnerText == Entitykey)
                {
                    if (Mode.ToUpper() == "DB")
                    {
                        Connectionstring = node.SelectSingleNode("FintechEntities1").InnerText;
                    }

                    else
                    {
                        Connectionstring = "";
                    }
                }
            }

            con = new SqlConnection(Connectionstring);
            Command = new SqlCommand();

            Command.CommandText = StoredProcedureName;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = 120;
            Command.Connection = con;
            con.Open();
        }
        protected void AddInParameter(String ParameterName, object Values)
        {
            Command.Parameters.AddWithValue(ParameterName, Values);
        }

        protected void AddOutParameter(String ParameterName, SqlDbType Datatype)
        {
            if (Datatype == SqlDbType.NVarChar)
            {
                Command.Parameters.Add(ParameterName, Datatype, 4000).Direction = ParameterDirection.Output;
            }
            else if (Datatype == SqlDbType.VarBinary)
            {
                Command.Parameters.Add(ParameterName, Datatype, 8000).Direction = ParameterDirection.Output;
            }
            else
            {
                Command.Parameters.Add(ParameterName, Datatype).Direction = ParameterDirection.Output;
            }
        }
        protected void AddOutParameterWithValue(String ParameterName, SqlDbType Datatype, dynamic defaultvalue)
        {
            if (Datatype == SqlDbType.NVarChar)
            {
                Command.Parameters.Add(ParameterName, Datatype, 4000).Direction = ParameterDirection.InputOutput;
            }
            else
            {
                Command.Parameters.Add(ParameterName, Datatype).Direction = ParameterDirection.InputOutput;
            }
            Command.Parameters[ParameterName].Value = defaultvalue;
        }

        protected string GetParameterValue(string ParameterName)
        {
            return Command.Parameters[ParameterName].Value.ToString();
        }
        protected byte[] GetParameterbyteValue(string ParameterName)
        {
            return (byte[])Command.Parameters[ParameterName].Value;
        }
        protected bool ExecuteNonQuery()
        {
            Command.ExecuteNonQuery();
            con.Close();
            return true;
        }

        protected DataSet ExecuteDataSet()
        {
            DataAdapter = new SqlDataAdapter(Command);
            _db = new DataSet();
            DataAdapter.Fill(_db);
            con.Close();
            return _db;
        }

        protected SqlDataReader ExecuteDataReader()
        {
            SqlDataReader dataReader;
            dataReader = Command.ExecuteReader(CommandBehavior.CloseConnection);
            return dataReader;
        }
        #endregion



        ~FintechDbContext()
        {
            if (!this.disposedValue)
                this.Dispose();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected new virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _db = null;
                    if (con != null)
                    {
                        con.Dispose();
                        con = null;
                    }
                    if (DataAdapter != null)
                    {
                        DataAdapter.Dispose();
                        DataAdapter = null;
                    }
                    if (trans != null)
                    {
                        trans.Dispose();
                        trans = null;
                    }
                    if (Command != null)
                    {
                        Command.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DBTransaction() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public new void Dispose()
        {
            Dispose(true);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
        #endregion

        public static string Get_Configvalue(string Entitykey, string Mode)
        {
            if (String.IsNullOrWhiteSpace(Entitykey))
            {
                return "";
            }

            try
            {
                switch (Mode)
                {
                    case "DB":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["DbConnectionString_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["DbConnectionString_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "REPORTFOOTER":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["ReportFooter_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["ReportFooter_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "REPORTDB":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["ReportDb_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["ReportDb_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "SMTPHOST":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["SMTPHOST_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["SMTPHOST_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "PORT":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["PORT_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["PORT_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "SSL":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["SSL_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["SSL_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "SMTPUSER":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["SMTPUSER_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["SMTPUSER_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "SMTPPWD":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["SMTPPWD_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["SMTPPWD_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "MAILTYPE":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["MAILTYPE_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["MAILTYPE_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "SMTPTYPE":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["SMTPTYPE_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["SMTPTYPE_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "SERVICEURL":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["SERVICEURL_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["SERVICEURL_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "QuoteDocumentId":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["QuoteDocumentId_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["QuoteDocumentId_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "ReservationDocumentId":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["ReservationDocumentId_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["ReservationDocumentId_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "DecimalDigits":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["DecimalDigits_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["DecimalDigits_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "QuoteUrl":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["QuoteUrl_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["QuoteUrl_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "AllowDiscount":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["AllowDiscount_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["AllowDiscount_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "GSTVisible":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["GSTVisible_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["GSTVisible_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "PANVisible":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["PANVisible_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["PANVisible_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "MobileNoRestriction":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["MobileNoRestriction_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["MobileNoRestriction_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    case "ShowHold":
                        if (!String.IsNullOrWhiteSpace(HttpContext.Current.Session["ShowHold_" + HttpContext.Current.Session["ECL"] as string] as string))
                        {
                            return HttpContext.Current.Session["ShowHold_" + HttpContext.Current.Session["ECL"] as string] as string;
                        }
                        break;
                    default:
                        break;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Models\\xmlFiles\\xmlConfig.xml");
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Categories/Category");
                foreach (XmlNode node in nodeList)
                {
                    if (node.SelectSingleNode("Entity").InnerText == Entitykey)
                    {
                        if (Mode.ToUpper() == "DB")
                        {
                            HttpContext.Current.Session["DbConnectionString_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("DbConnectionString").InnerText;
                            return node.SelectSingleNode("DbConnectionString").InnerText;
                        }
                        else if (Mode.ToUpper() == "REPORTFOOTER")
                        {
                            HttpContext.Current.Session["ReportFooter_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("ReportFooter").InnerText;
                            return node.SelectSingleNode("ReportFooter").InnerText;
                        }
                        else if (Mode.ToUpper() == "REPORTDB")
                        {
                            HttpContext.Current.Session["ReportDb_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("ReportConnectionString").InnerText;
                            return node.SelectSingleNode("ReportConnectionString").InnerText;
                        }
                        else if (Mode.ToUpper() == "REPORTDB")
                        {
                            HttpContext.Current.Session["ReportDb_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("ReportConnectionString").InnerText;
                            return node.SelectSingleNode("ReportConnectionString").InnerText;
                        }
                        else if (Mode.ToUpper() == "SMTPHOST")
                        {
                            HttpContext.Current.Session["SMTPHOST_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("SMTPHOST").InnerText;
                            return node.SelectSingleNode("SMTPHOST").InnerText;
                        }
                        else if (Mode.ToUpper() == "PORT")
                        {
                            HttpContext.Current.Session["PORT_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("PORT").InnerText;
                            return node.SelectSingleNode("PORT").InnerText;
                        }
                        else if (Mode.ToUpper() == "SSL")
                        {
                            HttpContext.Current.Session["SSL_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("SSL").InnerText;
                            return node.SelectSingleNode("SSL").InnerText;
                        }
                        else if (Mode.ToUpper() == "SMTPUSER")
                        {
                            HttpContext.Current.Session["SMTPUSER_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("SMTPUSER").InnerText;
                            return node.SelectSingleNode("SMTPUSER").InnerText;
                        }
                        else if (Mode.ToUpper() == "MAILTYPE")
                        {
                            HttpContext.Current.Session["MAILTYPE_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("MAILTYPE").InnerText;
                            return node.SelectSingleNode("MAILTYPE").InnerText;
                        }
                        else if (Mode.ToUpper() == "SMTPPWD")
                        {
                            HttpContext.Current.Session["SMTPPWD_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("SMTPPWD").InnerText;
                            return node.SelectSingleNode("SMTPPWD").InnerText;
                        }
                        else if (Mode.ToUpper() == "SMTPTYPE")
                        {
                            HttpContext.Current.Session["SMTPTYPE_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("SMTPTYPE").InnerText;
                            return node.SelectSingleNode("SMTPTYPE").InnerText;
                        }
                        else if (Mode.ToUpper() == "SERVICEURL")
                        {
                            HttpContext.Current.Session["SERVICEURL_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("SERVICEURL").InnerText;
                            return node.SelectSingleNode("SERVICEURL").InnerText;
                        }
                        else if (Mode.ToUpper() == "QUOTEDOCUMENTID")
                        {
                            HttpContext.Current.Session["QuoteDocumentId_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("QuoteDocumentId").InnerText;
                            return node.SelectSingleNode("QuoteDocumentId").InnerText;
                        }
                        else if (Mode.ToUpper() == "RESERVATIONDOCUMENTID")
                        {
                            HttpContext.Current.Session["ReservationDocumentId_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("ReservationDocumentId").InnerText;
                            return node.SelectSingleNode("ReservationDocumentId").InnerText;
                        }
                        else if (Mode.ToUpper() == "DECIMALDIGITS")
                        {
                            HttpContext.Current.Session["DecimalDigits_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("DecimalDigits").InnerText;
                            return node.SelectSingleNode("DecimalDigits").InnerText;
                        }
                        else if (Mode.ToUpper() == "QUOTEURL")
                        {
                            HttpContext.Current.Session["QuoteUrl_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("QuoteUrl").InnerText;
                            return node.SelectSingleNode("QuoteUrl").InnerText;
                        }
                        else if (Mode.ToUpper() == "ALLOWDISCOUNT")
                        {
                            HttpContext.Current.Session["AllowDiscount_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("AllowDiscount").InnerText;
                            return node.SelectSingleNode("AllowDiscount").InnerText;
                        }
                        else if (Mode.ToUpper() == "GSTVISIBLE")
                        {
                            HttpContext.Current.Session["GSTVisible_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("GSTVisible").InnerText;
                            return node.SelectSingleNode("GSTVisible").InnerText;
                        }
                        else if (Mode.ToUpper() == "PANVISIBLE")
                        {
                            HttpContext.Current.Session["PANVisible_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("PANVisible").InnerText;
                            return node.SelectSingleNode("PANVisible").InnerText;
                        }
                        else if (Mode.ToUpper() == "MOBILENORESTRICTION")
                        {
                            HttpContext.Current.Session["MobileNoRestriction_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("MobileNoRestriction").InnerText;
                            return node.SelectSingleNode("MobileNoRestriction").InnerText;
                        }
                        else if (Mode.ToUpper() == "SHOWHOLD")
                        {
                            HttpContext.Current.Session["ShowHold_" + HttpContext.Current.Session["ECL"] as string] = node.SelectSingleNode("ShowHold").InnerText;
                            return node.SelectSingleNode("ShowHold").InnerText;
                        }

                        else
                        {
                            return "";
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
using fintech.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static fintech.Models.PasswordComplexity;

namespace fintech.DAL
{
    public class dalPasswordComplexity:FintechDbContext
    {
        ~dalPasswordComplexity()
        {
            base.Dispose();
        }
        public IEnumerable<PasswordComplexityModel> GetPasswordComplexitys(PasswordComplexityModel _model)
        {
            try
            {
                OpenStoredPorcedure("[SERVICE].[Sp_GetPasswordComplexity]");
                AddInParameter("@Entity_ID", _model.Entity_ID);
                AddInParameter("@PassComp_ID", _model.PassComp_ID);
                AddInParameter("@start", _model.start);
                AddInParameter("@length", _model.length);
                AddInParameter("@orderby", _model.orderby);
                AddInParameter("@search", _model.search);
                return ExecuteDataSet().Tables[0].ConvertDataTableToList<PasswordComplexityModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Save_PasswordComplexity(PasswordComplexityModel _model, bool restore, string passCompXml)
        {
            try
            {
                OpenStoredPorcedure("[CORE].[Sp_InsUpd_PasswordComplexity]");
                AddInParameter("@PassComp_ID", _model.PassComp_ID);
                AddInParameter("@PassComp_Xml", passCompXml);
                AddInParameter("@Active", _model.Active);
                AddInParameter("@Restore", restore);
                AddInParameter("@Entity_ID", _model.Entity_ID);
                AddInParameter("@Login_ID", _model.LoginID);
                AddOutParameter("@Message_code", SqlDbType.NVarChar);
                ExecuteDataSet();
                return GetParameterValue("@Message_code");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PasswordComplexityModel> GetCurrentPasswordComplexity(int entityID, string module, long LoginId)
        {
            try
            {
                OpenStoredPorcedure("[Core].[Sp_GetCurrentPasswordComplexity]");
                AddInParameter("@Entity_ID", entityID);
                AddInParameter("@Module", module);
                AddInParameter("@Login_ID", LoginId);
                return ExecuteDataSet().Tables[0].ConvertDataTableToList<PasswordComplexityModel>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
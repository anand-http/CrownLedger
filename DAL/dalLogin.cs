using fintech.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using static fintech.Models.Login;
using static fintech.Models.LicenseModel;

namespace fintech.DAL
{
    public class dalLogin : FintechDbContext
    {
        public Tuple<LoginModel, List<LoginEntityModel>> LoginCredentialCheck(string Uname, ref string Pwd, ref string EntityCode, out string StatusCode, string IpAddress)
        {
            try
            {
                OpenStoredPorcedure("UMS.SP_CheckLoginCredential");
                AddInParameter("@UserName", Uname);
                AddInParameter("@Password", Pwd);
                AddInParameter("@EntityCode", EntityCode);
                AddOutParameterWithValue("@StatusCode", SqlDbType.NVarChar, "");
                AddInParameter("@IpAddress", IpAddress);

                DataSet ds = ExecuteDataSet();
                StatusCode = GetParameterValue("@StatusCode").ToString();
                LoginModel login;
                if (ds.Tables.Count > 0)
                {
                    login = ds.Tables[0]
                        .AsEnumerable()
                        .Select(s => new LoginModel()
                        {
                            Prompt2FARegister = s.Field<bool>("Prompt2FARegister"),
                            Prompt2FACode = s.Field<bool>("Prompt2FACode"),
                            Skip2FACode = s.Field<DateTime>("Skip2FACode"),
                            LoginID = s.Field<Int32>("LoginID"),
                            FullName = s.Field<string>("FullName"),
                            UserLogin = Uname,
                            UserImage = s.Field<string>("User_Image"),
                            UserEmail = s.Field<string>("User_Email"),
                            User_MailPassword = s.Field<string>("User_MailPassword"),
                            IsResetPassword = s.Table.Columns.Contains("User_ResetPassword") ? s.Field<bool>("User_ResetPassword") : false,
                            IsFirstLogin = s.Table.Columns.Contains("User_FirstLogin") ? s.Field<bool>("User_FirstLogin") : false
                        }).FirstOrDefault();
                }
                else
                {
                    login = new LoginModel();
                }
                List<LoginEntityModel> Entitys;
                if (ds.Tables.Count > 1)
                {
                    Entitys = ds.Tables[1]
                        .AsEnumerable()
                        .Select(s => new LoginEntityModel()
                        {
                            EntityId = s.Field<Int32>("Entity_ID"),
                            EntityName = s.Field<string>("EntityName"),
                            UserEntity_Default = s.Field<bool>("UserEG_Default"),
                            DateDisplayFormat = s.Field<string>("Date_Display"),
                            DateEntryFormat = s.Field<string>("Date_Entry"),
                            DateServerFormat = s.Field<string>("Date_Server"),
                            Dec_Symbol = s.Field<string>("Decimal_Symbol"),
                            Thousand_Sep = s.Field<string>("Thousand_Seperator"),
                            Max_User = s.Field<int>("Max_User"),
                            Entity_Ver = s.Field<string>("Entity_Ver"),
                            NewResourceAssignment_Page = ds.Tables[1].Columns.Contains("NewResourceAssignment_Page") ? s.Field<string>("NewResourceAssignment_Page") : "",
                        }).ToList();
                }
                else
                {
                    Entitys = new List<LoginEntityModel>();
                }

                if (Entitys.Count() > 0)
                {
                    switch (Entitys[0].Entity_Ver.ToUpper())
                    {
                        case "L":
                            Entitys[0].Entity_Version = "Lite";
                            break;
                        case "P":
                            Entitys[0].Entity_Version = "Professional";
                            break;
                        case "E":
                            Entitys[0].Entity_Version = "Enterprise";
                            break;
                    }
                    return new Tuple<LoginModel, List<LoginEntityModel>>(login, Entitys);
                }
                else
                {
                    return new Tuple<LoginModel, List<LoginEntityModel>>(login, Entitys);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ClearSession(string SessionID)
        {
            try
            {
                OpenStoredPorcedure("[UMS].[Sp_mst_ClearSesssion]");
                AddInParameter("@SessionID", SessionID);
                ExecuteNonQuery();
                return "U0001";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IEnumerable<UserAccessModel> getPageAccess(int menuID, int userID, int entityID)
        {
            try
            {
                OpenStoredPorcedure("UMS.SP_GetPageRestrictrions");
                AddInParameter("@Entity_ID", entityID);
                AddInParameter("@Menu_ID", menuID);
                AddInParameter("@Login_ID", userID);
                var ds = ExecuteDataSet();
                return ds.Tables[0]
                    .AsEnumerable()
                    .Select(s => new UserAccessModel()
                    {
                        Add = s.Field<bool>("PRestr_Add"),
                        Mod = s.Field<bool>("PRestr_Mod"),
                        Del = s.Field<bool>("PRestr_Del"),
                        Next_width = s.Field<int>("Next_width"),
                        Add_width = s.Field<int>("Add_width"),
                        Active = s.Field<bool>("Active"),
                        ButtonList = ds.Tables[1].ConvertDataTableToList<buttonList>()
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ChangePassword(int LoginID, string NewPassword, int EntityID)
        {
            try
            {
                OpenStoredPorcedure("[UMS].[Sp_mst_InsUpdUserPassword]");
                AddInParameter("@User_ID", LoginID);
                AddInParameter("@User_Password", NewPassword);
                AddInParameter("@Entity_ID", EntityID);
                AddOutParameter("@Message_Code", SqlDbType.NVarChar);
                ExecuteDataSet();
                string msgout = GetParameterValue("@Message_Code");
                return msgout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string EntityLicenceValidity(string EntityCode)
        {
            try
            {
                string LicenceKey = ConfigurationManager.AppSettings["StriggleLicenceKey"];
                OpenStoredPorcedure("" + LicenceKey + ".[UMS].[SP_GetEntityLicenceValidity]");
                AddInParameter("@Entity_Code", EntityCode);
                AddOutParameter("@MessageCode", SqlDbType.NVarChar);
                ExecuteDataSet();
                string msgout = GetParameterValue("@MessageCode");
                return msgout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public LicenceModel EntityVerifyEntityEmail(string EntityCode, string Email, out string EmailOTP, out string msgcode, out int EntityId)
        {
            try
            {
                string LicenceKey = ConfigurationManager.AppSettings["StriggleLicenceKey"];
                OpenStoredPorcedure("" + LicenceKey + ".[UMS].[SP_VerifyEntityEmail]");
                AddInParameter("@Entity_Code", EntityCode);
                AddInParameter("@Email", Email);
                AddOutParameter("@MessageCode", SqlDbType.NVarChar);
                AddOutParameter("@EmailOTP", SqlDbType.NVarChar);
                AddOutParameter("@EntityId", SqlDbType.NVarChar);
                DataSet ds = new DataSet();
                ds = ExecuteDataSet();
                EmailOTP = GetParameterValue("@EmailOTP");
                string msgout = GetParameterValue("@MessageCode");
                msgcode = GetParameterValue("@MessageCode");

                if (msgout == "S0001")
                {
                    EntityId = Convert.ToInt32(GetParameterValue("@EntityId"));
                    return new LicenceModel()
                    {
                        Headers = DataTableHelper.GetItem<fintech.Models.LicenseModel.Header>(ds.Tables[0].Rows[0]),
                        Entity_LicValids = ds.Tables[1].ConvertDataTableToList<Entity_LicValid>(),
                        Entity_LicPaymentHistorys = ds.Tables[2].ConvertDataTableToList<Entity_LicPaymentHistory>(),
                    };
                }
                else
                {
                    EntityId = 0;
                    return new LicenceModel();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fintech.Models
{
    public class Login
    {
        [Serializable]
        public class LoginModel
        {
            public Int32 LoginID { get; set; }
            public string UserLogin { get; set; }
            public string FullName { get; set; }
            public string UserEmail { get; set; }
            public string UserImage { get; set; }
            public string User_MailPassword { get; set; }
            private string _DateDisplayFormat;

            public string SessionKey { get; set; }
            public bool LoggedIn { get; set; }
            public bool IsResetPassword { get; set; }
            public bool IsFirstLogin { get; set; }
            public string DateDisplayFormat { get { return string.IsNullOrEmpty(_DateDisplayFormat) ? "dd-MMM-yyyy" : _DateDisplayFormat; } set { _DateDisplayFormat = value; } }

            private string _DateEntryFormat;
            public string DateEntryFormat { get { return string.IsNullOrEmpty(_DateEntryFormat) ? "dd-MMM-yyyy" : _DateEntryFormat; } set { _DateEntryFormat = value; } }
            public bool Prompt2FARegister { get; set; }
            public bool Prompt2FACode { get; set; }
            public DateTime Skip2FACode { get; set; }
        }
        [Serializable]
        public class LoginEntityModel
        {
            public int EntityId { get; set; }
            public string EntityName { get; set; }
            public bool UserEntity_Default { get; set; }
            public string DateDisplayFormat { get; set; }
            public string DateEntryFormat { get; set; }
            public string DateServerFormat { get; set; }
            public string Dec_Symbol { get; set; }
            public string Thousand_Sep { get; set; }
            public int Max_User { get; set; }
            public string Entity_Ver { get; set; }
            public string Entity_Version { get; set; }
            public string NewResourceAssignment_Page { get; set; }

        }
        public class LoginRquest
        {
            public LoginCredential Credential { get; set; }
            public string ProductName { get; set; }
            public string Code { get; set; }

        }
        public class LoginCredential
        {
            public string Type { get; set; }
            public string Module { get; set; }
            public string Domain { get; set; }
            public string LoginID { get; set; }
            public string Password { get; set; }
            public string LanguageLocale { get; set; }
            public string IpAddress { get; set; }



        }

        [Serializable]
        [JsonObject]
        public class Credential
        {
            public string Domain { get; set; }
            public string LoginID { get; set; }
            public string Password { get; set; }
            public string Type = "C";
            public string Module = "B";
            public string LanguageLocale { get; set; }
            public string IpAddress { get; set; }
        }
        public class WSResponseModel
        {
            public string Skip2FADate { get; set; }
            public string QrImage { get; set; }
            public string SrvStatus { get; set; }
            public string SrvStatusCode { get; set; }
            public string SrvStatusMessage { get; set; }

        }
        public class Request2FAResponse
        {
            public string QrImage { get; set; }
            public string StatusMessage { get; set; }
        }
        public class Validate2FAResponse
        {
            public bool IsValid { get; set; }
            public string StatusCode { get; set; }
            public string StatusMessage { get; set; }
            public string StatusType { get; set; }
        }
        [Serializable]
        public class UserAccessModel
        {
            public bool Add { get; set; }
            public bool Mod { get; set; }
            public bool Del { get; set; }
            public int Next_width { get; set; }
            public int Add_width { get; set; }
            public bool Active { get; set; }
            public List<buttonList> ButtonList { get; set; }
        }
        public class buttonList
        {
            public bool PRestr_Add { get; set; }
            public bool PRestr_Mod { get; set; }
            public bool PRestr_Del { get; set; }
            public int Menu_ID { get; set; }
            public string Menu_BtnName { get; set; }
        }
    }
}
using  fintech.BAL;
using fintech.DAL;
using fintech.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static fintech.Models.Login;
using  fintech.App_Data;
using static fintech.Models.PasswordComplexity;

namespace fintech.Controllers
{
    public class LoginController : SessionObjects
    {
        // GET: Login

        public class FileLogger
        {
            public static void Log(string message)
            {
                string filePath = @"C:\Logs\Application.log";

                // Ensure directory exists
                   Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                // Write log
                System.IO.File.AppendAllText(
                    filePath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}"
                );
            }
        }

        public ActionResult LogOut()
        {
            if (System.Web.HttpContext.Current.Application["Sessions"] != null)
            {
                if (sessionLogin != null)
                {
                    ballogin _ballogin = new ballogin();
                    _ballogin.ClearSession(System.Web.HttpContext.Current.Session.SessionID);
                    ((Dictionary<string, string>)System.Web.HttpContext.Current.Application["Sessions"]).Remove(sessionLogin.LoginID.ToString());
                }
            }
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoginCredentialCheckOnly(LoginRquest objLoginRequest)
        {

            ballogin _ballogin = new ballogin();
            Session["ECL"] = null;
            Session["ECL"] = objLoginRequest.Credential.Domain.ToUpper();
            string outLicenceEntitymsg = "S0001";
            //outLicenceEntitymsg = _ballogin.EntityLicenceValidity(EntityCode);
            if (outLicenceEntitymsg == "S0001")
            {

                balEncrytion objEncrypt = new balEncrytion();

                string outmsg;
                var login = _ballogin.LoginCredentialCheck(objLoginRequest.Credential.LoginID, objEncrypt.Encrypt(objLoginRequest.Credential.Password), objLoginRequest.Credential.Domain, out outmsg, objLoginRequest.Credential.IpAddress);
                WSResponseModel objWSResponse = new WSResponseModel();
                Request2FAResponse obj2FAResponse = new Request2FAResponse();
                if (outmsg == "LS001")
                {
                    bool Prompt2FARegister = login.Item1.Prompt2FARegister;
                    DateTime currentdate = DateTime.Now;
                    DateTime Skip2FACode = login.Item1.Skip2FACode;
                    bool Prompt2FACode = login.Item1.Prompt2FACode;
                    //CheckUser(login.Item1.LoginID.ToString(), System.Web.HttpContext.Current.Session.SessionID);
                    Session["tempentityname"] = objLoginRequest.Credential.Domain;
                    Session["temploginid"] = objLoginRequest.Credential.LoginID;
                    Session["temppassword"] = objLoginRequest.Credential.Password;
                    WSHelper objWSHelper = new WSHelper();
                    string JsoRes2;
                    string checkstrReqCode = "", checkstrReqMessage = "";
                    string Encrypassword = objEncrypt.Encrypt(objLoginRequest.Credential.Password);
                    objLoginRequest.Credential.Password = Encrypassword;
                    string _objLoginRequestString = JsonConvert.SerializeObject(objLoginRequest, Newtonsoft.Json.Formatting.Indented);
                    JsoRes2 = objWSHelper.WebService2FACall(objLoginRequest.Credential.LoginID, Encrypassword, "Request_2FA", _objLoginRequestString, "POST", out checkstrReqCode, out checkstrReqMessage);
                    if (Prompt2FARegister)
                    {
                        if (Skip2FACode > currentdate)
                        {
                            obj2FAResponse = JsonConvert.DeserializeObject<Request2FAResponse>(JsoRes2);
                            objWSResponse.QrImage = obj2FAResponse.QrImage;
                            objWSResponse.SrvStatus = "openQRCode";
                            objWSResponse.Skip2FADate = Skip2FACode.AddDays(-1).ToShortDateString();
                        }
                        else
                        {
                            obj2FAResponse = JsonConvert.DeserializeObject<Request2FAResponse>(JsoRes2);
                            objWSResponse.QrImage = obj2FAResponse.QrImage;
                            objWSResponse.SrvStatus = "openWQRCode";
                            objWSResponse.Skip2FADate = Skip2FACode.AddDays(-1).ToShortDateString();
                        }
                    }
                    else if (Prompt2FACode)
                    {
                        if (Skip2FACode > currentdate)
                        {
                            obj2FAResponse = JsonConvert.DeserializeObject<Request2FAResponse>(JsoRes2);
                            objWSResponse.QrImage = obj2FAResponse.QrImage;
                            objWSResponse.SrvStatus = "opencodediv";
                            objWSResponse.Skip2FADate = Skip2FACode.AddDays(-1).ToShortDateString();
                        }
                        else
                        {
                            obj2FAResponse = JsonConvert.DeserializeObject<Request2FAResponse>(JsoRes2);
                            objWSResponse.QrImage = obj2FAResponse.QrImage;
                            objWSResponse.SrvStatus = "openWcodediv";
                            objWSResponse.Skip2FADate = Skip2FACode.AddDays(-1).ToShortDateString();
                        }
                    }
                    Session.Clear();
                    Session.Abandon();

                }
                else
                {
                    objWSResponse.SrvStatus = outmsg;
                }
                return Json(objWSResponse);
            }
            else
            {
                return Json(outLicenceEntitymsg);
            }
        }

        public JsonResult LoginCredentialwithQrCode(LoginRquest objLoginRequest)
        {
            ballogin _ballogin = new ballogin();
            Session["ECL"] = null;
            Session["ECL"] = objLoginRequest.Credential.Domain.ToUpper();
            string outLicenceEntitymsg = "S0001";
            //outLicenceEntitymsg = _ballogin.EntityLicenceValidity(EntityCode);
            if (outLicenceEntitymsg == "S0001")
            {

                balEncrytion objEncrypt = new balEncrytion();
                WSResponseModel objWSResponsecheck = new WSResponseModel();
                string outmsg;
                var login = _ballogin.LoginCredentialCheck(objLoginRequest.Credential.LoginID, objEncrypt.Encrypt(objLoginRequest.Credential.Password), objLoginRequest.Credential.Domain, out outmsg, objLoginRequest.Credential.IpAddress);
                if (outmsg == "LS001")
                {

                    CheckUser(login.Item1.LoginID.ToString(), System.Web.HttpContext.Current.Session.SessionID);

                    WSHelper objWSHelper = new WSHelper();
                    string JsoRes;
                    string checkstrReqCode = "", checkstrReqMessage = "", password = "";
                    Validate2FAResponse _Validate2FAResponse = new Validate2FAResponse();
                    password = objLoginRequest.Credential.Password;
                    string Encrypassword = objEncrypt.Encrypt(objLoginRequest.Credential.Password);
                    objLoginRequest.Credential.Password = Encrypassword;
                    string _objLoginRequestString = JsonConvert.SerializeObject(objLoginRequest, Newtonsoft.Json.Formatting.Indented);
                    JsoRes = objWSHelper.WebService2FACall(objLoginRequest.Credential.LoginID, Encrypassword, "Validate_2FA", _objLoginRequestString, "POST", out checkstrReqCode, out checkstrReqMessage);
                    _Validate2FAResponse = JsonConvert.DeserializeObject<Validate2FAResponse>(JsoRes);
                    //_Validate2FAResponse.IsValid = true;
                    if (!_Validate2FAResponse.IsValid)
                    {
                        Session.Clear();
                        Session.Abandon();
                        objWSResponsecheck.SrvStatus = _Validate2FAResponse.StatusType; ;
                        objWSResponsecheck.SrvStatusCode = _Validate2FAResponse.StatusCode;
                        objWSResponsecheck.SrvStatusMessage = _Validate2FAResponse.StatusMessage;
                        return Json(objWSResponsecheck);
                    }
                    else
                    {
                        objWSResponsecheck.SrvStatus = _Validate2FAResponse.StatusType; ;
                        sessionLogin = login.Item1;
                        Session["ResetPassword"] = login.Item1.IsResetPassword;
                        if (login.Item2.Count == 1)
                        {
                            sessionEntity = login.Item2.First();
                            outmsg = "Dashboard";
                            TempData["Entity1"] = login.Item2;
                            outmsg = "Dashboard";
                            Session["oldPassword"] = password;
                            Session["New_Resource_Assignment_Page"] = sessionEntity.NewResourceAssignment_Page;
                        }
                        string FilePath = System.Configuration.ConfigurationManager.AppSettings["SignatureUpload"].ToString();
                        string EntityPath = "\\" + sessionEntity.EntityId.ToString();
                        string folderpath = "\\User\\UserImages_" + login.Item1.LoginID + "\\";
                        if (System.IO.Directory.Exists(FilePath + EntityPath + folderpath))
                        {
                            var inputdirectory = new DirectoryInfo(FilePath + EntityPath + folderpath);
                            if (inputdirectory.GetFiles().Count() > 0)
                            {
                                Session["UserImage"] = inputdirectory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                            }
                            else
                            {
                                Session["UserImage"] = "";
                            }
                        }
                        else
                        {
                            Session["UserImage"] = "";
                        }
                        return Json(outmsg);
                    }

                }
                else
                {
                    return Json(outLicenceEntitymsg);
                }
            }
            else
            {
                return Json(outLicenceEntitymsg);
            }
        }
        [HttpPost]
        public JsonResult SkipAndLogin(string UserName, string Password, string EntityCode, string IpAddress)
        {
            FileLogger.Log("Application started");
            ballogin _ballogin = new ballogin();
            Session["ECL"] = null;
            Session["ECL"] = EntityCode.ToUpper();
            string outLicenceEntitymsg = "S0001";
            //outLicenceEntitymsg = _ballogin.EntityLicenceValidity(EntityCode);
            try
            {
                if (outLicenceEntitymsg == "S0001")
                {
                    FileLogger.Log(outLicenceEntitymsg);
                    balEncrytion objEncrypt = new balEncrytion();

                    string outmsg;
                    var login = _ballogin.LoginCredentialCheck(UserName, objEncrypt.Encrypt(Password), EntityCode, out outmsg, IpAddress);
                    FileLogger.Log(outmsg);
                    FileLogger.Log(JsonConvert.SerializeObject(login).ToString());
                    if (outmsg == "LS001")
                    {

                        //balPasswordComplexity _balPasswordComplexity = new balPasswordComplexity();
                        //PasswordComplexityModel passwordComplexity = null;
                        //try
                        //{
                        //    passwordComplexity = _balPasswordComplexity.GetAppliedPasswordComplexity(login.Item2[0].EntityId, "S", login.Item1.LoginID).FirstOrDefault();
                        //}
                        //catch
                        //{
                        //    passwordComplexity = null;
                        //}

                        //if (passwordComplexity != null && passwordComplexity.ShowReminder && passwordComplexity.RemainingDays > 0)
                        //{
                        //    Session["ExpiryReminder"] = $"Your password will expire {(passwordComplexity.RemainingDays == 0 ? "today!" : $"in next {passwordComplexity.RemainingDays} {(passwordComplexity.RemainingDays == 1 ? "day" : "days")}!")}";
                        //}

                        CheckUser(login.Item1.LoginID.ToString(), System.Web.HttpContext.Current.Session.SessionID);
                        sessionLogin = login.Item1;
                        Session["ResetPassword"] = login.Item1.IsResetPassword;
                        Session["UserFullName"] = login.Item1.FullName.Replace(",", "");
                        Session["FirstLogin"] = login.Item1.IsFirstLogin;
                        if (login.Item2.Count == 1)
                        {
                            sessionEntity = login.Item2.First();
                            int entityId = sessionEntity.EntityId;
                            int LoginID = login.Item1.LoginID;
                            Session["EntityID"] = entityId;
                            Session["LoginID"] = LoginID;
                            outmsg = "Dashboard";
                            TempData["Entity1"] = login.Item2;
                            outmsg = "Dashboard";
                            Session["oldPassword"] = Password;
                            Session["New_Resource_Assignment_Page"] = sessionEntity.NewResourceAssignment_Page;
                        }
                        string FilePath = System.Configuration.ConfigurationManager.AppSettings["SignatureUpload"].ToString();
                        string EntityPath = "\\" + sessionEntity.EntityId.ToString();
                        string folderpath = "\\User\\UserImages_" + login.Item1.LoginID + "\\";
                        if (System.IO.Directory.Exists(FilePath + EntityPath + folderpath))
                        {
                            var inputdirectory = new DirectoryInfo(FilePath + EntityPath + folderpath);
                            if (inputdirectory.GetFiles().Count() > 0)
                            {
                                Session["UserImage"] = inputdirectory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                            }
                            else
                            {
                                Session["UserImage"] = "";
                            }
                        }
                        else
                        {
                            Session["UserImage"] = "";
                        }
                    }
                    return Json(outmsg);
                }
                else
                {
                    return Json(outLicenceEntitymsg);
                }
            }
            catch (Exception ex)
            {
                FileLogger.Log(ex.ToString());
                return Json(ex.ToString());
            }
            

        }
        [HttpPost]
        public JsonResult LoginCredentialCheck(LoginRquest objLoginRequest)
        {
            ballogin _ballogin = new ballogin();
            Session["ECL"] = null;
            Session["ECL"] = objLoginRequest.Credential.Domain.ToUpper();
            string outLicenceEntitymsg = "S0001";
            //outLicenceEntitymsg = _ballogin.EntityLicenceValidity(EntityCode);
            if (outLicenceEntitymsg == "S0001")
            {

                balEncrytion objEncrypt = new balEncrytion();

                string outmsg;
                var login = _ballogin.LoginCredentialCheck(objLoginRequest.Credential.LoginID, objEncrypt.Encrypt(objLoginRequest.Credential.Password), objLoginRequest.Credential.Domain, out outmsg, objLoginRequest.Credential.IpAddress);
                if (outmsg == "LS001")
                {

                    CheckUser(login.Item1.LoginID.ToString(), System.Web.HttpContext.Current.Session.SessionID);
                    WSResponseModel objWSResponsecheck = new WSResponseModel();
                    WSHelper objWSHelper = new WSHelper();
                    string JsoRes;
                    string checkstrReqCode = "", checkstrReqMessage = "", password = objLoginRequest.Credential.Password;
                    Validate2FAResponse _Validate2FAResponse = new Validate2FAResponse();
                    string Encrypassword = objEncrypt.Encrypt(objLoginRequest.Credential.Password);
                    objLoginRequest.Credential.Password = Encrypassword;
                    string _objLoginRequestString = JsonConvert.SerializeObject(objLoginRequest, Newtonsoft.Json.Formatting.Indented);
                    JsoRes = objWSHelper.WebService2FACall(objLoginRequest.Credential.LoginID, Encrypassword, "Validate_2FA", _objLoginRequestString, "POST", out checkstrReqCode, out checkstrReqMessage);
                    _Validate2FAResponse = JsonConvert.DeserializeObject<Validate2FAResponse>(JsoRes);
                    //_Validate2FAResponse.IsValid = true;
                    if (!_Validate2FAResponse.IsValid)
                    {
                        Session.Clear();
                        Session.Abandon();
                        objWSResponsecheck.SrvStatus = "Invalid Code";
                        objWSResponsecheck.SrvStatusCode = "Please retry";
                        objWSResponsecheck.SrvStatusMessage = _Validate2FAResponse.StatusMessage;
                        return Json(objWSResponsecheck);

                    }
                    else
                    {
                        sessionLogin = login.Item1;
                        Session["ResetPassword"] = login.Item1.IsResetPassword;
                        if (login.Item2.Count == 1)
                        {
                            sessionEntity = login.Item2.First();
                            outmsg = "Dashboard";
                            TempData["Entity1"] = login.Item2;
                            outmsg = "Dashboard";
                            Session["oldPassword"] = password;
                            Session["New_Resource_Assignment_Page"] = sessionEntity.NewResourceAssignment_Page;
                        }
                        string FilePath = System.Configuration.ConfigurationManager.AppSettings["SignatureUpload"].ToString();
                        string EntityPath = "\\" + sessionEntity.EntityId.ToString();
                        string folderpath = "\\User\\UserImages_" + login.Item1.LoginID + "\\";
                        if (System.IO.Directory.Exists(FilePath + EntityPath + folderpath))
                        {
                            var inputdirectory = new DirectoryInfo(FilePath + EntityPath + folderpath);
                            if (inputdirectory.GetFiles().Count() > 0)
                            {
                                Session["UserImage"] = inputdirectory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                            }
                            else
                            {
                                Session["UserImage"] = "";
                            }
                        }
                        else
                        {
                            Session["UserImage"] = "";
                        }
                        return Json(outmsg);
                    }
                }
                else
                {
                    return Json(outmsg);
                }
            }
            else
            {
                return Json(outLicenceEntitymsg);
            }
        }

        public void CheckUser(string UserID, string sSessionID)
        {
            Dictionary<string, string> dic = (Dictionary<string, string>)System.Web.HttpContext.Current.Application["Sessions"];
            if (System.Web.HttpContext.Current.Application["Sessions"] == null)
            {
                dic = new Dictionary<string, string>();
                System.Web.HttpContext.Current.Application.Add("Sessions", dic);
                ((Dictionary<string, string>)System.Web.HttpContext.Current.Application["Sessions"]).Add(UserID, sSessionID);
            }
            else
            {

                if (dic.ContainsKey(UserID))
                {
                    string sessionid = dic[UserID];
                    ((Dictionary<string, string>)System.Web.HttpContext.Current.Application["Sessions"]).Remove(UserID);
                    ballogin _balLogin = new ballogin();
                    _balLogin.ClearSession(sessionid);
                    ((Dictionary<string, string>)System.Web.HttpContext.Current.Application["Sessions"]).Add(UserID, sSessionID);
                }
                else
                {
                    ((Dictionary<string, string>)System.Web.HttpContext.Current.Application["Sessions"]).Add(UserID, sSessionID);
                }
            }

        }
    }
}
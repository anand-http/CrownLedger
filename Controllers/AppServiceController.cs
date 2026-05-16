using DocumentFormat.OpenXml.Spreadsheet;
using fintech.BAL;
using fintech.Data;
using fintech.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using static fintech.Models.CommonModel;
using static fintech.Models.Login;

namespace fintech.Controllers
{
    public class AppServiceController : Controller
    {
        // GET: AppService
        [HttpPost]
        public JsonResult ws_GetSuggestedValue(string Table, string Prefix, string SearchText)
        {
            try
            {
                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                var returnlist = _balGetSuggestedValue.GetSuggestedValue(Table, Prefix, SearchText);
                return Json(returnlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ws_GetSuggestedValueWithChild(string Table, string Prefix, string SearchText, string WhereCond)
        {
            try
            {
                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                var returnlist = _balGetSuggestedValue.GetSuggestedValuewithChild(Table, Prefix, SearchText,WhereCond);
                return Json(returnlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SearchddlMultiSelect(string Table, string Prefix, string SearchText, string WhereCond)
        {
            try
            {
                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                var returnlist = _balGetSuggestedValue.GetSuggestedValuewithChild(Table, Prefix, SearchText, WhereCond);
                return Json(returnlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult ws_GetSuggestedValueWithCondition(string Table, string Prefix, string SearchText, string WhereCond)
        {
            try
            {
                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                var returnlist = _balGetSuggestedValue.GetSuggestedValue(Table, Prefix, SearchText, WhereCond);
                return Json(returnlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult ws_GetSuggestedValueForCon(string Table, string Prefix, string SearchText, string WhereCond, int EntityID)
        {
            try
            {
                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                var returnlist = _balGetSuggestedValue.GetSuggestedValue(Table, Prefix, SearchText, WhereCond);
                return Json(returnlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public List<AutoCompleteModel> ws_GetSuggestedValueForConClass(string Table, string Prefix, string SearchText, string WhereCond, int EntityID)
        {
            try
            {
                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                return _balGetSuggestedValue.GetSuggestedValue(Table, Prefix, SearchText, WhereCond);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult ws_GetMulitSelectSuggestedValue(string Table, string Prefix, string SearchText, string WhereCond, int pageSize, int pageNum)
        {
            try
            {



                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                AutoMulitSelectModelResult ObjList = new AutoMulitSelectModelResult();
                var returnlist = _balGetSuggestedValue.GetMulitSelectSuggestedValue(Table, Prefix, SearchText, pageSize, pageNum, WhereCond);
                if (returnlist.Count > 0)
                {
                    ObjList.Total = returnlist.First().TotalRecord;
                }
                else
                {
                    ObjList.Total = 0;
                }
                ObjList.Results = returnlist;
                return new JsonResult
                {
                    Data = ObjList,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult ws_GetSuggestedFlex(string Module, string Table, string SearchText)
        {
            try
            {
                //SessionObjects.SessionEntity sessEnt = SessionObjects.GetSessionValue<SessionObjects.SessionEntity>(SessionObjects.SessionEnum.Entity);
                //int iEntity = sessEnt == null || sessEnt.EntityId == 0 ? 1 : sessEnt.EntityId;

                balGetSuggestedValueController _balGetSuggestedValue = new balGetSuggestedValueController();
                var returnlist = _balGetSuggestedValue.GetSuggestedFlexValue(Module, Table, SearchText);
                return Json(returnlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CdnSingleFileUpload()
        {
            try
            {
                string JsoRes = "";
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["Image"];
                    var FileID = System.Web.HttpContext.Current.Request.Form["FileID"];
                    var ImageType = System.Web.HttpContext.Current.Request.Form["ImageType"];
                    var ImageSubType = System.Web.HttpContext.Current.Request.Form["ImageSubType"];
                    var ImageId = System.Web.HttpContext.Current.Request.Form["ImageId"];
                    var Datatext = System.Web.HttpContext.Current.Request.Form["Datatext"];
                    string base64 = "";
                    string FileStringData = "";
                    Credential cred = new Credential();
                    cred.Domain = ConfigurationManager.AppSettings["ClientDomain"]?.ToString();
                    cred.LoginID = ConfigurationManager.AppSettings["ClientLoginID"]?.ToString();
                    cred.Password = ConfigurationManager.AppSettings["ClientPassword"]?.ToString();
                    cred.LanguageLocale = ConfigurationManager.AppSettings["ClientLanguageLocale"]?.ToString();
                    //cred = (Credential)Session["Credential"];
                    if (Datatext != "" && Datatext != null)
                    {
                        base64 = Datatext.Substring(Datatext.IndexOf(',') + 1);
                        base64 = base64.Trim('\0');
                    }
                    if (pic.ContentLength > 0)
                    {
                        Guid id = Guid.NewGuid();
                        HttpPostedFileBase file = Request.Files[0];
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(pic.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(pic.ContentLength);
                        }
                        if (base64 != "")
                        {
                            FileStringData = base64;
                        }
                        else
                        {
                            FileStringData = Convert.ToBase64String(fileData);

                        }
                        var ClientDomain = cred.Domain;
                        MultiFileUpload multiFileUpload = new MultiFileUpload() { Domain = ClientDomain };
                        multiFileUpload.Files = new List<Models.File>();
                        multiFileUpload.Files.Add(new Models.File()
                        {
                            FileId = FileID.ToString(),
                            GenerateFileName = false,
                            FilePath = "UserDoc",
                            UploadText = FileStringData,
                            Domain = ClientDomain,
                            ImageType = ImageType,
                            ImageSubType = ImageSubType,
                            ImageId = ImageId,
                        });

                        int cahceDuration = Convert.ToInt16(ConfigurationManager.AppSettings["CMSCacheDuration"].ToString());

                        string strReqCode = "", strReqMessage = "";
                        string CDNUploadURL = ConfigurationManager.AppSettings["CDNIMGUploadURL"]?.ToString();
                        WSHelper wSHelper = new WSHelper();
                        JsoRes = wSHelper.WebServiceImageUpload(CDNUploadURL, JsonConvert.SerializeObject(multiFileUpload), "POST", out strReqCode, out strReqMessage);
                        if (ImageType.ToString() == "User")
                        {
                            if (JsoRes != "")
                            {
                                UploadFileResponseCDN res = JsonConvert.DeserializeObject<UploadFileResponseCDN>(JsoRes);
                                if (res.Files != null)
                                {

                                    Session["DashboardIMG"] = res.Files[0].CloudAccessURL;
                                }

                            }
                        }


                    }

                }
                return Json(JsoRes);

            }
            catch (Exception ex)
            {
                return Json("fail-" + ex.Message);
            }
        }
    }
}
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using fintech.BAL;
using fintech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using YourApp.Models;
using static fintech.Models.ConfigurationModel;

namespace fintech.Controllers
{
    public class ConfigurationController : Controller

    {
        [HttpPost]
        public JsonResult BranchAction(BranchDetailCreate model, FormCollection form)
        {

            var bal = new fintech.BAL.ConfigurationBAL();
            string actionType = form["ActionType"];
            int entityId = Convert.ToInt32(Session["EntityID"]);
            int? loginId = Session["LoginID"] as int?;

            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = bal.SaveBranch(model, entityId, loginId, out messageCode, actionType);
                result = new
                {
                    status = success,
                    message = messageCode
                };
            }
            catch (Exception ex)
            {
                result = new
                {
                    status = false,
                    message = ex.Message
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       // GET: Configuration
        public ActionResult AddBranch()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Branch_ID desc";
            var bal = new fintech.BAL.ConfigurationBAL();
            var branches = bal.GetBranchesList(entityId, 0, length, 0, null);
            return View("add_branch", branches);
        }

        [HttpPost]
        public JsonResult GetBranchesPaged(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault());
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault());
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault());

            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() +
                                        "][name]")?.FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();

            var search = Request.Form.GetValues("search[value]").FirstOrDefault() ?? "";

            for (int i = 0; i < icolelngth; i++)
            {
                if (Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() != "")
                {
                    search = search + " and " + Request.Form.GetValues("columns[" + i + "][name]")?.FirstOrDefault() + " like '%" + Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() + "%'";
                }
            }

            string Orderby = "";
            if (sortColumn != "")
            {
                Orderby = "Order By " + sortColumn + " " + sortColumnDir;
            }

            var bal = new fintech.BAL.ConfigurationBAL();
            GetBranchDetails_Result result = bal.GetBranchesList(entityId, start, length, Orderby);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.BranchDetail.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.BranchDetail.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.BranchDetail.First().Records);
            }
            dataTableData.data = result.BranchDetail;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBranch(bool isChecked, int? branchId,bool isDelete=false)
        {
            var bal = new fintech.BAL.ConfigurationBAL();
            bool updated = bal.UpdateBranch(isChecked, branchId,isDelete);

            if (updated)
                return Json(new { success = true, message = "Status updated successfully." });
            else
                return Json(new { success = false, message = "Failed to update status." });
        }
        [HttpPost]
        public JsonResult DeleteUser(int? userId)
        {
            var bal = new fintech.BAL.ConfigurationBAL();
            bool updated = bal.DeleteUser(userId);

            if (updated)
                return Json(new { success = true, message = "Status updated successfully." });
            else
                return Json(new { success = false, message = "Failed to update status." });
        }

        [HttpPost]
        public JsonResult UpdateDepartment(bool isChecked, int? departmentId, bool isDelete=false)
        {
            var bal = new fintech.BAL.ConfigurationBAL();
            bool updated = bal.UpdateDepartment(isChecked, departmentId, isDelete);

            if (updated)
                return Json(new { success = true, message = "Status updated successfully." });
            else
                return Json(new { success = false, message = "Failed to update status." });
        }
        public ActionResult AddBranchSettings(int? id = null)
        {
            BranchDetailList model = new BranchDetailList(); 

            if (id.HasValue)
            {
                int? entityId = Session["EntityID"] as int?;
                int length = 10;
                string orderby = "order by Branch_ID desc";

                var bal = new fintech.BAL.ConfigurationBAL();
                var data = bal.GetBranchesList(entityId, id.Value, 0, length, orderby);

                var result = data.BranchDetail.FirstOrDefault();

                if (result != null)
                {
                    model = result; 
                }
            }

            return View("add-branch-settings", model);
        }

        public ActionResult BusinessCalender()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by BusCal_ID desc";
            var bal = new fintech.BAL.ConfigurationBAL();
            var busclanedar = bal.GetBussinessCalendarList(entityId, 0, length, orderby, null);
            return View(busclanedar);
        }

        public ActionResult Department()
        {
            int? entityId = Session["EntityID"] as int?;
            int length = 10;
            string orderby = "order by Deptmt_ID desc";
            var bal = new fintech.BAL.ConfigurationBAL();
            var departments = bal.GetDepartmentList(entityId, 0, length, orderby, null);
            return View("department", departments);
        }

        [HttpPost]
        public JsonResult GetDepartmentList(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault());
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault());
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault());

            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() +
                                        "][name]")?.FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();

            var search = Request.Form.GetValues("search[value]").FirstOrDefault() ?? "";

            for (int i = 0; i < icolelngth; i++)
            {
                if (Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() != "")
                {
                    search = search + " and " + Request.Form.GetValues("columns[" + i + "][name]")?.FirstOrDefault() + " like '%" + Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() + "%'";
                }
            }

            string Orderby = "";
            if (sortColumn != "")
            {
                Orderby = "Order By " + sortColumn + " " + sortColumnDir;
            }

            var bal = new fintech.BAL.ConfigurationBAL();
            GetDepartmentResult result = bal.GetDepartmentList(entityId, start, length, Orderby,search);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.DepartmentDetail.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.DepartmentDetail.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.DepartmentDetail.First().Records);
            }
            dataTableData.data = result.DepartmentDetail;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBussinessCalendarList(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault());
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault());
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault());

            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() +
                                        "][name]")?.FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();

            var search = Request.Form.GetValues("search[value]").FirstOrDefault() ?? "";

            for (int i = 0; i < icolelngth; i++)
            {
                if (Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() != "")
                {
                    search = search + " and " + Request.Form.GetValues("columns[" + i + "][name]")?.FirstOrDefault() + " like '%" + Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() + "%'";
                }
            }

            string Orderby = "";
            if (sortColumn != "")
            {
                Orderby = "Order By " + sortColumn + " " + sortColumnDir;
            }

            var bal = new fintech.BAL.ConfigurationBAL();
            GetbusscalenResult result = bal.GetBussinessCalendarList(entityId, start, length, Orderby, search);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.bussCalDetail.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.bussCalDetail.First().TotalRecord);
                dataTableData.recordsFiltered = Convert.ToInt64(result.bussCalDetail.First().TotalRecord);
            }
            dataTableData.data = result.bussCalDetail;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePeriod(SavePeriodModel objPeriod)
        {
            try
            {
                if (objPeriod == null)
                    return Json("error", JsonRequestBehavior.AllowGet);

                int? entityId = Session["EntityID"] as int?;
                int? loginId = Session["LoginID"] as int?;

                objPeriod.Entity_ID = entityId;
                objPeriod.Login_ID = loginId;

                var bal = new fintech.BAL.ConfigurationBAL();
                string result = bal.SavePeriod(objPeriod);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log exception here
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeletePeriod(int documentId)
        {
            try
            {
                var bal = new fintech.BAL.ConfigurationBAL();
                string result = bal.DeletePeriod(documentId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DepartmentAction(Department model, FormCollection form)
        {
            var bal = new fintech.BAL.ConfigurationBAL();
            int? entityId = Session["EntityID"] as int?;
            int? loginId = Session["LoginID"] as int?;
            model.Deptmt_Private = form["Deptmt_Private"] == "true";
            model.Deptmt_Group = form["Deptmt_Group"] == "true";
            model.Active = form["Active"] == "true";
            var result = new { status = false, message = "" };
            try
            {
                string messageCode;
                bool success = bal.DepartmentAction(model, entityId, loginId, out messageCode);
                result = new
                {
                    status = success,
                    message = messageCode
                };
            }
            catch (Exception ex)
            {
                result = new
                {
                    status = false,
                    message = ex.Message
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NumberSeries()
        {

            var bal = new fintech.BAL.ConfigurationBAL();
            var series = bal.GetNumberSeriesList();
            string jsores = JsonConvert.SerializeObject(series);
            return View("number-series",series);
        }

        [HttpPost]
        public JsonResult GetNumberSeriesList(int draw, int start, int length, int icolelngth, int fiscalYear)
        {
            int? entityId = Session["EntityID"] as int?;

            var bal = new fintech.BAL.ConfigurationBAL();
            DocumentSequenceResult result = bal.GetNumberSeriesList();

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            dataTableData.recordsTotal = result.Series.Count;
            dataTableData.recordsFiltered = result.Series.Count;
            dataTableData.data = result.Series;

            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransactionApproval()
        {
            return View("transaction_approval");
        }

        public ActionResult TransactionsSettings()
        {
            int? entityId = Session["EntityID"] as int?;
            int? loginId = Session["LoginID"] as int?;
            var bal = new fintech.BAL.ConfigurationBAL();
            var model = bal.TransactionSettings(entityId, loginId);
            return View("transaction_settings",model);
        }

        public ActionResult UserAccess()
        {
            return View("user-access");
        }
        [HttpPost]
        public JsonResult GetUserAccess(int icolelngth)
        {
            int? entityId = Session["EntityID"] as int?;
            var draw = int.Parse(Request.Form.GetValues("draw")?.FirstOrDefault());
            var start = int.Parse(Request.Form.GetValues("start")?.FirstOrDefault());
            int length = int.Parse(Request.Form.GetValues("length")?.FirstOrDefault());

            var sortColumn =
                Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]")?.FirstOrDefault() +
                                        "][name]")?.FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]")?.FirstOrDefault();

            var search = Request.Form.GetValues("search[value]").FirstOrDefault() ?? "";

            for (int i = 0; i < icolelngth; i++)
            {
                if (Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() != "")
                {
                    search = search + " and " + Request.Form.GetValues("columns[" + i + "][name]")?.FirstOrDefault() + " like '%" + Request.Form.GetValues("columns[" + i + "][search][value]")?.FirstOrDefault() + "%'";
                }
            }

            string Orderby = "";
            if (sortColumn != "")
            {
                Orderby = "Order By " + sortColumn + " " + sortColumnDir;
            }

            var bal = new fintech.BAL.ConfigurationBAL();
            GetUserAccess_Result result = bal.GetUserAccess(entityId, start, length);

            lDataTable dataTableData = new lDataTable();
            dataTableData.draw = draw;
            if (result.UserAccessDetail.Count() == 0)
            {
                dataTableData.recordsTotal = 0;
                dataTableData.recordsFiltered = 0;
            }
            else
            {
                dataTableData.recordsTotal = Convert.ToInt64(result.UserAccessDetail.First().Records);
                dataTableData.recordsFiltered = Convert.ToInt64(result.UserAccessDetail.First().Records);
            }
            dataTableData.data = result.UserAccessDetail;
            return Json(dataTableData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUserAccess(SaveUserAccessRequest model)
        {
            try
            {
                var bal = new fintech.BAL.ConfigurationBAL();
                int? entityId = Session["EntityID"] as int?;
                int? loginId = Session["LoginID"] as int?;
                string messageCode;
                bool result = bal.SaveUserAccess(model, entityId, loginId, out messageCode);

                if (result)
                {
                    return Json(new { success = true, message = "User access saved successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save user access" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetUserAccessById(int userId)
        {
            try
            {
                int? entityId = Session["EntityID"] as int?;
                var bal = new fintech.BAL.ConfigurationBAL();
                var userDetails = bal.GetUserDetails(userId, entityId,0,0);

                if (userDetails == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        userName = userDetails.UserName,
                        email = userDetails.Email,
                        moduleAccess = userDetails.ModuleAccess.Select(m => new
                        {
                            moduleDesc = m.ModuleDesc,
                            isRead = m.IsRead,
                            isWrite = m.IsWrite
                        }).ToList()
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SendEmail(GridActionEmailRequest request)
        {
            try
            {
                var bal = new fintech.BAL.ConfigurationBAL();
                bal.SendEmail(request.ToEmail, request.EmailSub, request.EmailBody);

                return Json(new { success = true, message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // Save Transaction Setting
        [HttpPost]
        public JsonResult SaveTransactionSettings(TransactionSettingsRequest request)
        {
            try
            {
                if (request == null)
                    return Json(new ApiResponse { Success = false, Message = "Invalid request data." });

                int? entityId = Session["EntityID"] as int?;
                int? loginId = Session["LoginID"] as int?;
                string messageCode;
                var bal = new fintech.BAL.ConfigurationBAL();
                bool result = bal.SaveTransactionSettings(request, entityId, loginId, out messageCode);

                return result
                    ? Json(new ApiResponse { Success = true, Message = "Transaction settings saved successfully." })
                    : Json(new ApiResponse { Success = false, Message = "Failed to save transaction settings." });
            }
            catch (Exception ex)
            {
                // Log ex here
                return Json(new ApiResponse { Success = false, Message = "An unexpected error occurred: " + ex.Message });
            }
        }

    }
}
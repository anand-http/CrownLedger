using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using fintech.Models;
using static fintech.Models.CommonModel;
using fintech.DAL;

namespace fintech.BAL
{
    public class balGetSuggestedValueController : Controller
    {
        // GET: balGetSuggestedValue
        public List<AutoCompleteModel> GetSuggestedValue(string table, string prefix, string text, string whereCond = "")
        {
            try
            {
                var session = System.Web.HttpContext.Current?.Session;

                int entityId = session != null && session["EntityID"] != null
                               ? Convert.ToInt32(session["EntityID"])
                               : 0;
                AutoCompleteRequestModel request = new AutoCompleteRequestModel() { Entity_ID = entityId, Prefix = prefix, Table = table, Text = text, WhereCond = whereCond };
                using (dalGetSuggestedValue _dalGetSuggestedValue = new dalGetSuggestedValue())
                {
                    return _dalGetSuggestedValue.GetSuggestedValue(request).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AutoMulitSelectModel> GetMulitSelectSuggestedValue(string table, string prefix, string text, int pageSize, int pageNum, string whereCond = "")
        {
            try
            {
                AutoCompleteRequestModel request = new AutoCompleteRequestModel() { Entity_ID = 0, Prefix = prefix, Table = table, Text = text, WhereCond = whereCond, PageNum = pageNum, PageSize = pageSize };
                using (dalGetSuggestedValue _dalGetSuggestedValue = new dalGetSuggestedValue())
                {
                    return _dalGetSuggestedValue.GetMulitSelectSuggestedValue(request).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AutoCompleteWithChildModel> GetSuggestedValuewithChild(string table, string prefix, string text, string whereCond = "")
        {
            try
            {
                AutoCompleteRequestModel request = new AutoCompleteRequestModel() { Entity_ID = 0, Prefix = prefix, Table = table, Text = text, WhereCond = whereCond };
                using (dalGetSuggestedValue _dalGetSuggestedValue = new dalGetSuggestedValue())
                {
                    return _dalGetSuggestedValue.GetSuggestedValuewithChild(request).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AutoCompleteFlexModel> GetSuggestedFlexValue(string Module, string Table, string SearchText, string whereCond = "")
        {
            try
            {
                AutoCompleteRequestModel request = new AutoCompleteRequestModel() { Entity_ID = 0, Module = Module, Table = Table, Text = SearchText, WhereCond = whereCond };
                using (dalGetSuggestedValue _dalGetSuggestedValue = new dalGetSuggestedValue())
                {
                    return _dalGetSuggestedValue.GetSuggestedFlexValue(request).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<itemList> GetItemDetails(string Table, string Prefix, string SearchText)
        //{
        //    var dal = new fintech.DAL.SalesDAL();
        //    return dal.GetItemDetails(Table, Prefix, SearchText);
        //}
    }
}
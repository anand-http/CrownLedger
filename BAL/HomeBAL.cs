using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using fintech;
using fintech.DAL;
using System;
using System.Collections.Generic;

namespace fintech.BAL
{
    public class HomeBAL
    {
        private HomeDAL dal = new HomeDAL();

        public Company GetCompanyDetails(int id)
        {
            return dal.GetCompanyDetails(id);
        }
        public List<sp_GetOpeningBalanceGridData_Result> GetOpeningBalanceGridData(DateTime date)
        {
            return dal.GetOpeningBalanceGridData(date);
        }

        public List<usp_GetBankList_Result> GetBankList()
        {
            return dal.GetBankList();
        }

        public List<GetBankAccounts_Result> GetBankAccounts()
        {
            return dal.GetBankAccounts();
        }

        public int DeleteBankAccount(int accountId)
        {
            return dal.DeleteBankAccount(accountId);
        }

        public List<sp_GetProductAndService_Result> GetProductAndService(int? entityId, int start, int length, string Orderby, string search, string statusFilter,string startDate, string endDate)
        {
            return dal.GetProductAndService(entityId, start, length, Orderby, search, statusFilter, startDate, endDate);
        }

        public List<GetAllChartOfAccounts_Result> GetAllChartOfAccounts(int? entityId, int start, int length, string orderby, string search, string statusFilter)
        {
            return dal.GetAllChartOfAccounts(entityId, start, length, orderby, search, statusFilter);
        }
        public string SaveOrUpdateCompany(GetCompanyDetails_Result model)
        {
            return dal.SaveOrUpdateCompany(model);
        }

        public string SaveOrUpdateGLCode(GetAllChartOfAccounts_Result model, string userName)
        {
            return dal.SaveOrUpdateGLCode(model, userName);
        }

        public string SaveOrUpdateProductAndService(sp_GetProductAndService_Result model)
        {
           return dal.SaveOrUpdateProductAndService(model);
        }
        public void AddNewTax(mst_Tax model)
        {
            dal.AddNewTax(model);
        }
        public List<mst_Tax> GetTaxList(int? entityId, int start, int length, string orderby, string search,int gst, int tds,int other)
        {
            return dal.GetTaxList(entityId, start, length, orderby, search,gst,tds,other);
        }
        public mst_Currency GetCurrencyList(int? entityId, int start, int length, string orderby, string search)
        {
            return dal.GetCurrencyList(entityId, start, length, orderby, search);
        }

        public bool SaveTax(mst_Tax model, List<TaxDetail> Details)
        {
            return dal.SaveTax(model,Details);
        
        }

        public bool SaveExchangeRate(ExchangeRateViewModel model)
        {
            // Business validations
            if (model.FromCurrencyId == model.ToCurrencyId)
                throw new Exception("From Currency and To Currency cannot be the same.");

            if (model.CurrencyRate <= 0)
                throw new Exception("Currency rate must be greater than zero.");

            if (string.IsNullOrWhiteSpace(model.MultDiv) ||
               (model.MultDiv != "Multiply" && model.MultDiv != "Divide"))
                throw new Exception("Mult/Div selection is invalid.");

            return dal.SaveExchangeRate(model);
        }

        public string SaveRateType(CurrencyRateType model)
        {
            // Business rule: RateTypeCode must be unique (checked in SP)
            // Trim inputs before saving
            model.RateTypeCode = model.RateTypeCode?.Trim().ToUpper();
            model.Description = model.Description?.Trim();
            return dal.SaveRateType(model);
        }
    }
}   

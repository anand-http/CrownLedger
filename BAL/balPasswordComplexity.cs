using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static fintech.Models.PasswordComplexity;
using fintech.DAL;
using fintech.Data;

namespace fintech.BAL
{
    public class balPasswordComplexity
    {
        public IEnumerable<PasswordComplexityModel> GetPasswordComplexitys(int EntityID, int start, int length, string orderby, string search)
        {
            dalPasswordComplexity _dalPasswordComplexity = null;
            try
            {
                _dalPasswordComplexity = new dalPasswordComplexity();
                PasswordComplexityModel _model = new PasswordComplexityModel()
                {
                    Entity_ID = EntityID,
                    start = start,
                    length = length,
                    orderby = orderby,
                    search = search,
                    Active = true
                };
                return _dalPasswordComplexity.GetPasswordComplexitys(_model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalPasswordComplexity != null)
                    _dalPasswordComplexity.Dispose();
            }
        }

        public PasswordComplexityModel GetPassWordComplexityDetails(int EntityID, long passCompID)
        {
            Lazy<dalPasswordComplexity> _dalPasswordComplexity = null;
            try
            {
                _dalPasswordComplexity = new Lazy<dalPasswordComplexity>();
                PasswordComplexityModel _model = new PasswordComplexityModel()
                {
                    Entity_ID = EntityID,
                    PassComp_ID = passCompID,
                    start = 1,
                    length = 1
                };
                var passComplexities = _dalPasswordComplexity.Value.GetPasswordComplexitys(_model);
                return passComplexities.Count() > 0 ? passComplexities.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalPasswordComplexity != null && _dalPasswordComplexity.IsValueCreated)
                    _dalPasswordComplexity.Value.Dispose();
            }

        }

        public string AddPasswordComplexity(PasswordComplexityModel model, int ientity, int ilogin)
        {
            Lazy<dalPasswordComplexity> _dalPasswordComplexity = null;
            try
            {
                _dalPasswordComplexity = new Lazy<dalPasswordComplexity>();
                PasswordComplexityModel _model = new PasswordComplexityModel();
                _model.Entity_ID = ientity;
                _model.LoginID = ilogin;
                List<PasswordComplexityModel> passwordComplexities = new List<PasswordComplexityModel>();
                passwordComplexities.Add(model);

                string response = _dalPasswordComplexity.Value.Save_PasswordComplexity(_model, false, passwordComplexities.ConvertObjectToXml("Entries"));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalPasswordComplexity != null && _dalPasswordComplexity.IsValueCreated)
                    _dalPasswordComplexity.Value.Dispose();
            }
        }

        public string UpdatePasswordComplexity(PasswordComplexityModel _model, int ientity, int ilogin)
        {
            Lazy<dalPasswordComplexity> _dalPasswordComplexity = null;
            try
            {
                _dalPasswordComplexity = new Lazy<dalPasswordComplexity>();
                _model.Entity_ID = ientity;
                _model.LoginID = ilogin;
                List<PasswordComplexityModel> passwordComplexities = new List<PasswordComplexityModel>();
                passwordComplexities.Add(_model);

                string response = _dalPasswordComplexity.Value.Save_PasswordComplexity(_model, false, passwordComplexities.ConvertObjectToXml("Entries"));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalPasswordComplexity != null && _dalPasswordComplexity.IsValueCreated)
                    _dalPasswordComplexity.Value.Dispose();
            }
        }

        public string DeletePasswordComplexity(PasswordComplexityModel _model, int ientity, int ilogin)
        {
            Lazy<dalPasswordComplexity> _dalPasswordComplexity = null;
            try
            {
                _dalPasswordComplexity = new Lazy<dalPasswordComplexity>();
                _model.Entity_ID = ientity;
                _model.LoginID = ilogin;
                List<PasswordComplexityModel> passwordComplexities = new List<PasswordComplexityModel>();
                passwordComplexities.Add(_model);

                string response = _dalPasswordComplexity.Value.Save_PasswordComplexity(_model, true, passwordComplexities.ConvertObjectToXml("Entries"));
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalPasswordComplexity != null && _dalPasswordComplexity.IsValueCreated)
                    _dalPasswordComplexity.Value.Dispose();
            }
        }
        public IEnumerable<PasswordComplexityModel> GetAppliedPasswordComplexity(int ientity, string module = "S", long loginID = 0)
        {
            Lazy<dalPasswordComplexity> _dalPasswordComplexity = null;
            try
            {
                _dalPasswordComplexity = new Lazy<dalPasswordComplexity>();
                List<PasswordComplexityModel> passwordComplexities = null;

                passwordComplexities = _dalPasswordComplexity.Value.GetCurrentPasswordComplexity(ientity, module, loginID).ToList();
                return passwordComplexities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalPasswordComplexity != null && _dalPasswordComplexity.IsValueCreated)
                    _dalPasswordComplexity.Value.Dispose();
            }
        }
    }
}
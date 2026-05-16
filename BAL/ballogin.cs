using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static fintech.Models.Login;
using static fintech.Models.LicenseModel;
using fintech.DAL;

namespace fintech.BAL
{
    public class ballogin
    {
        public Tuple<LoginModel, List<LoginEntityModel>> LoginCredentialCheck(string Uname, string Pwd, string EntityCode, out string strstatus, string IpAddress)
        {
            dalLogin _dalLogin = null;
            try
            {
                _dalLogin = new dalLogin();
                return _dalLogin.LoginCredentialCheck(Uname, ref Pwd, ref EntityCode, out strstatus, IpAddress);
                //if (login.Item1 != null)
                //{

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    if (_dalLogin != null && _dalLogin.IsValueCreated)
            //    {
            //        _dalLogin.Value.Dispose();
            //    }
            //}
        }
        public string ClearSession(string SessionID)
        {
            dalLogin _dalLogin = null;
            try
            {
                _dalLogin = new dalLogin();
                return _dalLogin.ClearSession(SessionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<UserAccessModel> getPageAccess(int menuID, int userID, int entityID)
        {
            dalLogin _dalLogin = null;
            try
            {
                _dalLogin = new dalLogin();
                return _dalLogin.getPageAccess(menuID, userID, entityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string changepassword(int LoginID, string Password, int entityid)
        {
            dalLogin _dalLogin = null;
            try
            {
                _dalLogin = new dalLogin();
                string msgout = _dalLogin.ChangePassword(LoginID, Password, entityid);
                return msgout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string EntityLicenceValidity(string EntityCode)
        {
            dalLogin _dalLogin = null;
            try
            {
                _dalLogin = new dalLogin();
                string msgout = _dalLogin.EntityLicenceValidity(EntityCode);
                return msgout;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public LicenceModel EntityVerifyEntityEmail(string EntityCode, string Email, out string EmailOTP, out string msgcode, out int EntityId)
        {

            dalLogin _dalLogin = null;
            try
            {
                _dalLogin = new dalLogin();
                return _dalLogin.EntityVerifyEntityEmail(EntityCode, Email, out EmailOTP, out msgcode, out EntityId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_dalLogin != null)
                    _dalLogin.Dispose();
            }
        }
    }
}
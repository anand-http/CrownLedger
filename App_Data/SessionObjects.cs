using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using  fintech.Models;

namespace fintech.App_Data
{
    public class SessionObjects : Controller
    {
        // GET: SessionObjects
        public enum SessionEnum
        {
            Login = 0,
            Entity
        }
        public Login.LoginModel sessionLogin
        {
            get
            {
                return Session[SessionEnum.Login.ToString()] as Login.LoginModel;
            }
            set
            {
                Session[SessionEnum.Login.ToString()] = value;
            }
        }

        public Login.LoginEntityModel sessionEntity
        {
            get
            {
                return Session[SessionEnum.Entity.ToString()] as Login.LoginEntityModel;
                //return new LoginModels.LoginEntityModel() { EntityId = 1, EntityName = "3E DMC" };
            }
            set
            {
                Session[SessionEnum.Entity.ToString()] = value;
            }
        }

        public static string GetUniqueSessionID()
        {
            try
            {
                return System.Web.HttpContext.Current.Session.SessionID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsSessionHasValue<T>(SessionEnum sess_enum)
        {
            try
            {
                return (System.Web.HttpContext.Current.Session[sess_enum.ToString()] is T);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static T GetSessionValue<T>(SessionEnum sess_enum)
        {
            try
            {
                return (T)System.Web.HttpContext.Current.Session[sess_enum.ToString()];
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void SetSessionValue<T>(SessionEnum sess_enum, T value)
        {
            try
            {
                System.Web.HttpContext.Current.Session[sess_enum.ToString()] = value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class CheckSessionTimeOutAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
    {
        var context = filterContext.HttpContext;
        if (System.Web.HttpContext.Current.Session != null)
        {
            if (System.Web.HttpContext.Current.Session.IsNewSession)
            {
                //string sessionCookie = context.Request.Headers["Cookie"];
                //if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET&#95;SessionId") >= 0))
                //{
                //   // FormsAuthentication.SignOut();

                //}
                filterContext.Result =
        new RedirectToRouteResult(new RouteValueDictionary{
             { "action", "LogOut" },
            { "controller", "Login" },
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}
           });

                return;
            }
        }
        base.OnActionExecuting(filterContext);
    }
}
using Shadows.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Shadows.Web
{
    public class ApplicationSession
    {
        public const string USER = "USER";

        private ApplicationSession() { }

        public static void SetUser(Character user)
        {
            Page p = (Page)HttpContext.Current.Handler;
            p.Session[USER] = user;
        }

        public static void Logout()
        {
            Page p = (Page)HttpContext.Current.Handler;
            p.Session[USER] = null;
        }

        public static Character GetUser()
        {
            Page p = (Page)HttpContext.Current.Handler;
            object o = p.Session[USER];
            if (o != null)
            {
                return (Character)o;
            }
            else
            {
                return null;
            }
        }

        public static int GetUserId()
        {
            Page p = (Page)HttpContext.Current.Handler;
            object o = p.Session[USER];
            if (o != null)
            {
                return ((Character)o).Id;
            }
            else
            {
                return 0;
            }
        }

        public static bool LoggedIn
        {
            get
            {
                Page p = (Page)HttpContext.Current.Handler;
                object o = p.Session[USER];
                if (o != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsAdmin
        {
            get
            {
                Page p = (Page)HttpContext.Current.Handler;
                object o = p.Session[USER];
                if (o != null)
                {
                    return ((Character)o).Administrator;
                }
                else
                {
                    return false;
                }
            }
        }

        #region SessionParams
        public static void SetValue(object v, string code)
        {
            Page p = (Page)HttpContext.Current.Handler;
            p.Session[code] = v;
        }

        public static object GetValue(string code)
        {
            Page p = (Page)HttpContext.Current.Handler;
            object o = p.Session[code];
            return o;
        }

        public static bool GetValueBool(string code)
        {
            object o = GetValue(code);
            if (o != null)
            {
                return (bool)o;
            }
            else
            {
                return false;
            }
        }

        public static int GetValueInt(string code)
        {
            object o = GetValue(code);
            if (o != null)
            {
                return (int)o;
            }
            else
            {
                return 0;
            }
        }

        public static string GetValueString(string code)
        {
            object o = GetValue(code);
            if (o != null)
            {
                return (string)o;
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion
    }
}
using Shadows.Data;
using Shadows.Data.Facade;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Shadows.Web
{
    public class CustomWebPage : System.Web.UI.Page
    {
        #region Constructor
        public CustomWebPage()
        { 
        }
        #endregion

        #region Events

        #region RedirectOut
        public void RedirectOut()
        {
            Response.Redirect("Login.aspx", true);
        }
        #endregion

        #region RedirectIn
        public void RedirectIn()
        {
            Response.Redirect("Default.aspx", false);
        }
        #endregion

        #region ShowMessage
        public void ShowMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), String.Format("alert('{0}');", message), true);
        }
        #endregion

        public int EditId
        {
            get
            {
                object obj = ViewState["_EditId"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_EditId"] = value;
            }
        }

        public string EditDetailGuid
        {
            get
            {
                object obj = ViewState["_EditDetailGuid"];
                if (obj == null)
                    return String.Empty;
                else
                    return (string)obj;
            }
            set
            {
                this.ViewState["_EditDetailGuid"] = value;
            }
        }

        public int EditDetailId
        {
            get
            {
                object obj = ViewState["_EditDetailId"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_EditDetailId"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            _nhibernateSession = NHibernateHelper.OpenSession();
            base.OnInit(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (_nhibernateSession != null)
            {
                _nhibernateSession.Flush();
                _nhibernateSession.Clear();
                _nhibernateSession.Close();
                _nhibernateSession.Dispose();
                _nhibernateSession = null;
            }
        }

        #endregion

        #region NHibernateSession
        private ISession _nhibernateSession;
        public ISession NHibernateSession
        {
            get
            {
                return _nhibernateSession;
            }
        }
        #endregion

        #region Return pdf file
        protected void ReturnPdfFile(MemoryStream ms, string caption)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + caption + ".pdf");
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.Close();
            Response.End();
        }
        #endregion

        #region Return excel file
        protected void ReturnExcelFile(MemoryStream ms, string filename)
        {
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
            Response.AddHeader("content-disposition", "inline; filename=vyplneno.xlsm");
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();
        }
        #endregion

        public void ShowBootstrapPopup(string popupId)
        {
            if (!String.IsNullOrEmpty(popupId))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "ShowBootstrapPopup('" + popupId + "');", true);
            }
        }

    }
}
using Shadows.Data;
using Shadows.Data.Facade;
using Shadows.Data.Model;
using Shadows.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = String.Empty;

        if (!IsPostBack)
        {
            string userName = Request.QueryString["login"];
            string password = Request.QueryString["password"];

            if ((!String.IsNullOrEmpty(userName)) && (!String.IsNullOrEmpty(password)))
            {
                txtUserName.Text = userName;
                txtPassword.Text = password;
                Login_Click(null, null);
            }
        }
    }

    protected void Login_Click(object sender, EventArgs e)
    {
        string password = txtPassword.Text;
        string userName = txtUserName.Text;

        CharacterFacade f = new CharacterFacade(NHibernateSession);
        Character u = f.Login(userName, password);

        if (u != null)
        {
            ApplicationSession.SetUser(u);
            RedirectIn();
        }
        else
        {
            lblError.Text = "Špatné přihlašovací údaje";
        }
    }
}
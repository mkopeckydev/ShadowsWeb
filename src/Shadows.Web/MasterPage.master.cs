using Shadows.Data.Facade;
using Shadows.Data.Model;
using Shadows.Web;
using System;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        Character u = ApplicationSession.GetUser();

        if (u != null)
        {
            lblLoggedInfoName.Text = u.Name;
            lblLoggedInfoClass.Text = u.CharacterClass;
            lblLoggedInfoClass2.Text = u.CharacterClass2;

            btnLogout.Visible = true;

            hlCharacters.Visible = u.Administrator;
            hlCharactersMenu.Visible = u.Administrator;

            hlSkils.Visible = u.Administrator;
            hlSkilsMenu.Visible = u.Administrator;

            btnChangePassword.Visible = !u.IsDemoUser;
        }
        else
        {
            lblLoggedInfoName.Text = String.Empty;
            lblLoggedInfoClass.Text = String.Empty;
            lblLoggedInfoClass2.Text = String.Empty;
            btnLogout.Visible = false;
        }
    }
    #endregion

    #region Common
    private CustomWebPage ContentPage
    {
        get
        {
            if (Page is CustomWebPage)
            {
                return Page as CustomWebPage;
            }
            else
            {
                throw new Exception("Page is not CustomWebPage");
            }
        }
    }

    protected void Logout_Click(object sender, EventArgs e)
    {
        ApplicationSession.Logout();
        CustomWebPage p = (CustomWebPage)Page;
        p.RedirectOut();
    }
    #endregion

    #region ChangePassword

    private void ShowChangePassword()
    {
        ContentPage.ShowBootstrapPopup(pnlChangePasswordModal.ClientID);
    }

    protected void ChangePassword_Click(object sender, EventArgs e)
    {
        lblChangePasswordError.Text = String.Empty;
        txtPassword.Text = String.Empty;
        txtPassword2.Text = String.Empty;

        ShowChangePassword();
    }

    protected void ChangePasswordSave_Click(object sender, EventArgs e)
    {
        bool error = false;

        if (txtPassword.Text == String.Empty)
        {
            error = true;
            lblChangePasswordError.Text = "Heslo nemůže být prázdné";
        }

        if (txtPassword.Text != txtPassword2.Text)
        {
            error = true;
            lblChangePasswordError.Text = "Zadaná hesla nejsou stejná";
        }

        if (error)
        {
            ShowChangePassword();
        }
        else
        {
            Character u = ApplicationSession.GetUser();
            u.Password = txtPassword.Text;
            ApplicationSession.SetUser(u);

            if (Page is CustomWebPage)
            {
                CustomWebPage p = Page as CustomWebPage;
                BusinessFacade<Character> f = new BusinessFacade<Character>(p.NHibernateSession);
                f.SaveEntity(u);
            }
        }
    }

    #endregion
}

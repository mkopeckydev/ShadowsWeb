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

public partial class Characters : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.IsAdmin) RedirectOut();

        if (!IsPostBack)
        {
            BusinessFacade<CharacterClass> fCharacterClass = new BusinessFacade<CharacterClass>(NHibernateSession);
            List<CharacterClass> chars = fCharacterClass.GetListEntities("Caption", true, String.Empty, 0, true);
            ddlIdCharacterClass.DataSource = chars;
            ddlIdCharacterClass.DataBind();
            ddlIdCharacterClass2.DataSource = chars;
            ddlIdCharacterClass2.DataBind();

            chbIsDeadFilter.Checked = true;

            ReloadData();
        }
    }

    private void ReloadData()
    {
        string filter = String.Empty;
        if (chbIsDeadFilter.Checked) filter = "(IsDead = 0)";

        BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
        List<Character> rows = fCharacter.GetListEntities("Name", true, filter);
        rptItems.DataSource = rows;
        rptItems.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        EditId = BaseEntity.NEW_OBJECT_ID;
        txtName.Text = String.Empty;
        txtUserName.Text = String.Empty;
        txtPassword.Text = String.Empty;
        ControlsTools.InitializeDDL(ddlIdCharacterClass);
        ControlsTools.InitializeDDL(ddlIdCharacterClass2);
        txtClientMessage.Text = String.Empty;
        txtIntelligenceBonus.Text = "0";
        chbAdministrator.Checked = false;
        chbIsDead.Checked = false;
        chbIsActive.Checked = false;

        detailPopup.Show();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);

        Character ch = new Character();
        if (EditId > BaseEntity.NEW_OBJECT_ID)
        {
            ch = fCharacter.GetEntity(EditId);
        }

        ch.Name = txtName.Text;
        ch.UserName = txtUserName.Text;
        ch.Password = txtPassword.Text;
        ch.ClientMessage = txtClientMessage.Text;

        int tmp = 0;
        Int32.TryParse(txtIntelligenceBonus.Text, out tmp);
        ch.IntelligenceBonus = tmp;

        ch.Administrator = chbAdministrator.Checked;
        ch.IsDead = chbIsDead.Checked;
        ch.IsActive = chbIsActive.Checked;

        ch.IdCharacterClass = Convert.ToInt32(ddlIdCharacterClass.SelectedValue);
        ch.CharacterClass = ddlIdCharacterClass.Text;
        ch.IdCharacterClass2 = Convert.ToInt32(ddlIdCharacterClass2.SelectedValue);
        ch.CharacterClass2 = ddlIdCharacterClass2.Text;

        fCharacter.SaveEntity(ch);

        ReloadData();
    }

    protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);
        BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
        Character ch = fCharacter.GetEntity(id);

        if (e.CommandName == "DeleteRow")
        {
            fCharacter.DeleteEntitiy(ch);
            ReloadData();
        }
        else if (e.CommandName == "EditRow")
        {
            EditId = ch.Id;

            txtName.Text = ch.Name;
            txtUserName.Text = ch.UserName;
            txtPassword.Text = ch.Password;
            txtIntelligenceBonus.Text = ch.IntelligenceBonus.ToString();
            chbAdministrator.Checked = ch.Administrator;
            chbIsDead.Checked = ch.IsDead;
            chbIsActive.Checked = ch.IsActive;
            txtClientMessage.Text = ch.ClientMessage;
            ControlsTools.SetDDL(ddlIdCharacterClass, ch.IdCharacterClass);
            ControlsTools.SetDDL(ddlIdCharacterClass2, ch.IdCharacterClass2);

            detailPopup.Show();
        }
    }

    protected void chbIsDeadFilter_CheckedChanged(object sender, EventArgs e)
    {
        ReloadData();
    }
}
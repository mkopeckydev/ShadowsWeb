using Shadows.Data;
using Shadows.Data.Facade;
using Shadows.Data.Model;
using Shadows.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : CustomWebPage
{
    private string SKILL = "SKILL";
    private string TEXT = "TEXT";

    private bool _tmpAdmin;
    private bool _tmpShowImage;

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.LoggedIn) RedirectOut();

        if (!IsPostBack)
        {
            txtSearchText.MaxLength = ItemFacade.MAX_SEARCH_TEXT_LENGTH;

            Character ch = ApplicationSession.GetUser();
            btnNew.Visible = ch.Administrator;

            BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
            List<CharacterSkillLevel> l = fCharacterSkillLevel.GetListEntities("Skill", true, String.Format("(IdCharacter = {0})", ApplicationSession.GetUserId()), 0, true);
            ddlSearchSkill.DataSource = l;
            ddlSearchSkill.DataBind();

            ControlsTools.SetDDL(ddlSearchSkill, ApplicationSession.GetValueInt(SKILL));
            txtSearchText.Text = ApplicationSession.GetValueString(TEXT);

            Reload();
        }
    }
    #endregion

    #region btnPrint_Click
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (ddlSearchSkill.SelectedIndex > -1)
        {
            int idSkill = Convert.ToInt32(ddlSearchSkill.SelectedValue);

            string skillName = String.Empty;
            string fileName = "dovednosti";
            if (idSkill > 0)
            {
                BusinessFacade<Skill> fSkill = new BusinessFacade<Skill>(NHibernateSession);
                Skill s = fSkill.GetEntity(idSkill);
                skillName = s.Caption;
                fileName += "-" + skillName.ToLower();
            }

            BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
            Character ch = fCharacter.GetEntity(ApplicationSession.GetUserId());
            
            MemoryStream ms = Print.GetReportItems(Reload(), ch, skillName, NHibernateSession, this);
            ReturnPdfFile(ms, fileName);
        }
    }
    #endregion

    #region Reload
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Reload();
    }

    private List<Item> Reload()
    {
        int idSkill = Convert.ToInt32(ddlSearchSkill.SelectedValue);
        string searchText = txtSearchText.Text.Trim();

        ApplicationSession.SetValue(idSkill, SKILL);
        ApplicationSession.SetValue(searchText, TEXT);

        Character character = ApplicationSession.GetUser();

        ItemFacade fItem = new ItemFacade(NHibernateSession);
        List<Item> list = fItem.Search(searchText, idSkill, character);

        _tmpAdmin = character.Administrator;
        _tmpShowImage = chbShowImages.Checked;

        rptItems.DataSource = list;
        rptItems.DataBind();

        rptItems.Visible = (list.Count > 0);
        lblRowsCountInfo.Text = String.Format("Počet položek: {0}", list.Count);

        return list;
    }
    #endregion

    #region RepeaterEvents
    protected void rptItems_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        LinkButton b = (LinkButton)e.Item.FindControl("btnEditRow");
        if (b != null) b.Visible = _tmpAdmin;
        b = (LinkButton)e.Item.FindControl("btnDeleteRow");
        if (b != null) b.Visible = _tmpAdmin;
    }

    protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "DeleteRow")
        {
            //smazani radku
            BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
            List<ItemRow> rows = fItemRow.GetListEntities("InfoLevel", true, String.Format("(IdItem = {0})", id));
            foreach(ItemRow r in rows)
            {
                fItemRow.DeleteEntitiy(r);
            }

            //smazani hlavicky
            BusinessFacade<Item> fItem = new BusinessFacade<Item>(NHibernateSession);
            Item i = fItem.GetEntity(id);
            fItem.DeleteEntitiy(i);

            Reload();
        }
        else if (e.CommandName == "ShowRow")
        {
            Response.Redirect(String.Format("~/Detail.aspx?id={0}", id), true);
        }
        else if (e.CommandName == "EditRow")
        {
            Response.Redirect(String.Format("~/Edit.aspx?id={0}", id), true);
        }
    }

    protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Item i = (Item)e.Item.DataItem;
        if (i != null)
        {
            Image image = (Image)e.Item.FindControl("imgListImage");
            image.ImageUrl = String.Format("~/{0}/{1}", FilesManager.IMAGES_PATH, i.ImageName);
            image.Visible = (_tmpShowImage && !String.IsNullOrEmpty(i.ImageName));
        }
    }
    #endregion

    #region Buttons
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Edit.aspx", true);
    }

    protected void btnValues_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Values.aspx", true);
    }

    protected void btnSkils_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CharacterSkillSetup.aspx", true);
    }

    protected void ddlSearchSkill_SelectedIndexChanged(object sender, EventArgs e)
    {
        Reload();
    }
    #endregion
}
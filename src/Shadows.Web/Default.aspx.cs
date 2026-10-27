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
    private const string SEARCH_SKILL = "Default.SearchSkill";
    private const string SEARCH_TEXT = "Default.SearchText";
    private const string PAGE_SIZE = "Default.PageSize";
    private const string PAGE_INDEX = "Default.PageIndex";

    private bool _tmpAdmin;
    private bool _tmpShowImage;

    private int CurrentPageIndex
    {
        get { return (int)(ViewState["CurrentPageIndex"] ?? 0); }
        set { ViewState["CurrentPageIndex"] = value; }
    }

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
            List<CharacterSkillLevel> l = fCharacterSkillLevel.GetListEntities("Skill", true, String.Format("(IdCharacter = {0})", ApplicationSession.GetUserId()), true);
            ddlSearchSkill.DataSource = l;
            ddlSearchSkill.DataBind();

            ControlsTools.SetDDL(ddlSearchSkill, ApplicationSession.GetValueInt(SEARCH_SKILL));
            txtSearchText.Text = ApplicationSession.GetValueString(SEARCH_TEXT);
            object savedPageSize = ApplicationSession.GetValue(PAGE_SIZE);
            if (savedPageSize != null && ddlPageSize.Items.FindByValue(savedPageSize.ToString()) != null)
                ddlPageSize.SelectedValue = savedPageSize.ToString();
            CurrentPageIndex = ApplicationSession.GetValueInt(PAGE_INDEX);

            Reload();
        }
    }
    #endregion

    #region Print_Click
    protected void Print_Click(object sender, EventArgs e)
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
    protected void Search_Click(object sender, EventArgs e)
    {
        if (sender != chbShowImages) CurrentPageIndex = 0;
        Reload();
    }

    protected void PreviousPage_Click(object sender, EventArgs e)
    {
        CurrentPageIndex = Math.Max(0, CurrentPageIndex - 1);
        Reload();
    }

    protected void NextPage_Click(object sender, EventArgs e)
    {
        CurrentPageIndex++;
        Reload();
    }

    protected void PageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        Reload();
    }

    private List<Item> Reload()
    {
        int idSkill = Convert.ToInt32(ddlSearchSkill.SelectedValue);
        string searchText = txtSearchText.Text.Trim();

        ApplicationSession.SetValue(idSkill, SEARCH_SKILL);
        ApplicationSession.SetValue(searchText, SEARCH_TEXT);

        Character character = ApplicationSession.GetUser();

        ItemFacade fItem = new ItemFacade(NHibernateSession);
        List<Item> list = fItem.Search(searchText, idSkill, character);

        _tmpAdmin = character.Administrator;
        _tmpShowImage = chbShowImages.Checked;

        PagedDataSource pagedItems = new PagedDataSource
        {
            DataSource = list
        };
        int pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
        pagedItems.AllowPaging = pageSize > 0;
        if (pagedItems.AllowPaging) pagedItems.PageSize = pageSize;
        CurrentPageIndex = Math.Min(CurrentPageIndex, Math.Max(0, pagedItems.PageCount - 1));
        pagedItems.CurrentPageIndex = CurrentPageIndex;
        ApplicationSession.SetValue(pageSize, PAGE_SIZE);
        ApplicationSession.SetValue(CurrentPageIndex, PAGE_INDEX);

        rptItems.DataSource = pagedItems;
        rptItems.DataBind();

        rptItems.Visible = (list.Count > 0);
        lblRowsCountInfo.Text = String.Format("Počet položek: {0}", list.Count);
        lblPageInfo.Text = String.Format("Stránka {0} z {1}", list.Count > 0 ? CurrentPageIndex + 1 : 0, list.Count > 0 ? pagedItems.PageCount : 0);
        btnPreviousPage.Enabled = list.Count > 0 && !pagedItems.IsFirstPage;
        btnNextPage.Enabled = list.Count > 0 && !pagedItems.IsLastPage;

        return list;
    }
    #endregion

    #region RepeaterEvents
    protected void Items_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        LinkButton b = (LinkButton)e.Item.FindControl("btnEditRow");
        if (b != null) b.Visible = _tmpAdmin;
        b = (LinkButton)e.Item.FindControl("btnDeleteRow");
        if (b != null) b.Visible = _tmpAdmin;
    }

    protected void Items_ItemCommand(object source, RepeaterCommandEventArgs e)
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

    protected void Items_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
    protected void New_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Edit.aspx", true);
    }

    protected void Values_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Values.aspx", true);
    }

    protected void Skils_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CharacterSkillSetup.aspx", true);
    }

    protected void SearchSkill_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        Reload();
    }
    #endregion
}
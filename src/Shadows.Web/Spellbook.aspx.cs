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

public partial class Spellbook : CustomWebPage
{
    private const string PAGE_SIZE = "Spellbook.PageSize";
    private const string PAGE_INDEX = "Spellbook.PageIndex";
    private const string SEARCH_TEXT = "Spellbook.SearchText";
    private const string SEARCH_LEVEL = "Spellbook.SearchLevel";

    private bool _tmpAdmin;

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
            txtSearchText.MaxLength = SpellbookItemFacade.MAX_SEARCH_TEXT_LENGTH;

            BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
            List<Character> characters = fCharacter.GetListEntities("Name", true, "(IsDead = 0)");
            chbVisibilityCharacters.DataSource = characters;
            chbVisibilityCharacters.DataBind();

            btnNew.Visible = ApplicationSession.IsAdmin;
            txtSearchText.Text = ApplicationSession.GetValueString(SEARCH_TEXT);
            txtSearchLevel.Text = ApplicationSession.GetValueString(SEARCH_LEVEL);
            object savedPageSize = ApplicationSession.GetValue(PAGE_SIZE);
            if (savedPageSize != null && ddlPageSize.Items.FindByValue(savedPageSize.ToString()) != null)
                ddlPageSize.SelectedValue = savedPageSize.ToString();
            CurrentPageIndex = ApplicationSession.GetValueInt(PAGE_INDEX);
            ReloadData();
        }
    }
    #endregion

    #region ReloadData
    private List<SpellbookItem> ReloadData()
    {
        _tmpAdmin = ApplicationSession.IsAdmin;

        SpellbookItemFacade fSpellbookItem = new SpellbookItemFacade(NHibernateSession);

        int searchLevel;
        Int32.TryParse(txtSearchLevel.Text, out searchLevel);   

        ApplicationSession.SetValue(txtSearchText.Text, SEARCH_TEXT);
        ApplicationSession.SetValue(txtSearchLevel.Text, SEARCH_LEVEL);
        List<SpellbookItem> spellbookItems = fSpellbookItem.Search(txtSearchText.Text, searchLevel, ApplicationSession.GetUser());

        PagedDataSource pagedItems = new PagedDataSource
        {
            DataSource = spellbookItems
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

        lblRowsCountInfo.Text = String.Format("Počet položek: {0}", spellbookItems.Count);
        lblPageInfo.Text = String.Format("Stránka {0} z {1}", spellbookItems.Count > 0 ? CurrentPageIndex + 1 : 0, spellbookItems.Count > 0 ? pagedItems.PageCount : 0);
        btnPreviousPage.Enabled = spellbookItems.Count > 0 && !pagedItems.IsFirstPage;
        btnNextPage.Enabled = spellbookItems.Count > 0 && !pagedItems.IsLastPage;

        return spellbookItems;
    }
    #endregion

    #region Items_ItemCommand
    protected void Items_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);

            BusinessFacade<SpellbookItem> fSpellbookItem = new BusinessFacade<SpellbookItem>(NHibernateSession);
            SpellbookItem item = fSpellbookItem.GetEntity(id);
            fSpellbookItem.DeleteEntitiy(item);
            ReloadData();
        }
        else if (e.CommandName == "ShowRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect(String.Format("~/SpellbookDetail.aspx?id={0}", id), true);
        }
        else if (e.CommandName == "EditRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect(String.Format("~/SpellbookEdit.aspx?id={0}", id), true);
        }
        else if (e.CommandName == "VisibilityRow")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            EditId = id;

            BusinessFacade<SpellbookItemVisibility> fSpellbookItemVisibility = new BusinessFacade<SpellbookItemVisibility>(NHibernateSession);
            List<SpellbookItemVisibility> visibility = fSpellbookItemVisibility.GetListEntities(String.Empty, true, String.Format("IdSpellbookItem = {0}", id));

            foreach (ListItem i in chbVisibilityCharacters.Items)
            {
                i.Selected = false;
                int idCharacter = Convert.ToInt32(i.Value);
                foreach (SpellbookItemVisibility v in visibility)
                {
                    if (v.IdCharacter == idCharacter)
                    {
                        i.Selected = true;
                        break;
                    }
                }
            }

            detailPopup.Show();
        }
    }
    #endregion

    #region Save_Click
    protected void Save_Click(object sender, EventArgs e)
    {
        int id = EditId;

        BusinessFacade<SpellbookItemVisibility> fSpellbookItemVisibility = new BusinessFacade<SpellbookItemVisibility>(NHibernateSession);
        List<SpellbookItemVisibility> visibility = fSpellbookItemVisibility.GetListEntities(String.Empty, true, String.Format("IdSpellbookItem = {0}", id));

        foreach (ListItem i in chbVisibilityCharacters.Items)
        {
            int idCharacter = Convert.ToInt32(i.Value);

            SpellbookItemVisibility fv = visibility.Find(x => x.IdCharacter == idCharacter);
            if (fv == null)
            {
                if (i.Selected)
                {
                    fv = new SpellbookItemVisibility
                    {
                        IdSpellbookItem = id,
                        IdCharacter = idCharacter,
                        Selected = i.Selected
                    };
                    visibility.Add(fv);
                }
            }
            else
            {
                fv.Selected = i.Selected;
            }
        }

        foreach (SpellbookItemVisibility fv in visibility)
        {
            if (fv.Selected)
            {
                fSpellbookItemVisibility.SaveEntity(fv);
            }
            else
            {
                fSpellbookItemVisibility.DeleteEntitiy(fv);
            }
        }

        ReloadData();
    }
    #endregion

    #region New_Click
    protected void New_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/SpellbookEdit.aspx", true);
    }
    #endregion

    #region Search_Click
    protected void Search_Click(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        ReloadData();
    }
    #endregion

    protected void PreviousPage_Click(object sender, EventArgs e)
    {
        CurrentPageIndex = Math.Max(0, CurrentPageIndex - 1);
        ReloadData();
    }

    protected void NextPage_Click(object sender, EventArgs e)
    {
        CurrentPageIndex++;
        ReloadData();
    }

    protected void PageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentPageIndex = 0;
        ReloadData();
    }

    #region Items_ItemDataBound
    protected void Items_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        LinkButton b = (LinkButton)e.Item.FindControl("btnEditRow");
        if (b != null) b.Visible = _tmpAdmin;
        b = (LinkButton)e.Item.FindControl("btnDeleteRow");
        if (b != null) b.Visible = _tmpAdmin;
        b = (LinkButton)e.Item.FindControl("btnVisibility");
        if (b != null) b.Visible = _tmpAdmin;
    }
    #endregion

    #region Print_Click
    protected void Print_Click(object sender, EventArgs e)
    {
        string fileName = "knihakouzel";

        BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
        Character ch = fCharacter.GetEntity(ApplicationSession.GetUserId());

        MemoryStream ms = Print.GetReportSpellBook(ReloadData(), ch, this, NHibernateSession);
        ReturnPdfFile(ms, fileName);
    }
    #endregion
}
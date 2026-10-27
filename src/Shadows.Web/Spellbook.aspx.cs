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
    private bool _tmpAdmin;

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

        List<SpellbookItem> spellbookItems = fSpellbookItem.Search(txtSearchText.Text, searchLevel, ApplicationSession.GetUser());

        rptItems.DataSource = spellbookItems;
        rptItems.DataBind();

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
        ReloadData();
    }
    #endregion

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
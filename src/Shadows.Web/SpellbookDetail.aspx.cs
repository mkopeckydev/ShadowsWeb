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

public partial class SpellbookDetail : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.LoggedIn) RedirectOut();

        if (!IsPostBack)
        {

            string id = Request.QueryString["id"];

            if (!String.IsNullOrEmpty(id))
            {
                EditId = Convert.ToInt32(id);
            }
            else
            {
                btnClose_Click(null, null);
            }

            if (EditId > 0)
            {
                BusinessFacade<SpellbookItem> fItem = new BusinessFacade<SpellbookItem>(NHibernateSession);
                SpellbookItem i = fItem.GetEntity(EditId);

                lblCaption.Text = i.Caption;
                lblAttributesValue.Text = i.AttributesHtml;
                lblDescriptionValue.Text = i.DescriptionHtml;
                lblModificationValue.Text = i.ModificationHtml;
                lblModificationRuneMageValue.Text = i.ModificationRuneMageHtml;


                //poznamka
                BusinessFacade<SpellbookItemNote> fItemSpellbookNote = new BusinessFacade<SpellbookItemNote>(NHibernateSession);
                List<SpellbookItemNote> notes = fItemSpellbookNote.GetListEntities(String.Empty, true, String.Format("(IdSpellbookItem = {0}) and (IdCharacter = {1})", EditId, ApplicationSession.GetUserId()));
                if (notes.Count > 0)
                {
                    txtSpellbookItemNote.Text = notes[0].Note;
                }
                else
                {
                    txtSpellbookItemNote.Text = String.Empty;
                }
            }
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Spellbook.aspx", true);
    }

    protected void btnSaveSpellbookItemNote_Click(object sender, EventArgs e)
    {
        lblSaveSpellbookItemNote.Text = "Uloženo<br />";

        BusinessFacade<SpellbookItemNote> fItemSpellbookNote = new BusinessFacade<SpellbookItemNote>(NHibernateSession);
        SpellbookItemNote itemNote = new SpellbookItemNote();

        List<SpellbookItemNote> notes = fItemSpellbookNote.GetListEntities(String.Empty, true, String.Format("(IdSpellbookItem = {0}) and (IdCharacter = {1})", EditId, ApplicationSession.GetUserId()));
        if (notes.Count > 0)
        {
            itemNote = notes[0];
        }
        else
        {
            itemNote.IdCharacter = ApplicationSession.GetUserId();
            itemNote.IdSpellbookItem = EditId;
        }

        itemNote.Note = txtSpellbookItemNote.Text;

        fItemSpellbookNote.SaveEntity(itemNote);
    }
}
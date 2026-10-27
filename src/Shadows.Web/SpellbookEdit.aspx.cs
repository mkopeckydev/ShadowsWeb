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

public partial class SpellbookEdit : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.IsAdmin) RedirectOut();

        if (!IsPostBack)
        {
            string id = Request.QueryString["id"];

            if (!String.IsNullOrEmpty(id))
            {
                EditId = Convert.ToInt32(id);
            }
            else
            {
                EditId = BaseEntity.NEW_OBJECT_ID;
            }

            if (EditId > 0)
            {
                lblCaption.Text = "Editace kouzla";
                //nacteni detailu
                BusinessFacade<SpellbookItem> fSpellbookItem = new BusinessFacade<SpellbookItem>(NHibernateSession);
                SpellbookItem i = fSpellbookItem.GetEntity(EditId);

                txtCaption.Text = i.Caption;
                txtSpellGroup.Text = i.SpellGroup;
                txtSpellLevel.Text = i.SpellLevel.ToString();
                txtAttributes.Text = i.Attributes;
                txtDescription.Text = i.Description;
                txtModification.Text = i.Modification;
                txtModificationRuneMage.Text = i.ModificationRuneMage;
            }
            else
            {
                lblCaption.Text = "Založení nového kouzla";
                txtCaption.Text = String.Empty;
                txtSpellGroup.Text = String.Empty;
                txtSpellLevel.Text = "1";
                txtAttributes.Text = String.Empty;
                txtDescription.Text = String.Empty;
                txtModification.Text = String.Empty;
                txtModificationRuneMage.Text = String.Empty;
            }
        }
    }

    protected void Close_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Spellbook.aspx", true);
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        BusinessFacade<SpellbookItem> fSpellbookItem = new BusinessFacade<SpellbookItem>(NHibernateSession);

        SpellbookItem i = new SpellbookItem();
        if (EditId > BaseEntity.NEW_OBJECT_ID)
        {
            i = fSpellbookItem.GetEntity(EditId);
        }

        i.Caption = txtCaption.Text;
        i.SpellGroup = txtSpellGroup.Text;
        
        int l;
        Int32.TryParse(txtSpellLevel.Text, out l);
        i.SpellLevel = l;

        i.Attributes = txtAttributes.Text;
        i.Description = txtDescription.Text;
        i.Modification = txtModification.Text;
        i.ModificationRuneMage = txtModificationRuneMage.Text;

        i.Attributes = i.Attributes.Replace((char)9, ' ');
        i.Description = i.Description.Replace((char)9, ' ');
        i.Modification = i.Modification.Replace((char)9, ' ');
        i.ModificationRuneMage = i.ModificationRuneMage.Replace((char)9, ' ');

        fSpellbookItem.SaveEntity(i);

        Response.Redirect("~/Spellbook.aspx", true);
    }
}
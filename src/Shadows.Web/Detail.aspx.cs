using Shadows.Data;
using Shadows.Data.CustomData;
using Shadows.Data.Facade;
using Shadows.Data.Model;
using Shadows.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Detail : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.LoggedIn) RedirectOut();
        
        lblSaveItemNote.Text = String.Empty;

        if (!IsPostBack)
        {
            pnlImage.Visible = false;

            Character character = ApplicationSession.GetUser();
            ItemFacade fItem = new ItemFacade(NHibernateSession);

            string id = Request.QueryString["id"];

            if (!String.IsNullOrEmpty(id))
            {
                EditId = Convert.ToInt32(id);
            }
            else
            {
                Close_Click(null, null);
            }

            if (EditId > 0)
            {
                Item i = fItem.GetEntity(EditId);

                GetVisibleItemRowsResult rowsData = fItem.GetVisibleItemRows(character, i);
                lblValueSum.Text = rowsData.ValueSum;
                lblBonusLevel.Text = rowsData.BonusLevel;

                //viditelnost, pokud ma byt viditelne
                if (rowsData.List.Count > 0) pnlImage.Visible = true;

                //naplneni panelu
                lblCaption.Text = i.Caption;
                if (String.IsNullOrEmpty(i.ImageName))
                {
                    pnlImage.Visible = false;
                }
                else
                {
                    pnlImage.Visible = true;
                    imgDetail.ImageUrl = String.Format("~/{0}/{1}", FilesManager.IMAGES_PATH, i.ImageName);
                    imgDetail.ToolTip = i.ImageName;
                }

                //radky
                rptRows.DataSource = rowsData.List;
                rptRows.DataBind();
                rptRows.Visible = rowsData.List.Count > 0;

                //poznamka
                BusinessFacade<ItemNote> fItemNote = new BusinessFacade<ItemNote>(NHibernateSession);
                List<ItemNote> notes = fItemNote.GetListEntities(String.Empty, true, String.Format("(IdItem = {0}) and (IdCharacter = {1})", EditId, ApplicationSession.GetUserId()));
                if (notes.Count > 0)
                {
                    txtItemNote.Text = notes[0].Note;
                }
                else
                {
                    txtItemNote.Text = String.Empty;
                }
            }
        }
    }

    protected void Close_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx", true);
    }

    protected void SaveItemNote_Click(object sender, EventArgs e)
    {
        lblSaveItemNote.Text = "Uloženo<br />";

        BusinessFacade<ItemNote> fItemNote = new BusinessFacade<ItemNote>(NHibernateSession);
        ItemNote itemNote = new ItemNote();

        List<ItemNote> notes = fItemNote.GetListEntities(String.Empty, true, String.Format("(IdItem = {0}) and (IdCharacter = {1})", EditId, ApplicationSession.GetUserId()));
        if (notes.Count > 0)
        {
            itemNote = notes[0];
        }
        else
        {
            itemNote.IdCharacter = ApplicationSession.GetUserId();
            itemNote.IdItem = EditId;
        }

        itemNote.Note = txtItemNote.Text;

        fItemNote.SaveEntity(itemNote);
    }
}

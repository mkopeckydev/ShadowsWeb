using Shadows.Data;
using Shadows.Data.Facade;
using Shadows.Data.Model;
using Shadows.Web;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Files : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.LoggedIn) RedirectOut();

        if (!IsPostBack)
        {
            BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
            List<Character> characters = fCharacter.GetListEntities("Name", true, "(IsDead = 0)");
            chbVisibilityCharacters.DataSource = characters;
            chbVisibilityCharacters.DataBind();

            pnlUpload.Visible = ApplicationSession.IsAdmin;
            ReloadData();
        }
    }

    private void ReloadData()
    {
        FilesManager fm = new FilesManager();
        List<FileItem> files = fm.GetFiles(ApplicationSession.IsAdmin, ApplicationSession.GetUserId(), NHibernateSession);
        rptItems.DataSource = files;
        rptItems.DataBind();
    }

    protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRow")
        {
            string fileName = Convert.ToString(e.CommandArgument);

            FilesManager fm = new FilesManager();
            fm.DeleteFile(fileName);
            ReloadData();
        }
        else if (e.CommandName == "VisibilityRow")
        {
            int idFileData = Convert.ToInt32(e.CommandArgument);
            EditId = idFileData;

            BusinessFacade<FileVisibility> fFileVisibility = new BusinessFacade<FileVisibility>(NHibernateSession);
            List<FileVisibility> visibility = fFileVisibility.GetListEntities(String.Empty, true, String.Format("IdFileData = {0}", idFileData));

            foreach (ListItem i in chbVisibilityCharacters.Items)
            {
                i.Selected = false;
                int idCharacter = Convert.ToInt32(i.Value);
                foreach (FileVisibility v in visibility)
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


    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (fuUpload.HasFile)
        {
            FilesManager fm = new FilesManager();
            fuUpload.SaveAs(fm.GetFilePath(fuUpload.FileName));

            ShowMessage(String.Format("Soubor {0} nahrán.", fuUpload.FileName));
        }
        ReloadData();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int idFileData = EditId;

        BusinessFacade<FileVisibility> fFileVisibility = new BusinessFacade<FileVisibility>(NHibernateSession);
        List<FileVisibility> visibility = fFileVisibility.GetListEntities(String.Empty, true, String.Format("IdFileData = {0}", idFileData));

        foreach (ListItem i in chbVisibilityCharacters.Items)
        {
            int idCharacter = Convert.ToInt32(i.Value);

            FileVisibility fv = visibility.Find(x => x.IdCharacter == idCharacter);
            if (fv == null)
            {
                if (i.Selected)
                {
                    fv = new FileVisibility();
                    fv.IdFileData = idFileData;
                    fv.IdCharacter = idCharacter;
                    fv.Selected = i.Selected;
                    visibility.Add(fv);
                }
            }
            else
            {
                fv.Selected = i.Selected;
            }

            
        }

        foreach (FileVisibility fv in visibility)
        {
            if (fv.Selected)
            {
                fFileVisibility.SaveEntity(fv);
            }
            else
            {
                fFileVisibility.DeleteEntitiy(fv);
            }
        }

        ReloadData();
    }
}
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Edit : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.IsAdmin) RedirectOut();

        if (!IsPostBack)
        {
            BusinessFacade<Skill> fSkill = new BusinessFacade<Skill>(NHibernateSession);
            List<Skill> skills = fSkill.GetListEntities("Caption", true);
            ddlSkill.DataSource = skills;
            ddlSkill.DataBind();

            BusinessFacade<CharacterClass> fCharacterClass = new BusinessFacade<CharacterClass>(NHibernateSession);
            List<CharacterClass> chars = fCharacterClass.GetListEntities("Caption", true, String.Empty, true);
            ddlIdCharacterClass.DataSource = chars;
            ddlIdCharacterClass.DataBind();

            BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
            List<Character> users = fCharacter.GetListEntities("Name", true);
            ddlIdCharacter.DataSource = users;
            ddlIdCharacter.DataBind();

            BusinessFacade<SkillLevel> fSkillLevel = new BusinessFacade<SkillLevel>(NHibernateSession);
            List<SkillLevel> skillLevels = fSkillLevel.GetListEntities("Id", true);
            ddlIdSkillLevel.DataSource = skillLevels;
            ddlIdSkillLevel.DataBind();


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
                lblCaption.Text = "Editace záznamu";
                //nacteni detailu
                BusinessFacade<Item> fItem = new BusinessFacade<Item>(NHibernateSession);
                Item i = fItem.GetEntity(EditId);

                ControlsTools.SetDDL(ddlSkill, i.IdSkill);
                txtCaption.Text = i.Caption;
                if (String.IsNullOrEmpty(i.ImageName))
                {
                    imgDetail.Visible = false;
                }
                else
                {
                    imgDetail.Visible = true;
                    imgDetail.ImageUrl = String.Format("~/{0}/{1}", FilesManager.IMAGES_PATH, i.ImageName);
                    imgDetail.ToolTip = i.ImageName;
                }

                BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
                List<ItemRow> rows = fItemRow.GetListEntities("InfoLevel", true, String.Format("(IdItem = {0})", i.Id));
                SetRows(rows);
                ReloadRows();

                upValues.Visible = true;
                ReloadValues();
            }
            else
            {
                lblCaption.Text = "Založení nového záznamu";
                //novy zaznam
                ControlsTools.InitializeDDL(ddlSkill);
                txtCaption.Text = String.Empty;
                imgDetail.Visible = false;

                SetRows(new List<ItemRow>());

                ReloadRows();

                upValues.Visible = false;
            }
        }
    }

    private List<ItemRow> GetRows()
    {
        object obj = ApplicationSession.GetValue("Edit_ItemRowList");
        if (obj != null)
        {
            return (List<ItemRow>)obj;
        }
        else
        {
            return new List<ItemRow>();
        }
    }
    private void SetRows(List<ItemRow> list)
    {
        ApplicationSession.SetValue(list, "Edit_ItemRowList");
    }

  


    private void ReloadRows()
    {
        List<ItemRow> l = GetRows();
        rptRows.DataSource = l;
        rptRows.Visible = (l.Count > 0);
        rptRows.DataBind();
    }

    private List<CharacterItemValue> ReloadValues()
    {
        BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);
        List<CharacterItemValue> values = fCharacterItemValue.GetListEntities("Character", true, String.Format("(IdItem = {0})", EditId));
        List<CharacterItemValue> valuesVisible = new List<CharacterItemValue>();
        foreach (CharacterItemValue v in values)
        {
            if (!v.IsDead) valuesVisible.Add(v);
        }

        rptValues.DataSource = valuesVisible;
        rptValues.Visible = (valuesVisible.Count > 0);
        rptValues.DataBind();

        return values;
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        BusinessFacade<Item> fItem = new BusinessFacade<Item>(NHibernateSession);
        BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
        BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);

        Item i = new Item();
        if (EditId > BaseEntity.NEW_OBJECT_ID)
        {
            i = fItem.GetEntity(EditId);
        }
        i.Caption = txtCaption.Text;
        i.IdSkill = Convert.ToInt32(ddlSkill.SelectedValue);

        var imageFile = SaveImage();

        if (!String.IsNullOrEmpty(imageFile))
        {
            i.ImageName = imageFile;
        }

        fItem.SaveEntity(i);

        List<ItemRow> rows = GetRows();

        foreach(ItemRow row in rows)
        {
            row.InfoText = row.InfoText.Replace((char)9, ' ');

            row.IdItem = i.Id;
            if (row.Delete)
            {
                fItemRow.DeleteEntitiy(row);
            }
            else
            {
                fItemRow.SaveEntity(row);
            }
        }

        Response.Redirect("~/Default.aspx", true);
    }

    protected void Close_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx", true);
    }

    #region SaveImage
    protected string SaveImage()
    {
        try
        {
            string fileName = String.Empty;

            if (fuImage.HasFile)
            {
                fileName = Tools.FixFileName(fuImage.FileName);
                FilesManager fm = new FilesManager();
                DirectoryInfo diImages = fm.GetImagesDir();

                string targetPath = Path.Combine(diImages.FullName, fileName);
                FileInfo fi = new FileInfo(targetPath);
                int inputSize = fuImage.FileBytes.Length;

                if ((fi.Exists) && (inputSize != fi.Length))
                {
                    int sufix = 0;
                    string newTargetPath;
                    while (true)
                    {
                        sufix++;
                        string newFileName = fi.Name.Replace(fi.Extension, "") + "_" + sufix.ToString() + fi.Extension;
                        newTargetPath = Path.Combine(diImages.FullName, newFileName);
                        FileInfo fiNew = new FileInfo(newTargetPath);
                        if ((fiNew.Exists) && (inputSize == fiNew.Length))
                        {
                            fileName = newFileName;
                            break;
                        }
                        if (!fiNew.Exists)
                        {
                            fuImage.SaveAs(newTargetPath);
                            fileName = newFileName;
                            break;
                        }
                        //pojistka proti zacykleni
                        if (sufix == 999999) break;
                    }
                }
                else if (!fi.Exists)
                {
                    fuImage.SaveAs(targetPath);
                }
            }

            return fileName;
        }
        catch
        {
            return String.Empty;
        }
    }
    #endregion

    #region Rows

    protected void Rows_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string guid = Convert.ToString(e.CommandArgument);

        BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
        List<ItemRow> list = GetRows();
        ItemRow i = fItemRow.FindEntityInList(guid, list);

        if (e.CommandName == "DeleteRow")
        {
            i.Delete = true;
            SetRows(list);
            ReloadRows();
        }
        else if (e.CommandName == "EditRow")
        {
            //nacteni detailu
            if (i != null)
            {
                EditDetailGuid = i.Guid;
                txtInfoLevel.Text = i.InfoLevel.ToString();
                txtInfoText.Text = i.InfoText;
                txtItemCaption.Text = i.InfoCaption;
                ControlsTools.SetDDL(ddlIdCharacterClass, i.IdCharacterClass);
                ControlsTools.SetDDL(ddlIdSkillLevel, i.IdSkillLevel);
            }
            detailPopup.Show();
        }
    }

    protected void Rows_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ItemRow row = (ItemRow)e.Item.DataItem;
        if (row != null)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trRow");
            tr.Visible = !row.Delete;
        }
    }

    protected void NewRow_Click(object sender, EventArgs e)
    {
        //novy radek
        EditDetailGuid = String.Empty;
        txtInfoLevel.Text = String.Empty;
        txtInfoText.Text = String.Empty;
        txtItemCaption.Text = String.Empty;
        ControlsTools.InitializeDDL(ddlIdCharacterClass);
        ControlsTools.InitializeDDL(ddlIdSkillLevel);
        detailPopup.Show();
    }

    protected void SaveRow_Click(object sender, EventArgs e)
    {
        BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
        List<ItemRow> list = GetRows();

        ItemRow i = new ItemRow();
        if (!String.IsNullOrEmpty(EditDetailGuid))
        {
            i = fItemRow.FindEntityInList(EditDetailGuid, list);
        }
        else
        {
            list.Add(i);
        }

        int tmp = 0;
        Int32.TryParse(txtInfoLevel.Text, out tmp);
        i.InfoLevel = tmp;
        i.InfoText = txtInfoText.Text;
        i.InfoCaption = txtItemCaption.Text;
        i.IdCharacterClass = Convert.ToInt32(ddlIdCharacterClass.SelectedValue);
        i.CharacterClass = ddlIdCharacterClass.SelectedItem.Text;
        i.IdSkillLevel = Convert.ToInt32(ddlIdSkillLevel.SelectedValue);
        i.SkillLevel = ddlIdSkillLevel.SelectedItem.Text;

        list = list.OrderBy(o => o.InfoLevel).ToList();

        SetRows(list);
        ReloadRows();
    }
    #endregion

    #region Values

    protected void NewValue_Click(object sender, EventArgs e)
    {
        EditDetailId = 0;
        txtCharacterValue.Text = String.Empty;
        txtCharacterBonus.Text = String.Empty;
        ControlsTools.InitializeDDL(ddlIdCharacter);
        detailPopupValues.Show();
    }

    protected void SaveValue_Click(object sender, EventArgs e)
    {
        BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);

        CharacterItemValue i = new CharacterItemValue();
        if (EditDetailId > 0)
        {
            i = fCharacterItemValue.GetEntity(EditDetailId);
        }

        int tmp;
        Int32.TryParse(txtCharacterValue.Text, out tmp);
        i.CharacterValue = tmp;

        Int32.TryParse(txtCharacterBonus.Text, out tmp);
        i.CharacterBonus = tmp;

        i.IdCharacter = Convert.ToInt32(ddlIdCharacter.SelectedValue);
        i.Character = ddlIdCharacter.SelectedItem.Text;
        i.IdItem = EditId;

        fCharacterItemValue.SaveEntity(i);

        ReloadValues();
    }

    protected void Values_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        CharacterItemValue row = (CharacterItemValue)e.Item.DataItem;
        if (row != null)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trRow");
            tr.Visible = !row.Delete;
        }
    }

    protected void Values_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);
        CharacterItemValue i = fCharacterItemValue.GetEntity(id);

        if (e.CommandName == "DeleteRow")
        {
            fCharacterItemValue.DeleteEntitiy(i);
            ReloadValues();
        }
        else if (e.CommandName == "EditRow")
        {
            //nacteni detailu
            EditDetailId = i.Id;
            txtCharacterValue.Text = i.CharacterValue.ToString();
            txtCharacterBonus.Text = i.CharacterBonus.ToString();
            ControlsTools.SetDDL(ddlIdCharacter, i.IdCharacter);
            detailPopupValues.Show();
        }
    }

    protected void GenerateValues_Click(object sender, EventArgs e)
    {
        //automaticke doplneni hodu kostkou pro vsechny ostatni postavy
        BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
        BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);

        List<Character> chars = fCharacter.GetListEntities("Name", true, "(IsDead = 0)");
        List<CharacterItemValue> values = ReloadValues();

        Dice dice = new Dice();

        foreach (Character ch in chars)
        {
            bool createValue = true;

            foreach(CharacterItemValue v in values)
            {
                if (v.IdCharacter == ch.Id)
                {
                    createValue = false;
                }
            }

            if (createValue)
            {
                var newVal = new CharacterItemValue
                {
                    IdCharacter = ch.Id,
                    IdItem = EditId,
                    CharacterValue = dice.Get3k6(),
                    CharacterBonus = 0,
                    Character = ch.Name,
                    IsDead = ch.IsDead
                };

                fCharacterItemValue.SaveEntity(newVal);
            }
        }

        ReloadValues();
    }
    #endregion
}
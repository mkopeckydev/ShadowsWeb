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

public partial class CharacterSkillSetup : CustomWebPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!ApplicationSession.IsAdmin) RedirectOut();

        if (!IsPostBack)
        {
            BusinessFacade<Character> fCharacter = new BusinessFacade<Character>(NHibernateSession);
            List<Character> chars = fCharacter.GetListEntities("Name", true, String.Empty);
            ddlIdCharacter.DataSource = chars;
            ddlIdCharacter.DataBind();

            BusinessFacade<Skill> fSkill = new BusinessFacade<Skill>(NHibernateSession);
            List<Skill> skils = fSkill.GetListEntities("Id", true, String.Empty);
            ddlIdSkill.DataSource = skils;
            ddlIdSkill.DataBind();

            BusinessFacade<SkillLevel> fSkillLevel = new BusinessFacade<SkillLevel>(NHibernateSession);
            List<SkillLevel> levels = fSkillLevel.GetListEntities("Id", true, String.Empty);
            ddlIdSkillLevel.DataSource = levels;
            ddlIdSkillLevel.DataBind();

            chbIsDeadFilter.Checked = true;

            ReloadData();
        }
    }

    private void ReloadData()
    {
        BusinessFacade<CharacterSkillLevel> fCharacter = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
        List<CharacterSkillLevel> rows = fCharacter.GetListEntities("CharacterName,Skill", true, String.Empty);

        if (chbIsDeadFilter.Checked)
        {
            List<CharacterSkillLevel> rowsVisible = new List<CharacterSkillLevel>();
            foreach (CharacterSkillLevel r in rows)
            {
                if (!r.CharacterIsDead) rowsVisible.Add(r);
            }

            rptItems.DataSource = rowsVisible;
            rptItems.DataBind();
        }
        else
        {
            rptItems.DataSource = rows;
            rptItems.DataBind();
        }
    }

    protected void New_Click(object sender, EventArgs e)
    {
        EditId = BaseEntity.NEW_OBJECT_ID;
        ControlsTools.InitializeDDL(ddlIdCharacter);
        ControlsTools.InitializeDDL(ddlIdSkill);
        ControlsTools.InitializeDDL(ddlIdSkillLevel);

        detailPopup.Show();
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
        CharacterSkillLevel ch = new CharacterSkillLevel();
        if (EditId > BaseEntity.NEW_OBJECT_ID)
        {
            ch = fCharacterSkillLevel.GetEntity(EditId);
        }

        ch.IdCharacter = Convert.ToInt32(ddlIdCharacter.SelectedValue);
        ch.CharacterName = ddlIdCharacter.Text;

        ch.IdSkill = Convert.ToInt32(ddlIdSkill.SelectedValue);
        ch.Skill = ddlIdSkill.Text;

        ch.IdSkillLevel = Convert.ToInt32(ddlIdSkillLevel.SelectedValue);
        ch.SkillLevel = ddlIdSkillLevel.Text;

        fCharacterSkillLevel.SaveEntity(ch);
        ReloadData();
    }

    protected void Items_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);
        BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
        CharacterSkillLevel ch = fCharacterSkillLevel.GetEntity(id);

        if (e.CommandName == "DeleteRow")
        {
            fCharacterSkillLevel.DeleteEntitiy(ch);
            ReloadData();
        }
        else if (e.CommandName == "EditRow")
        {
            EditId = ch.Id;

            ControlsTools.SetDDL(ddlIdCharacter, ch.IdCharacter);
            ControlsTools.SetDDL(ddlIdSkill, ch.IdSkill);
            ControlsTools.SetDDL(ddlIdSkillLevel, ch.IdSkillLevel);

            detailPopup.Show();
        }
    }

    protected void GenerateAll_Click(object sender, EventArgs e)
    {
        //pro vsechny dovednosti postav se vygeneruje
        BusinessFacade<CharacterSkillLevel> fCharacter = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
        BusinessFacade<Item> fItem = new BusinessFacade<Item>(NHibernateSession);
        BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);

        Dice dice = new Dice();

        List<CharacterSkillLevel> rows = fCharacter.GetListEntities(String.Empty, true, String.Empty);

        int count = 0;
        foreach(CharacterSkillLevel skill in rows)
        {
            if (skill.CharacterIsDead) continue;
            
            List<Item> items = fItem.GetListEntities("Caption", true, String.Format("(IdSkill = {0})", skill.IdSkill));

            foreach(Item item in items)
            {
                List<CharacterItemValue> listchiv = fCharacterItemValue.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0}) AND (IdItem = {1})", skill.IdCharacter, item.Id));
                if (listchiv.Count == 0)
                {
                    //zaznam neexistuje, musi se vygenerovat

                    CharacterItemValue newVal = new CharacterItemValue
                    {
                        IdCharacter = skill.IdCharacter,
                        IdItem = item.Id,
                        CharacterValue = dice.Get3k6(),
                        CharacterBonus = 0,
                        Character = skill.CharacterName,
                        IsDead = skill.CharacterIsDead
                    };

                    fCharacterItemValue.SaveEntity(newVal);
                    count++;
                }
            }
        }

        ShowMessage(String.Format("Doplněno {0} hodů kostkou.", count));
    }

    protected void chbIsDeadFilter_CheckedChanged(object sender, EventArgs e)
    {
        ReloadData();
    }
}
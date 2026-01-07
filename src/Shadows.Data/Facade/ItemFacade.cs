using Shadows.Data.CustomData;
using Shadows.Data.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Facade
{
    public class ItemFacade : BusinessFacade<Item>
    {
        private const char SearchBlockSeparator = '"';
        private const char SearchSpace = ' ';

        public ItemFacade(ISession nhibernateSession) : base(nhibernateSession) { }

        public List<Item> Search(string searchText, int idSkill, Character character)
        {
            string searchSql = String.Empty;
            List<Item> finalList = new List<Item>();

            if (!String.IsNullOrEmpty(searchText))
            {
                if (searchText.Length > MAX_SEARCH_TEXT_LENGTH)
                {
                    return finalList;
                }

                List<string> searchTextItem = new List<string>();
                char[] chars = searchText.ToCharArray();
                bool blockStarted = false;
                string currentItem = String.Empty;

                foreach (char c in chars)
                {
                    if (blockStarted)
                    {
                        if (c == SearchBlockSeparator)
                        {
                            if (!String.IsNullOrEmpty(currentItem)) searchTextItem.Add(currentItem);

                            blockStarted = false;
                            currentItem = String.Empty;
                        }
                        else
                        {
                            currentItem += c;
                        }
                    }
                    else
                    {
                        if (c == SearchBlockSeparator)
                        {
                            if (!String.IsNullOrEmpty(currentItem)) searchTextItem.Add(currentItem);

                            blockStarted = true;
                            currentItem = String.Empty;
                        }
                        else if (c == SearchSpace)
                        {
                            if (!String.IsNullOrEmpty(currentItem)) searchTextItem.Add(currentItem);
                            currentItem = String.Empty;
                        }
                        else
                        {
                            currentItem += c;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(currentItem)) searchTextItem.Add(currentItem);
                currentItem = String.Empty;

                string sql1 = String.Empty;
                string sql2 = String.Empty;

                foreach (string s in searchTextItem)
                {
                    if (!String.IsNullOrEmpty(sql1)) sql1 += " and ";
                    sql1 += String.Format("(cast(Caption as varchar(max)) COLLATE SQL_Latin1_General_CP1251_CI_AS like '%{0}%' COLLATE SQL_Latin1_General_CP1251_CI_AS)", s);

                    if (!String.IsNullOrEmpty(sql2)) sql2 += " and ";
                    sql2 += String.Format("(Id in (select r.IdItem from ItemRow r where cast(r.InfoText as varchar(max)) COLLATE SQL_Latin1_General_CP1251_CI_AS like '%{0}%' COLLATE SQL_Latin1_General_CP1251_CI_AS))", s);
                }

                searchSql = String.Format("(({0}) or ({1}))", sql1, sql2);
            }

            if (idSkill > 0)
            {
                if (!String.IsNullOrEmpty(searchSql)) searchSql += " and ";
                searchSql += String.Format("(IdSkill = {0})", idSkill);
            }

            //prenacteni hodnot pro rychlost
            BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
            List<ItemRow> allRows = fItemRow.GetListEntities("InfoLevel", true);

            BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
            List<CharacterSkillLevel> charSkillLevels = fCharacterSkillLevel.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0})", character.Id));

            BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);
            List<CharacterItemValue> charItemValues = fCharacterItemValue.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0})", character.Id));

            //polozky
            List<Item> allItems = GetListEntities("Caption", true, searchSql);

            foreach (Item item in allItems)
            {
                //odfiltrovani polozek, o kterych postava nic nezna
                if (character.Administrator)
                {
                    finalList.Add(item);
                }
                else
                {
                    GetVisibleItemRowsResult d = GetVisibleItemRows(character, item, allRows, charSkillLevels, charItemValues);
                    if (d.List.Count > 0)
                    {
                        finalList.Add(item);
                    }
                    item.ListItemsCount = d.List.Count;
                }
            }

            return finalList;
        }

        public GetVisibleItemRowsResult GetVisibleItemRows(Character character, Item item)
        {
            BusinessFacade<ItemRow> fItemRow = new BusinessFacade<ItemRow>(NHibernateSession);
            List<ItemRow> allRows = fItemRow.GetListEntities("InfoLevel", true, String.Format("(IdItem = {0})", item.Id));

            BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
            List<CharacterSkillLevel> charSkillLevels = fCharacterSkillLevel.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0}) AND (IdSkill = {1})", character.Id, item.IdSkill));

            BusinessFacade<CharacterItemValue> fCharacterItemValue = new BusinessFacade<CharacterItemValue>(NHibernateSession);
            List<CharacterItemValue> charItemValues = fCharacterItemValue.GetListEntities(String.Empty, true, String.Format("(IdCharacter = {0}) AND (IdItem = {1})", character.Id, item.Id));

            return GetVisibleItemRows(character, item, allRows, charSkillLevels, charItemValues);
        }

        public GetVisibleItemRowsResult GetVisibleItemRows(Character character, Item item, List<ItemRow> allRows, List<CharacterSkillLevel> charSkillLevels, List<CharacterItemValue> charItemValues)
        {
            GetVisibleItemRowsResult result = new GetVisibleItemRowsResult();
            result.List = new List<ItemRow>();

            List<ItemRow> rows = allRows.FindAll(x => x.IdItem == item.Id);
            rows = rows.OrderBy(o => o.InfoLevel).ToList();

            CharacterSkillLevel chsl = charSkillLevels.Find(x => x.IdSkill == item.IdSkill);
            if (chsl == null) return result;

            CharacterItemValue chiv = charItemValues.Find(x => x.IdItem == item.Id);
            if (chiv == null) chiv = new CharacterItemValue();

            //celkovy soucet
            int totalSum = 0;
            string valuesCaption = String.Empty;

            valuesCaption += Tools.FormatBonus(chsl.SkillLevelBonus, String.Format("{0}, {1}", chsl.Skill, chsl.SkillLevel));
            totalSum += chsl.SkillLevelBonus;

            valuesCaption += Tools.FormatBonus(character.IntelligenceBonus, "Bonus na inteligenci / 2");
            totalSum += character.IntelligenceBonus;

            valuesCaption += Tools.FormatBonus(chiv.CharacterValue, "Hod kostkou");
            totalSum += chiv.CharacterValue;

            valuesCaption += Tools.FormatBonus(chiv.CharacterBonus, "Bonus");
            totalSum += chiv.CharacterBonus;


            valuesCaption += " = " + totalSum.ToString();
            result.ValueSum = valuesCaption;


            //bonusovy level
            if (chsl.BonusLevel != 0)
            {
                //zvednuti celkoveho souctu o prislusnou uroven

                int usedLevel = 0;

                foreach (ItemRow r in rows)
                {
                    if (usedLevel == chsl.BonusLevel) break;

                    if (r.InfoLevel > totalSum)
                    {
                        totalSum = r.InfoLevel;
                        usedLevel++;
                    }
                }

                result.BonusLevel = String.Format("<br />Bonusová úroveň: +{0}, celkem {1}", chsl.BonusLevel, totalSum);
            }
            else
            {
                result.BonusLevel = String.Empty;
            }


            //nalezeni povolenych polozek
            int maxInfoLevel = 0;
            foreach (ItemRow r in rows)
            {
                List<int> allowedIdCharacter = new List<int>();
                allowedIdCharacter.Add(0);
                allowedIdCharacter.Add(character.IdCharacterClass);
                allowedIdCharacter.Add(character.IdCharacterClass2);

                if ((character.IdCharacterClass == 7) || (character.IdCharacterClass2 == 7))
                {
                    allowedIdCharacter.Add(6);
                }

                if ((character.IdCharacterClass == 8) || (character.IdCharacterClass2 == 8))
                {
                    allowedIdCharacter.Add(6);
                    allowedIdCharacter.Add(7);
                }

                if ((totalSum >= r.InfoLevel) && (chsl.IdSkillLevel >= r.IdSkillLevel) && (allowedIdCharacter.Contains(r.IdCharacterClass)))
                {
                    result.List.Add(r);
                }
                if (r.InfoLevel > maxInfoLevel) maxInfoLevel = r.InfoLevel;
            }

            //pokud vidi vse az po nejvyssi uroven a umi dokonale, mistrne, vloyi se vse, bey filtru na povolani
            if (((chsl.IdSkillLevel == 6) || (chsl.IdSkillLevel == 7)) && (totalSum >= maxInfoLevel))
            {
                result.List = rows;
            }

            result.List = result.List.OrderBy(o => o.InfoLevel).ToList();

            return result;
        }
    }
}

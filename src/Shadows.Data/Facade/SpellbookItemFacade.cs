using Shadows.Data.Model;
using NHibernate;
using System;
using System.Collections.Generic;

namespace Shadows.Data.Facade
{
    public class SpellbookItemFacade : BusinessFacade<SpellbookItem>
    {
        public SpellbookItemFacade(ISession nhibernateSession) : base(nhibernateSession) { }

        public List<SpellbookItem> Search(string searchText, int level, Character character)
        {
            string filter = String.Empty;

            if (!character.Administrator)
            {
                Tools.AddToFilter(ref filter, String.Format("(Id in (select IdSpellbookItem from SpellbookItemVisibility where IdCharacter = {0}))", character.Id));
            }

            if (!String.IsNullOrEmpty(searchText))
            {
                if (searchText.Length > MAX_SEARCH_TEXT_LENGTH)
                {
                    return new List<SpellbookItem>();
                }

                Tools.AddToFilter(ref filter, String.Format("((Caption like '%{0}%') OR (SpellGroup like '%{0}%') OR (Attributes like '%{0}%') OR (Description like '%{0}%') OR (Modification like '%{0}%') OR (ModificationRuneMage like '%{0}%'))", searchText));
            }

            if (level > 0)
            {
                Tools.AddToFilter(ref filter, String.Format("(SpellLevel = {0})", level));
            }
            
            List<SpellbookItem> spellbookItems = GetListEntities("Caption,SpellGroup,SpellLevel", true, filter);

            return spellbookItems;
        }
    }
}

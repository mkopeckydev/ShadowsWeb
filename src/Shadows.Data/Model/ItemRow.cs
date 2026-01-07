using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    [Serializable]
    public class ItemRow : BaseEntity
    {
        public int IdItem { get; set; }
        public int IdCharacterClass { get; set; }
        public int IdSkillLevel { get; set; }
        public int InfoLevel { get; set; }
        public string InfoCaption { get; set; }
        public string InfoText { get; set; }
        public string CharacterClass { get; set; }
        public string SkillLevel { get; set; }

        public string InfoTextHtml
        {
            get
            {
                string s = InfoText;
                s = s.Replace("\n", "<br />");

                if (!String.IsNullOrEmpty(InfoCaption))
                {
                    s = String.Format("<b>{0}</b><br />", InfoCaption) + s;
                }

                return s;
            }
        }
    }
}

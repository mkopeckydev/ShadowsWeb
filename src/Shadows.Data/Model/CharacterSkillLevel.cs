using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    public class CharacterSkillLevel : BaseEntity
    {
        public int IdCharacter { get; set; }
        public int IdSkill { get; set; }
        public int IdSkillLevel { get; set; }
        public int BonusLevel { get; set; }

        public string CharacterName { get; set; }
        public string Skill { get; set; }
        public string SkillLevel { get; set; }
        public int SkillLevelBonus { get; set; }

        public string DisplayCaption
        {
            get
            {
                String s = String.Empty;
                if (BonusLevel > 0) s = String.Format(" +{0}", BonusLevel);

                return String.Format("{0} - {1}{2}", Skill, SkillLevel, s);
            }
        }
    }
}

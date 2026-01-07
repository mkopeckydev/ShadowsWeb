using System.Collections.Generic;

namespace Shadows.Data.Model
{
    public class FightNumber : BaseEntity
    {
        public FightNumber()
        {
            Bonuses = new List<FightNumberBonus>();
        }

        public int IdCharacter { get; set; }
        public string Caption { get; set; }

        public string BaseValueCaption1 { get; set; }
        public int BaseValue1 { get; set; }
        public string BaseValueCaption2 { get; set; }
        public int BaseValue2 { get; set; }
        public string BaseValueCaption3 { get; set; }
        public int BaseValue3 { get; set; }
        public string BaseValueCaption4 { get; set; }
        public int BaseValue4 { get; set; }
        public string BaseValueCaption5 { get; set; }
        public int BaseValue5 { get; set; }

        public int Ordering { get; set; }

        public List<FightNumberBonus> Bonuses { get; set; }
    }
}

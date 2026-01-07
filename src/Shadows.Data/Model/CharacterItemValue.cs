using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    [Serializable]
    public class CharacterItemValue : BaseEntity
    {
        public int IdCharacter { get; set; }
        public int IdItem { get; set; }
        public int CharacterValue { get; set; }
        public int CharacterBonus { get; set; }
        
        public string Character { get; set; }
        public bool IsDead { get; set; }
    }
}

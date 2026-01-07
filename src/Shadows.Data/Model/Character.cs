using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    [Serializable]
    public class Character : BaseEntity
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IdCharacterClass { get; set; }
        public int IdCharacterClass2 { get; set; }
        public bool Administrator { get; set; }
        public string CharacterClass { get; set; }
        public string CharacterClass2 { get; set; }
        public int IntelligenceBonus { get; set; }
        public bool IsDead { get; set; }
        public bool IsActive { get; set; }
        public string ClientMessage { get; set; }

        public string FullName
        {
            get
            {
                string s = Name;
                if (!String.IsNullOrEmpty(CharacterClass)) s += ", " + CharacterClass;
                if (!String.IsNullOrEmpty(CharacterClass2)) s += ", " + CharacterClass2;
                return s;
            }
        }

        public string IsActiveLabel
        {
            get
            {
                return (IsActive ? "Ano" : String.Empty);
            }
        }

        public bool IsDemoUser
        {
            get
            {
                return UserName.ToLower().Equals("demo");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class SpellbookItemNote : BaseEntity
    {
        public int IdSpellbookItem { get; set; }
        public int IdCharacter { get; set; }
        public string Note { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class SpellbookItemVisibility : BaseEntity
    {
        public int IdSpellbookItem { get; set; }
        public int IdCharacter { get; set; }
    }
}

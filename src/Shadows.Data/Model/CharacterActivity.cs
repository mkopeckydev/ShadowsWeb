using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class CharacterActivity : BaseEntity
    {
        public int IdCharacter { get; set; }
        public string Caption { get; set; }
        public string Color { get; set; }
        public int CheckCount { get; set; }
        public bool Checked1 { get; set; }
        public bool Checked2 { get; set; }
        public bool Checked3 { get; set; }
        public bool Checked4 { get; set; }
        public bool Checked5 { get; set; }
    }
}

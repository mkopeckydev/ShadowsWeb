using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class FightNumberBonus : BaseEntity
    {
        public int IdFightNumber { get; set; }
        public string Caption { get; set; }
        public int Bonus1 { get; set; }
        public int Bonus2 { get; set; }
        public int Bonus3 { get; set; }
        public int Bonus4 { get; set; }
        public int Bonus5 { get; set; }

        public bool IsSelected { get; set; }
    }
}

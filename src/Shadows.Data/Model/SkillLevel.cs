using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    public class SkillLevel : BaseEntity
    {
        public string Caption { get; set; }
        public int Bonus { get; set; }
    }
}

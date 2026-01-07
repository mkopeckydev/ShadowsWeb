using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class CustomCounter : BaseEntity
    {
        public int IdCharacter { get; set; }
        public string Caption { get; set; }
        public int ActualValue { get; set; }
        public int MaxValue { get; set; }
        public string Color { get; set; }
        public int Ordering { get; set; }
    }
}

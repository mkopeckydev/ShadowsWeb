using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    public class ItemNote : BaseEntity
    {
        public int IdItem { get; set; }
        public int IdCharacter { get; set; }
        public string Note { get; set; }
    }
}

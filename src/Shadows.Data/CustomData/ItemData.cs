using Shadows.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.CustomData
{
    public class ItemData
    {
        public Item Item { get; set; }
        public List<ItemRow> List { get; set; }
        public string ValueSum { get; set; }
        public string BonusLevel { get; set; }
        public string ItemNote { get; set; }
        public byte[] ImageData { get; set; }
    }
}

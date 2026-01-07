using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    public class Item : BaseEntity
    {
        public int IdSkill { get; set; }
        public string ImageName { get; set; }
        public string Caption { get; set; }

        public string SkillName { get; set; }

        public bool ListVisibility { get; set; }
        public int ListItemsCount { get; set; }

        public string ShowDetailButtonLabel
        {
            get
            {
                string s = "Zobrazit";

                if (ListItemsCount > 0)
                {
                    s += String.Format("[{0}]", ListItemsCount);
                }

                return s;
            }
        }
            
    }
}

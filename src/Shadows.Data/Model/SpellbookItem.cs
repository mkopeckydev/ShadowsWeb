using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class SpellbookItem : BaseEntity
    {
        public string Caption { get; set; }
        public string SpellGroup { get; set; }
        public int SpellLevel { get; set; }
        public string Attributes { get; set; }
        public string Description { get; set; }
        public string Modification { get; set; }
        public string ModificationRuneMage { get; set; }

        public int VisibilityCount { get; set; }

        public string VisibilityLabel
        {
            get
            {
                if (VisibilityCount > 0)
                {
                    return String.Format("Viditelnost ({0})", VisibilityCount);
                }
                else
                {
                    return "Viditelnost";
                }
            }
        }

        public string AttributesHtml
        {
            get
            {
                string s = Attributes;
                if (String.IsNullOrEmpty(s)) s = String.Empty;
                s = s.Replace("\n", "<br />");
                s = String.Format("Skupina kouzel: {0}<br />Stupeň náročnosti: {1}<br />{2}", SpellGroup, SpellLevel, s);
                return s;
            }
        }

        public string DescriptionHtml
        {
            get
            {
                string s = Description;
                if (String.IsNullOrEmpty(s)) s = String.Empty;
                s = s.Replace("\n", "<br />");
                return s;
            }
        }

        public string ModificationHtml
        {
            get
            {
                string s = Modification;
                if (String.IsNullOrEmpty(s)) s = String.Empty;
                s = s.Replace("\n", "<br />");
                return s;
            }
        }

        public string ModificationRuneMageHtml
        {
            get
            {
                string s = ModificationRuneMage;
                if (String.IsNullOrEmpty(s)) s = String.Empty;
                s = s.Replace("\n", "<br />");
                return s;
            }
        }
    }
}

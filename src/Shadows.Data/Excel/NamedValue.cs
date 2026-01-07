using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Excel
{
    [Serializable]
    public class NamedValue
    {
        public string Name { get; set; }
        public string SheetName { get; set; }

        public int StartCellRow { get; set; }
        public int StartCellColumn { get; set; }

        public int EndCellRow { get; set; }
        public int EndCellColumn { get; set; }

        public List<object> Data;

        public NamedValue()
        {
            Data = new List<object>();
        }

        public int Width
        {
            get
            {
                return (EndCellColumn - StartCellColumn) + 1;
            }
        }

        public int Height
        {
            get
            {
                return (EndCellRow - StartCellRow) + 1;
            }
        }

        public override string ToString()
        {
            string vals = String.Empty;
            foreach(object o in Data)
            {
                if (!String.IsNullOrEmpty(vals)) vals += ", ";
                vals += Convert.ToString(o);
            }

            return String.Format("<b>{0} [{1}, {2}x{3}]</b> Hodnoty: {4}", Name, SheetName, Width, Height, vals);
        }
    }
}

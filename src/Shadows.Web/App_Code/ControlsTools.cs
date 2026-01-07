using Shadows.Data.CustomData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Shadows.Web
{
    public class ControlsTools
    {
        private ControlsTools()
        { }

        public const string COLOR_RED = "R";
        public const string COLOR_GREEN = "G";
        public const string COLOR_BLUE = "B";
        public const string COLOR_YELLOW = "Y";
        public const string COLOR_BLACK = "L";
        public const string COLOR_ORANGE = "O";
        public const string COLOR_BROWN = "H";
        public const string COLOR_PURPLE = "P";
        public const string COLOR_GRAY = "A";

        public static List<ListObject> GetColors()
        {
            List<ListObject> l = new List<ListObject>();
            l.Add(new ListObject(COLOR_YELLOW, "Žlutá"));
            l.Add(new ListObject(COLOR_ORANGE, "Oranžová"));
            l.Add(new ListObject(COLOR_RED, "Červená"));
            l.Add(new ListObject(COLOR_PURPLE, "Fialová"));
            l.Add(new ListObject(COLOR_BLUE, "Modrá"));
            l.Add(new ListObject(COLOR_GREEN, "Zelená"));
            l.Add(new ListObject(COLOR_BROWN, "Hnědá"));
            l.Add(new ListObject(COLOR_GRAY, "Šedá"));
            l.Add(new ListObject(COLOR_BLACK, "Černá"));
            return l;
        }

        public static Color GetColor(string code)
        {
            if (code == COLOR_YELLOW) return Color.Yellow;
            if (code == COLOR_ORANGE) return Color.Orange;
            if (code == COLOR_RED) return Color.Red;
            if (code == COLOR_PURPLE) return Color.Purple;
            if (code == COLOR_BLUE) return Color.Blue;
            if (code == COLOR_GREEN) return Color.Green;
            if (code == COLOR_BROWN) return Color.Brown;
            if (code == COLOR_GRAY) return Color.Gray;
            if (code == COLOR_BLACK) return Color.Black;

            return Color.White;
        }

        public static void SetDDL(DropDownList ddl, int v)
        {
            SetDDL(ddl, v.ToString());
        }

        public static void SetDDL(DropDownList ddl, string v)
        {
            InitializeDDL(ddl);
            if (String.IsNullOrEmpty(v)) return;
            try
            {
                ddl.SelectedValue = v;
            }
            catch
            { }
        }

        public static void InitializeDDL(DropDownList ddl)
        {
            if (ddl.Items.Count > 0)
            {
                ddl.SelectedIndex = 0;
            }
            else
            {
                ddl.SelectedIndex = -1;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data
{
    public class Tools
    {
        private Tools()
        { }

        public static string FormatBonus(int v, string caption)
        {
            string mark = "+";
            if (v < 0) mark = "-";


            string s = String.Format(" {0} {1} ({2})", mark, Math.Abs(v), caption);

            return s;
        }

        public static string FixFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return String.Empty;
            string result = fileName;
            result = result.Replace(" ", "_");
            result = result.Replace(",", "_");
            result = result.Replace("'", "");
            result = result.Replace("´", "");
            result = result.Replace("’", "");
            result = result.Replace("″", "");
            result = result.Replace("\"", "");
            result = result.Replace("\\", "");
            result = result.Replace("/", "");
            result = result.Replace("&", "");
            result = result.Replace("*", "");
            result = result.Replace("+", "");
            result = result.Replace("#", "");
            result = result.Replace("°", "");
            result = result.Replace("%", "");
            result = result.Replace("(", "");
            result = result.Replace(")", "");
            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("®", "");
            result = result.Replace("™", "");
            result = result.Replace("¾", "");
            result = result.Replace("€", "");
            result = result.Replace("ø", "");
            result = result.ToLower();
            result = RemoveDiacritics(result);
            return result;
        }

        public static string RemoveDiacritics(string s)
        {
            s = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(s[i]) != UnicodeCategory.NonSpacingMark) sb.Append(s[i]);
            }
            return sb.ToString();
        }

        public static void AddToFilter(ref string filter, string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += s;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.CustomData
{
    [Serializable]
    public class ListObject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Caption { get; set; }

        public ListObject(string code, string caption)
        {
            Code = code;
            Caption = caption;

            if (String.IsNullOrEmpty(Caption)) Caption = String.Empty;
        }

        public ListObject(int id, string caption)
        {
            Id = id;
            Code = String.Empty;
            Caption = caption;

            if (String.IsNullOrEmpty(Caption)) Caption = String.Empty;
        }
    }
}

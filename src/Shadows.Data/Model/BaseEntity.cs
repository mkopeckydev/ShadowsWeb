using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    [Serializable]
    public class BaseEntity
    {
        public const int NEW_OBJECT_ID = 0;

        public int Id { get; set; }
        public bool Delete { get; set; }
        public bool Selected { get; set; }

        private string _guid;
        public string Guid
        {
            get
            {
                return _guid;
            }
        }

        public BaseEntity()
        {
            this._guid = System.Guid.NewGuid().ToString();
            Delete = false;
        }
    }
}

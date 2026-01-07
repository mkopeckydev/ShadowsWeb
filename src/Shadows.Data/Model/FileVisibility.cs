using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class FileVisibility : BaseEntity
    {
        public int IdFileData { get; set; }
        public int IdCharacter { get; set; }
    }
}

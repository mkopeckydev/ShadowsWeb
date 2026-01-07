using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadows.Data.Model
{
    public class FileData : BaseEntity
    {
        public string FileName { get; set; }

        public List<FileVisibility> Visibility { get; set; }

        public FileData()
        {
            Visibility = new List<FileVisibility>();
        }

        public bool IsVisible(bool isAdmin, int idCharacter)
        {
            if (isAdmin)
            {
                return true;
            }
            else if (Visibility.Count == 0)
            {
                return true;
            }
            else
            {
                foreach(FileVisibility fv in Visibility)
                {
                    if (fv.IdCharacter == idCharacter)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

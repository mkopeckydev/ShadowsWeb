using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shadows.Data.Model
{
    public class FileItem
    {
        public string FileName { get; set; }
        public bool IsAdmin { get; set; }
        public FileData Data { get; set; }

        public string FileUrl
        {
            get
            {
                return String.Format("~/{0}/{1}", FilesManager.FILES_PATH, FileName);
            }
        }

        public int IdFileData
        {
            get
            {
                if (Data != null)
                {
                    return Data.Id;
                }

                return 0;
            }
        }

        public string VisibilityLabel
        {
            get
            {
                if (Data.Visibility.Count > 0)
                {
                    return String.Format("Viditelnost ({0})", Data.Visibility.Count);
                }
                else
                {
                    return "Viditelnost";
                }
            }
        }
    }
}

using Shadows.Data.Facade;
using Shadows.Data.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Shadows.Data
{
    public class FilesManager
    {
        public const string FILES_PATH = "FilesData";
        public const string IMAGES_PATH = "ImagesData";
        public const string TEMP_PATH = "Temp";

        public DirectoryInfo GetTempDir()
        {
            DirectoryInfo di = new DirectoryInfo(AppConfig.TempPath);

            if (!di.Exists)
            {
                di.Create();
            }

            return di;
        }

        public DirectoryInfo GetFilesDir()
        {
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(FILES_PATH));

            if (!di.Exists)
            {
                di.Create();
            }

            return di;
        }

        public DirectoryInfo GetImagesDir()
        {
            DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(IMAGES_PATH));

            if (!di.Exists)
            {
                di.Create();
            }

            return di;
        }

        public string GetTempPath(string fileName)
        {
            DirectoryInfo di = GetTempDir();
            string result = Path.Combine(di.FullName, fileName);
            return result;
        }

        public string GetFilePath(string fileName)
        {
            DirectoryInfo di = GetFilesDir();
            string result = Path.Combine(di.FullName, fileName);
            return result;
        }

        public string GetImagePath(string fileName)
        {
            DirectoryInfo di = GetImagesDir();
            string result = Path.Combine(di.FullName, fileName);
            return result;
        }

        private List<FileInfo> GetFilesInternal()
        {
            DirectoryInfo di = GetFilesDir();
            FileInfo[] files = di.GetFiles();

            List<FileInfo> result = new List<FileInfo>(files);
            return result;
        }

        public void DeleteFile(string fileName)
        {
            try
            {
                List<FileInfo> files = GetFilesInternal();
                foreach (FileInfo fi in files)
                {
                    if (fi.Name == fileName)
                    {
                        fi.Delete();
                    }
                }
            }
            catch
            { }
        }

        public List<FileItem> GetFiles(bool isAdmin, int idCharacter, ISession nHibernatesession)
        {
            List<FileItem> result = new List<FileItem>();

            try
            {
                List<FileInfo> files = GetFilesInternal();
                foreach(FileInfo fi in files)
                {
                    FileItem item = new FileItem();
                    item.FileName = fi.Name;
                    item.IsAdmin = isAdmin;
                    item.Data = GetFileData(item.FileName, nHibernatesession);

                    if (item.Data.IsVisible(isAdmin, idCharacter))
                    {
                        result.Add(item);
                    }
                }
            }
            catch
            { }

            return result;
        }

        private FileData GetFileData(string fileName, ISession nHibernatesession)
        {
            BusinessFacade<FileData> fFileData = new BusinessFacade<FileData>(nHibernatesession);
            BusinessFacade<FileVisibility> fFileVisibility = new BusinessFacade<FileVisibility>(nHibernatesession);

            List<FileData> listData = fFileData.GetListEntities(String.Empty, true, String.Format("(FileName like '{0}')", fileName));

            FileData data = new FileData();
            if (listData.Count > 0)
            {
                data = listData[0];
            }
            else
            {
                data.FileName = fileName;
                fFileData.SaveEntity(data);
            }

            data.Visibility = fFileVisibility.GetListEntities(String.Empty, true, String.Format("(IdFileData = {0})", data.Id));

            return data;
        }

        public MemoryStream ReadFile(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            FileInfo fi = new FileInfo(filePath);

            if (fi.Exists)
            {
                FileStream fs = File.OpenRead(filePath);
                fs.CopyTo(ms);
                fs.Close();
            }

            return ms;
        }
    }
}

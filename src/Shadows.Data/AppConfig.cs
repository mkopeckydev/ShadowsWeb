using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Shadows.Data
{
    public class AppConfig
    {
        private AppConfig()
        { }

        public static string TempPath
        {
            get
            {
                return ConfigurationManager.AppSettings["TempPath"];
            }
        }
    }
}
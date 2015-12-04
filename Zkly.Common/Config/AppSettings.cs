using System;
using System.Collections.Specialized;
using System.Configuration;
using Zkly.Common.Extension;

namespace Zkly.Common.Config
{
    public class AppSettings
    {
        private static NameValueCollection Settings
        {
            get
            {
                return ConfigurationManager.GetSection("appSettings") as NameValueCollection;
            }
        }

        public static readonly string MachineName = Environment.MachineName;
        public static readonly string ServerName = Settings["ServerName"];
        public static readonly long FileWatcherInterval = Settings["FileWatcherInterval"].ToLong();
        public static readonly bool FileWatcherDisable = Settings["FileWatcherDisable"].ToBoolean();

        public static readonly bool AllowUserDeletion = Settings["AllowUserDeletion"].ToBoolean();
    }
}

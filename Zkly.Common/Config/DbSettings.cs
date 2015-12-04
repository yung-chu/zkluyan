using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common.Extension;

namespace Zkly.Common.Config
{
    public static class DbSettings
    {
        private static NameValueCollection Settings
        {
            get
            {
                return ConfigurationManager.GetSection("dbSettings") as NameValueCollection;
            }
        }

        public static readonly bool MigrateToLatestVersion = Settings["MigrateDatabaseToLatestVersion"].ToBoolean();
    }
}

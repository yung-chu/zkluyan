using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Zkly.Common.Config;
using Zkly.DAL.Context;

namespace Zkly.DAL.Migrations
{
    public class MigrateConfig
    {
        public static void RegisterContexts()
        {
            if (DbSettings.MigrateToLatestVersion)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserDbContext, User.Configuration>());
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ConfigDbContext, Config.Configuration>());
            }
            else
            {
                Database.SetInitializer<UserDbContext>(null);
                Database.SetInitializer<ConfigDbContext>(null);
            }
        }
    }
}
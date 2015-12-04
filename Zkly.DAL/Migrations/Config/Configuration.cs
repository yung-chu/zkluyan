using DbSetMigrationsExtensions = System.Data.Entity.Migrations.DbSetMigrationsExtensions;

namespace Zkly.DAL.Migrations.Config
{
    internal sealed class Configuration : System.Data.Entity.Migrations.DbMigrationsConfiguration<Context.ConfigDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            MigrationsDirectory = @"Migrations\Config";
        }

        protected override void Seed(Context.ConfigDbContext context)
        {
            // This method will be called after migrating to the latest version.
            DbSetMigrationsExtensions.AddOrUpdate(
                context.Organizations,
                o => o.Name,
                new Models.Organization { Name = "ÃÏ π" },
                new Models.Organization { Name = "VC" },
                new Models.Organization { Name = "PE" });
        }
    }
}

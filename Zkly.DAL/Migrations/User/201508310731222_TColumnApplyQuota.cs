namespace Zkly.DAL.Migrations.User
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TColumnApplyQuota : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InvestFirstAuditInfoes", "ApplyQuota", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InvestFirstAuditInfoes", "ApplyQuota", c => c.Int(nullable: false));
        }
    }
}

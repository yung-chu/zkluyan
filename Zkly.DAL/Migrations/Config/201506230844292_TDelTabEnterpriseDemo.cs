namespace Zkly.DAL.Migrations.Config
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TDelTabEnterpriseDemo : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.EnterpriseDemo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EnterpriseDemo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompanyName = c.String(),
                        ProjectName = c.String(),
                        Contract = c.String(),
                        Phone = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
    }
}

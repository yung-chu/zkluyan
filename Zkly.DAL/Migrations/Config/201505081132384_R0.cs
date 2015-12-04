namespace Zkly.DAL.Migrations.Config
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R0 : DbMigration
    {
        public override void Up()
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
            
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Organization");
            DropTable("dbo.EnterpriseDemo");
        }
    }
}

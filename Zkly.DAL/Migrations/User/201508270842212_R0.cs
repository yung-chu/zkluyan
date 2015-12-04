namespace Zkly.DAL.Migrations.User
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgencyCommissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CashPercent = c.Single(nullable: false),
                        StockPercent = c.Single(nullable: false),
                        Description = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CapitalRoadshows",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        CompanyName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Subject = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        StartDate = c.DateTime(nullable: false, precision: 0),
                        EndDate = c.DateTime(nullable: false, precision: 0),
                        Hoster = c.String(maxLength: 30, storeType: "nvarchar"),
                        Description = c.String(maxLength: 500, storeType: "nvarchar"),
                        PublicPassword = c.String(maxLength: 20, storeType: "nvarchar"),
                        WebinarId = c.String(maxLength: 10, storeType: "nvarchar"),
                        Status = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        FileId = c.Long(nullable: false),
                        RecordState = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Invests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128, storeType: "nvarchar"),
                        Stage = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AuditQuota = c.Int(),
                        Reason = c.String(maxLength: 500, storeType: "nvarchar"),
                        ApplyTime = c.DateTime(nullable: false, precision: 0),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.InvestAgreements",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        LockMonth = c.Short(nullable: false),
                        AgencyCommissionId = c.Int(nullable: false),
                        AgreementFileId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgencyCommissions", t => t.AgencyCommissionId, cascadeDelete: true)
                .ForeignKey("dbo.UploadFileInfoes", t => t.AgreementFileId)
                .ForeignKey("dbo.Invests", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.AgencyCommissionId)
                .Index(t => t.AgreementFileId);
            
            CreateTable(
                "dbo.UploadFileInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FilePath = c.String(maxLength: 100, storeType: "nvarchar"),
                        FileName = c.String(maxLength: 100, storeType: "nvarchar"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvestFirstAuditInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        CompanyName = c.String(maxLength: 100, storeType: "nvarchar"),
                        CompanyDescription = c.String(maxLength: 4000, storeType: "nvarchar"),
                        ApplyQuota = c.Int(nullable: false),
                        CompanyAssessment = c.Long(),
                        FoundingDate = c.DateTime(precision: 0),
                        Industry = c.String(maxLength: 10, storeType: "nvarchar"),
                        Area = c.String(maxLength: 200, storeType: "nvarchar"),
                        ProjectName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        IsTeamAdv = c.Boolean(nullable: false),
                        TeamAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsProdAdv = c.Boolean(nullable: false),
                        ProdAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsTechAdv = c.Boolean(nullable: false),
                        TechAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsScaleAdv = c.Boolean(nullable: false),
                        ScaleAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsSaleAdv = c.Boolean(nullable: false),
                        SaleAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsIndustryAdv = c.Boolean(nullable: false),
                        IndustryAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsResourceAdv = c.Boolean(nullable: false),
                        ResourceAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsOtherAdv = c.Boolean(nullable: false),
                        OtherAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        Data1 = c.Long(),
                        Data2 = c.Long(),
                        Data3 = c.Long(),
                        Data4 = c.Long(),
                        Data5 = c.Long(),
                        IndustryPosition = c.String(maxLength: 500, storeType: "nvarchar"),
                        IndustryCompetition = c.String(maxLength: 500, storeType: "nvarchar"),
                        ProjectAwards = c.String(maxLength: 500, storeType: "nvarchar"),
                        InvestmentInstitutions = c.String(maxLength: 10, storeType: "nvarchar"),
                        LegalPerson = c.String(maxLength: 50, storeType: "nvarchar"),
                        LegalPhone = c.String(maxLength: 20, storeType: "nvarchar"),
                        LegalCellPhone = c.String(maxLength: 20, storeType: "nvarchar"),
                        Contact = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        ContactPhone = c.String(maxLength: 20, storeType: "nvarchar"),
                        ContactCellPhone = c.String(maxLength: 20, storeType: "nvarchar"),
                        MarketAndSales = c.String(maxLength: 500, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.InvestHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InvestId = c.Long(nullable: false),
                        UserId = c.String(maxLength: 128, storeType: "nvarchar"),
                        Stage = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AuditQuota = c.Long(),
                        Remark = c.String(maxLength: 500, storeType: "nvarchar"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invests", t => t.InvestId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.InvestId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        DisplayName = c.String(maxLength: 50, storeType: "nvarchar"),
                        Email = c.String(maxLength: 256, storeType: "nvarchar"),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 0),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        RoleId = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roadshows",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Priority = c.Int(nullable: false),
                        VideoName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        VideoDescrition = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        VideoSize = c.Long(nullable: false),
                        VideoFileName = c.String(maxLength: 100, storeType: "nvarchar"),
                        CoverFileId = c.Long(nullable: false),
                        Folder = c.String(maxLength: 100, storeType: "nvarchar"),
                        VhallRoadshowAddress = c.String(maxLength: 100, storeType: "nvarchar"),
                        IsOnShow = c.Boolean(nullable: false),
                        SubmitDate = c.DateTime(nullable: false, precision: 0),
                        UpdateDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.InvestSecondAuditInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Address = c.String(maxLength: 500, storeType: "nvarchar"),
                        RegisteredCapital = c.Long(),
                        CompanyStage = c.Int(nullable: false),
                        Introduction = c.String(maxLength: 500, storeType: "nvarchar"),
                        ProjectSource = c.String(maxLength: 500, storeType: "nvarchar"),
                        ProjectStage = c.String(maxLength: 500, storeType: "nvarchar"),
                        ProjectIntroduction = c.String(maxLength: 500, storeType: "nvarchar"),
                        Inferiority = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsHasIPR = c.Boolean(nullable: false),
                        PatentStatus = c.Int(nullable: false),
                        PatentNumber = c.String(maxLength: 100, storeType: "nvarchar"),
                        IprForm = c.String(maxLength: 50, storeType: "nvarchar"),
                        PatentInventor = c.String(maxLength: 50, storeType: "nvarchar"),
                        PatentOwner = c.String(maxLength: 50, storeType: "nvarchar"),
                        Plan = c.String(maxLength: 500, storeType: "nvarchar"),
                        RiskPrevention = c.String(maxLength: 500, storeType: "nvarchar"),
                        Debt = c.Boolean(nullable: false),
                        DebtAmount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.InvestSecondAuditFiles",
                c => new
                    {
                        InvestSecondAuditId = c.Long(nullable: false),
                        UploadFileInfoesId = c.Long(nullable: false),
                        FileTypeId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.InvestSecondAuditId, t.UploadFileInfoesId })
                .ForeignKey("dbo.InvestSecondAuditInfoes", t => t.InvestSecondAuditId, cascadeDelete: true, name: "FK_SecondAuditFiles_SecondAuditInfoes_InvestSecondAuditId")
                .ForeignKey("dbo.UploadFileInfoes", t => t.UploadFileInfoesId, cascadeDelete: true)
                .Index(t => t.InvestSecondAuditId)
                .Index(t => t.UploadFileInfoesId);
            
            CreateTable(
                "dbo.DataDictionaries",
                c => new
                    {
                        DataDictionaryId = c.Long(nullable: false),
                        DataDictionaryType = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        DataDictionaryName = c.String(maxLength: 50, storeType: "nvarchar"),
                        LevelCode = c.String(maxLength: 50, storeType: "nvarchar"),
                        Symbol = c.String(maxLength: 50, storeType: "nvarchar"),
                        ParentId = c.Int(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DataDictionaryId, t.DataDictionaryType });
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvestorPreferences",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128, storeType: "nvarchar"),
                        Lower = c.Long(nullable: false),
                        Upper = c.Long(nullable: false),
                        OrgPreference = c.String(maxLength: 128, storeType: "nvarchar"),
                        IndustryPreference = c.String(maxLength: 128, storeType: "nvarchar"),
                        FileId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.InvestRecents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false, precision: 0),
                        InvestAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvestSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CompanySum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvestTempFolders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128, storeType: "nvarchar"),
                        InvestId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invests", t => t.InvestId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.InvestId);
            
            CreateTable(
                "dbo.LoanAudits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Stage = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AuditQuota = c.Long(),
                        FailReason = c.String(maxLength: 500, storeType: "nvarchar"),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoanRecents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false, precision: 0),
                        LoanAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoanSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BorrowUser = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128, storeType: "nvarchar"),
                        CompanyName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        CompanyDescription = c.String(nullable: false, maxLength: 4000, storeType: "nvarchar"),
                        ApplyTime = c.DateTime(nullable: false, precision: 0),
                        ApplyQuota = c.Long(nullable: false),
                        GuaranteeAssessment = c.Long(nullable: false),
                        FoundingDate = c.DateTime(nullable: false, precision: 0),
                        Industry = c.String(nullable: false, maxLength: 10, storeType: "nvarchar"),
                        Area = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ProjectName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Contract = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Phone = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        IsTeamAdv = c.Boolean(nullable: false),
                        TeamAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsProdAdv = c.Boolean(nullable: false),
                        ProdAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsTechAdv = c.Boolean(nullable: false),
                        TechAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsScaleAdv = c.Boolean(nullable: false),
                        ScaleAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsSaleAdv = c.Boolean(nullable: false),
                        SaleAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsIndustryAdv = c.Boolean(nullable: false),
                        IndustryAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsResourceAdv = c.Boolean(nullable: false),
                        ResourceAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        IsOtherAdv = c.Boolean(nullable: false),
                        OtherAdv = c.String(maxLength: 500, storeType: "nvarchar"),
                        Data1 = c.Long(),
                        Data2 = c.Long(),
                        Data3 = c.Long(),
                        Data4 = c.Long(),
                        Data5 = c.Long(),
                        UpdateDate = c.DateTime(nullable: false, precision: 0),
                        Status = c.Int(nullable: false),
                        FailReason = c.String(maxLength: 500, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128, storeType: "nvarchar"),
                        From = c.String(maxLength: 128, storeType: "nvarchar"),
                        To = c.String(maxLength: 128, storeType: "nvarchar"),
                        Subject = c.String(maxLength: 100, storeType: "nvarchar"),
                        Body = c.String(maxLength: 500, storeType: "nvarchar"),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                        State = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MetaIndexes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IndexFile = c.String(maxLength: 20, storeType: "nvarchar"),
                        VideoName = c.String(maxLength: 20, storeType: "nvarchar"),
                        VhallShowAddress = c.String(maxLength: 20, storeType: "nvarchar"),
                        Error = c.String(maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoadshowUploadWatchers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Folder = c.String(maxLength: 100, storeType: "nvarchar"),
                        FileName = c.String(maxLength: 100, storeType: "nvarchar"),
                        SyncStatus = c.Int(nullable: false),
                        ServerName = c.String(maxLength: 100, storeType: "nvarchar"),
                        UpdateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");     
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Messages", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Loans", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InvestTempFolders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InvestTempFolders", "InvestId", "dbo.Invests");
            DropForeignKey("dbo.InvestorPreferences", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CapitalRoadshows", "Id", "dbo.Invests");
            DropForeignKey("dbo.Invests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InvestSecondAuditFiles", "UploadFileInfoesId", "dbo.UploadFileInfoes");
            DropForeignKey("dbo.InvestSecondAuditFiles", "InvestSecondAuditId", "dbo.InvestSecondAuditInfoes");
            DropForeignKey("dbo.InvestSecondAuditInfoes", "Id", "dbo.Invests");
            DropForeignKey("dbo.Roadshows", "Id", "dbo.Invests");
            DropForeignKey("dbo.InvestHistories", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InvestHistories", "InvestId", "dbo.Invests");
            DropForeignKey("dbo.InvestFirstAuditInfoes", "Id", "dbo.Invests");
            DropForeignKey("dbo.InvestAgreements", "Id", "dbo.Invests");
            DropForeignKey("dbo.InvestAgreements", "AgreementFileId", "dbo.UploadFileInfoes");
            DropForeignKey("dbo.InvestAgreements", "AgencyCommissionId", "dbo.AgencyCommissions");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Loans", new[] { "UserId" });
            DropIndex("dbo.InvestTempFolders", new[] { "InvestId" });
            DropIndex("dbo.InvestTempFolders", new[] { "UserId" });
            DropIndex("dbo.InvestorPreferences", new[] { "UserId" });
            DropIndex("dbo.InvestSecondAuditFiles", new[] { "UploadFileInfoesId" });
            DropIndex("dbo.InvestSecondAuditFiles", new[] { "InvestSecondAuditId" });
            DropIndex("dbo.InvestSecondAuditInfoes", new[] { "Id" });
            DropIndex("dbo.Roadshows", new[] { "Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.InvestHistories", new[] { "UserId" });
            DropIndex("dbo.InvestHistories", new[] { "InvestId" });
            DropIndex("dbo.InvestFirstAuditInfoes", new[] { "Id" });
            DropIndex("dbo.InvestAgreements", new[] { "AgreementFileId" });
            DropIndex("dbo.InvestAgreements", new[] { "AgencyCommissionId" });
            DropIndex("dbo.InvestAgreements", new[] { "Id" });
            DropIndex("dbo.Invests", new[] { "UserId" });
            DropIndex("dbo.CapitalRoadshows", new[] { "Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RoadshowUploadWatchers");
            DropTable("dbo.MetaIndexes");
            DropTable("dbo.Messages");
            DropTable("dbo.Loans");
            DropTable("dbo.LoanRecents");
            DropTable("dbo.LoanAudits");
            DropTable("dbo.InvestTempFolders");
            DropTable("dbo.InvestRecents");
            DropTable("dbo.InvestorPreferences");
            DropTable("dbo.Industries");
            DropTable("dbo.DataDictionaries");
            DropTable("dbo.InvestSecondAuditFiles");
            DropTable("dbo.InvestSecondAuditInfoes");
            DropTable("dbo.Roadshows");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.InvestHistories");
            DropTable("dbo.InvestFirstAuditInfoes");
            DropTable("dbo.UploadFileInfoes");
            DropTable("dbo.InvestAgreements");
            DropTable("dbo.Invests");
            DropTable("dbo.CapitalRoadshows");
            DropTable("dbo.AgencyCommissions");
        }
    }
}

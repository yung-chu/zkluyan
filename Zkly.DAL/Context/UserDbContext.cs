using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Zkly.DAL.Models;

namespace Zkly.DAL.Context
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>()
                .Property(c => c.Name).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers")//I have to declare the table name, otherwise IdentityUser will be created
                .Property(c => c.UserName).HasMaxLength(128).IsRequired();
        }

        public static UserDbContext Create()
        {
            return new UserDbContext();
        }

        #region invest
        public DbSet<Invest> Invests { get; set; }

        public DbSet<InvestHistory> InvestAuditHistories { get; set; }

        public DbSet<InvestorPreference> InvestorPreferences { get; set; }

        public DbSet<InvestFirstAuditInfo> FirstAuditInfos { get; set; }

        #region SecondAuditInfo

        public DbSet<InvestSecondAuditInfo> SecondAuditInfos { get; set; }

        #endregion

        public DbSet<InvestAgreement> InvestAgreements { get; set; }

        public DbSet<AgencyCommission> AgencyCommissions { get; set; }

        public DbSet<InvestSecondAuditFile> InvestSecondAuditFiles { get; set; }
        #endregion

        #region loan
        public DbSet<Loan> Loans { get; set; }

        public DbSet<LoanAudit> LoanAudits { get; set; }
        #endregion

        #region Roadshow
        public DbSet<Roadshow> Roadshows { get; set; }

        public DbSet<CapitalRoadshow> CapitalRoadshows { get; set; }

        public DbSet<MetaIndex> MetaIndexes { get; set; }

        public DbSet<RoadshowUploadWatcher> RoadshowUploadWatchers { get; set; }
        #endregion

        #region Message
        public DbSet<Message> Messages { get; set; }

        #endregion

        #region IndexTable

        public DbSet<InvestRecent> InvestRecents { get; set; }

        public DbSet<LoanRecent> LoanRecents { get; set; }

        public DbSet<Industry> Industries { get; set; }
        #endregion

        public DbSet<InvestTempFolder> InvestTempFolders { get; set; }

        public DbSet<UploadFileInfo> UploadFileInfos { get; set; }

        ////数据字典的功用对象
        public DbSet<DataDictionary> DataDictionaries { get; set; }
    }
}
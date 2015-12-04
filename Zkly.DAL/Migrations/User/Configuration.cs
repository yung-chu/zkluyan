using System;
using System.Data.Entity.Migrations;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.DAL.Migrations.User
{
    internal sealed class Configuration : DbMigrationsConfiguration<UserDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            MigrationsDirectory = @"Migrations\User";
        }

        protected override void Seed(UserDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            AddOrUpdateRoles(context);
            AddOrUpdateUsers(context);

            AddOrUpdateIndustry(context);

            AddOrUpdateAgencyCommission(context);

            AddOrUpdateDataDictionaries(context);
            context.SaveChanges();
        }

        private void AddOrUpdateRoles(UserDbContext context)
        {
            using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
            {
                // 添加系统角色
                if (!roleManager.RoleExists(EUserRole.Administrator.ToString()))
                {
                    roleManager.Create(new IdentityRole(EUserRole.Administrator.ToString()));
                }

                if (!roleManager.RoleExists(EUserRole.Enterprise.ToString()))
                {
                    roleManager.Create(new IdentityRole(EUserRole.Enterprise.ToString()));
                }

                if (!roleManager.RoleExists(EUserRole.Investor.ToString()))
                {
                    roleManager.Create(new IdentityRole(EUserRole.Investor.ToString()));
                }
            }
        }

        private void AddOrUpdateUsers(UserDbContext context)
        {
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                // 创建账号
                CreateUser(userManager, "Admin", "admin@zkjinji.com", "15866880000", EUserRole.Administrator);

                CreateUser(userManager, "TestEnt01", "testuser01@zkjinji.com", "13900660001", EUserRole.Enterprise);
                CreateUser(userManager, "TestEnt02", "testuser02@zkjinji.com", "13900660002", EUserRole.Enterprise);
                CreateUser(userManager, "TestEnt03", "testuser03@zkjinji.com", "13900660003", EUserRole.Enterprise);
                CreateUser(userManager, "TestEnt04", "testuser04@zkjinji.com", "13900660004", EUserRole.Enterprise);
                CreateUser(userManager, "TestEnt05", "testuser05@zkjinji.com", "13900660005", EUserRole.Enterprise);

                CreateUser(userManager, "TestInvest01", "testuser11@zkjinji.com", "18022880005", EUserRole.Investor);
                CreateUser(userManager, "TestInvest02", "testuser12@zkjinji.com", "18022880005", EUserRole.Investor);
                CreateUser(userManager, "TestInvest03", "testuser13@zkjinji.com", "18022880005", EUserRole.Investor);
                CreateUser(userManager, "TestInvest04", "testuser14@zkjinji.com", "18022880005", EUserRole.Investor);
                CreateUser(userManager, "TestInvest05", "testuser15@zkjinji.com", "18022880005", EUserRole.Investor);
            }
        }

        private void CreateUser(UserManager<ApplicationUser> userManager, string name, string email, string phone, EUserRole role)
        {
            var user = new ApplicationUser() { UserName = name, DisplayName = name, Email = email, PhoneNumber = phone, EmailConfirmed = true, PhoneNumberConfirmed = true };
            if (userManager.Create(user, "user@MAS2") == IdentityResult.Success)
            {
                // 为账号分配角色
                userManager.AddToRole(user.Id, role.ToString());
            }
            else
            {
                Console.WriteLine("初始化帐号失败：" + name);
            }
        }

        private void AddOrUpdateIndustry(UserDbContext context)
        {
            context.Industries.AddOrUpdate(
                                 o => o.Name, 
                                 new Industry { Name = "互联网" }, 
                                 new Industry { Name = "IT" }, 
                                 new Industry { Name = "电信及增值" }, 
                                 new Industry { Name = "传媒娱乐" }, 
                                 new Industry { Name = "能源" }, 
                                 new Industry { Name = "医疗健康" }, 
                                 new Industry { Name = "旅行" }, 
                                 new Industry { Name = "游戏" }, 
                                 new Industry { Name = "金融" }, 
                                 new Industry { Name = "教育" }, 
                                 new Industry { Name = "房地产" }, 
                                 new Industry { Name = "物流仓储" }, 
                                 new Industry { Name = "农林牧渔" }, 
                                 new Industry { Name = "住宿餐饮" }, 
                                 new Industry { Name = "商业服务" }, 
                                 new Industry { Name = "消费品" }, 
                                 new Industry { Name = "文体艺术" }, 
                                 new Industry { Name = "加工制造" }, 
                                 new Industry { Name = "其他" });
        }

        private void AddOrUpdateAgencyCommission(UserDbContext context)
        {
            context.AgencyCommissions.AddOrUpdate(
                                 o => o.Description, 
                                 new AgencyCommission { CashPercent = 0.05f, Description = "现金5%" }, 
                                 new AgencyCommission { CashPercent = 0.04f, Description = "现金4%" }, 
                                 new AgencyCommission { CashPercent = 0.03f, Description = "现金3%" }, 
                                 new AgencyCommission { StockPercent = 0.03f, Description = "股权3%" }, 
                                 new AgencyCommission { StockPercent = 0.02f, Description = "股权2%" }, 
                                 new AgencyCommission { StockPercent = 0.01f, Description = "股权1%" }, 
                                 new AgencyCommission { CashPercent = 0.05f, StockPercent = 0.01f, Description = "现金5% + 股权1%" }, 
                                 new AgencyCommission { CashPercent = 0.04f, StockPercent = 0.02f, Description = "现金4% + 股权2%" }, 
                                 new AgencyCommission { CashPercent = 0.03f, StockPercent = 0.03f, Description = "现金3% + 股权3%" });
        }

        private void AddOrUpdateDataDictionaries(UserDbContext context)
        {
            context.DataDictionaries.AddOrUpdate(
                o => o.DataDictionaryName,
                new DataDictionary { DataDictionaryId=1, DataDictionaryType = Zkly.Common.Dictionary.Enums.DataDictionaryType.IsRegister.ToString(), DataDictionaryName = "0" });
        }
    }
}

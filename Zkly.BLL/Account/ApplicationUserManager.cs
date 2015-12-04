using System;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Zkly.BLL.Message;
using Zkly.BLL.Repositories;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Account
{
    // 配置此应用程序中使用的应用程序用户管理器。UserManager 在 ASP.NET Identity 中定义，并由此应用程序使用。
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public virtual Task<ApplicationUser> FindByPhoneAsync(string phone)
        {
            return this.UserStore.FindByPhoneAsync(phone);
        }

        #region Role

        public bool IsAdmin(string userId)
        {
            //this.IsAdmin
            return this.IsInRole(userId, EUserRole.Administrator);
        }

        public bool IsEnterprise(string userId)
        {
            return this.IsInRole(userId, EUserRole.Enterprise);
        }

        public bool IsInvestor(string userId)
        {
            return this.IsInRole(userId, EUserRole.Investor);
        }

        public bool IsInRole(string userId, EUserRole role)
        {
            return this.IsInRole(userId, role.ToString());
        }

        public Task<bool> IsInRoleAsync(string userId, EUserRole role)
        {
            return base.IsInRoleAsync(userId, role.ToString());
        }

        public IdentityResult AddToRole(string userId, EUserRole role)
        {
            return this.AddToRole(userId, role.ToString());
        }

        public Task<IdentityResult> AddToRoleAsync(string userId, EUserRole role)
        {
            return this.AddToRoleAsync(userId, role.ToString());
        }

        public async Task<IdentityResult> RemoveAllRolesAsync(string userId)
        {
            var roles = await this.GetRolesAsync(userId).WithCurrentCulture();
            return await RemoveFromRolesAsync(userId, roles.ToArray()).WithCurrentCulture();
        }
        #endregion

        #region Message Service
        public IIdentityMessageService SystemMessageService { get; set; }

        public async Task SendAllMessagesAsync(string userId, string subject, string body)
        {
            await SendSystemMessageAsync(userId, subject, body);
            await SendSmsAsync(userId, subject, body);
            await SendEmailAsync(userId, subject, body);
        }

        public async Task SendSystemMessageAsync(string userId, string subject, string body)
        {
            if (SystemMessageService != null)
            {
                var msg = new IdentityMessage
                {
                    Destination = userId,
                    Subject = subject,
                    Body = body,
                };
                await SystemMessageService.SendAsync(msg).WithCurrentCulture();
            }
        }

        public async Task SendSmsAsync(string userId, string subject, string body)
        {
            if (SmsService != null)
            {
                var msg = new IdentityMessage
                {
                    Destination = await GetPhoneNumberAsync(userId).WithCurrentCulture(),
                    Subject = subject,
                    Body = body,
                };
                await SmsService.SendAsync(msg).WithCurrentCulture();
            }
        }
        #endregion

        #region create instance & config for OWIN
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, UserDbContext context)
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore(context));

            // 配置用户名的验证逻辑
            manager.UserValidator = new ApplicationUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
                RequireUniquePhoneNumber = true,
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider(
                "短信验证",
                new PhoneNumberTokenProvider<ApplicationUser>
                    {
                        MessageFormat = "您的短信验证码是：{0}"
                    });
            manager.RegisterTwoFactorProvider(
                "电子邮件验证",
                new EmailTokenProvider<ApplicationUser>
                    {
                        Subject = "登录验证码",
                BodyFormat = "您的邮件验证码是：{0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            manager.SystemMessageService = new SystemMessageService();

            if (options != null && options.DataProtectionProvider != null)
            {
                var protector = options.DataProtectionProvider.Create("ASP.NET Identity");
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(protector) { TokenLifespan = TimeSpan.FromHours(12) };
            }

            return manager;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            return Create(options, context.Get<UserDbContext>());
        }
        #endregion

        #region 企业信息

        public int PublicEnterpriseMessageCount(string userId, bool state = false)
        {
            MessageRepository messageRepository = new MessageRepository();
            return messageRepository.GetMessagesCount(userId, false);
        }
        #endregion

        protected ApplicationUserStore UserStore
        {
            get
            {
                return (ApplicationUserStore)Store;
            }
        }
    }
}

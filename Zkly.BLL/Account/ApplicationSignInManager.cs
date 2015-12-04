using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

using Zkly.Common.Utils;
using Zkly.DAL.Models;

namespace Zkly.BLL.Account
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public new ApplicationUserManager UserManager
        {
            get { return (ApplicationUserManager)base.UserManager; }
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync(UserManager);
        }

        public virtual async Task<SignInStatus> SignInAsync(string userNameOrEmailOrPhone, string password, bool isPersistent, bool shouldLockout)
        {
            var user = await GetUser(userNameOrEmailOrPhone);
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            //var isAdmin = await UserManager.IsInRoleAsync(user.Id, EUserRole.Administrator);
            //return isAdmin ? SignInStatus.Failure : await this.PasswordSignInAsync(user.UserName, password, isPersistent, shouldLockout);
            return await this.PasswordSignInAsync(user.UserName, password, isPersistent, shouldLockout);
        }

        public virtual async Task<SignInStatus> AdminSignInAsync(string userNameOrEmailOrPhone, string password, bool isPersistent, bool shouldLockout)
        {
            var user = await GetUser(userNameOrEmailOrPhone);
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            var isAdmin = await UserManager.IsInRoleAsync(user.Id, EUserRole.Administrator);
            return isAdmin ? await this.PasswordSignInAsync(user.UserName, password, isPersistent, shouldLockout) : SignInStatus.Failure;
        }

        public Task<ApplicationUser> GetUser(string userNameOrEmailOrPhone)
        {
            if (ValidateUtil.IsMobilePhone(userNameOrEmailOrPhone))
            {
                return UserManager.FindByPhoneAsync(userNameOrEmailOrPhone);
            }

            if (ValidateUtil.IsValidEmail(userNameOrEmailOrPhone))
            {
                return UserManager.FindByEmailAsync(userNameOrEmailOrPhone);
            }

            return UserManager.FindByNameAsync(userNameOrEmailOrPhone);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}

using System.Data.Entity.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Zkly.Common.Extension;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Account
{
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store)
            : base(store)
        {
        }

        public async Task<IdentityRole> GetRoleAsync(ApplicationUser user)
        {
            return await Store.FindByIdAsync(user.Roles.First().RoleId).WithCurrentCulture();
        }

        public async Task<string> GetRoleNameAsync(ApplicationUser user)
        {
            var role = await GetRoleAsync(user);
            return role.Name;
        }

        public async Task<EUserRole> GetRoleTypeAsync(ApplicationUser user)
        {
            var role = await GetRoleAsync(user);
            return role.Name.ToEnum<EUserRole>();
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context.Get<UserDbContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }
}

using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Zkly.DAL.Models;
using AsyncHelper = Zkly.Common.Extension.AsyncHelper;

namespace Zkly.BLL.Account
{
    public static class RoleManagerExtensions
    {
        public static IdentityRole GetRole(this ApplicationRoleManager manager, ApplicationUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            return AsyncHelper.RunSync(() => manager.GetRoleAsync(user));
        }

        public static string GetRoleName(this ApplicationRoleManager manager, ApplicationUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            return AsyncHelper.RunSync(() => manager.GetRoleNameAsync(user));
        }

        public static EUserRole GetRoleType(this ApplicationRoleManager manager, ApplicationUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            return AsyncHelper.RunSync(() => manager.GetRoleTypeAsync(user));
        }
    }
}

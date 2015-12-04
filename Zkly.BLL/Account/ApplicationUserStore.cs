using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Account
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(UserDbContext context)
            : base(context)
        {
        }

        public virtual Task<ApplicationUser> FindByPhoneAsync(string phone)
        {
            if (phone == null)
            {
                throw new ArgumentNullException("phone");
            }

            return GetUserAggregateAsync(u => u.PhoneNumber == phone.Trim());
        }
    }
}

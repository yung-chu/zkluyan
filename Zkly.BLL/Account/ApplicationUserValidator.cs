using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Zkly.Common.Utils;
using Zkly.DAL.Models;

namespace Zkly.BLL.Account
{
    public class ApplicationUserValidator : UserValidator<ApplicationUser, string>
    {
        private readonly ApplicationUserManager manager;

        public ApplicationUserValidator(ApplicationUserManager manager)
            : base(manager)
        {
            this.manager = manager;
        }

        public bool RequireUniquePhoneNumber { get; set; }

        /// <summary>
        /// Validates a user before saving
        /// </summary>
        /// <param name="item">item</param>
        /// <returns>IdentityResult.Success</returns>
        public override async Task<IdentityResult> ValidateAsync(ApplicationUser item)
        {
            var result = await base.ValidateAsync(item);
            var errors = new List<string>(result.Errors);
            await ValidatePhoneAsync(item, errors).WithCurrentCulture();
            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidatePhoneAsync(ApplicationUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                errors.Add("手机号码不能为空。");
            }
            else if (!ValidateUtil.IsMobilePhone(user.PhoneNumber))
            {
                errors.Add("手机号码格式不正确。");
            }
            else if (RequireUniquePhoneNumber)
            {
                var owner = await manager.FindByPhoneAsync(user.PhoneNumber).WithCurrentCulture();
                if (owner != null && !EqualityComparer<string>.Default.Equals(owner.Id, user.Id))
                {
                    errors.Add(string.Format("手机号码\"{0}\"已被使用。", user.PhoneNumber));
                }
            }
        }
    }
}

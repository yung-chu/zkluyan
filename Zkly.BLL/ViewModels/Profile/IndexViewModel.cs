using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace Zkly.BLL.ViewModels
{
    public class IndexViewModel
    {
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}

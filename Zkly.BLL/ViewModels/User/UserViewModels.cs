using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class UserViewModels
    {
        public List<ApplicationUser> List { get; set; }

        public ApplicationUser User { get; set; }
    }
}

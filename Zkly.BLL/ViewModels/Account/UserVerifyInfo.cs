using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.ViewModels
{
    public class UserVerifyInfo
    {
        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }
    }
}

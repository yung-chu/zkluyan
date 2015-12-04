using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.ViewModels
{
    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "请输入验证码")]
        [Display(Name = "验证码")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "手机号码")]
        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
    }
}

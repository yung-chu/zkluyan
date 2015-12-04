using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "用户名输入不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码输入不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "登录密码")]
        public string Password { get; set; }

        [Display(Name = "自动登录")]
        public bool RememberMe { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "电子邮件不能为空")]
        [EmailAddress(ErrorMessage = "电子邮件格式输入有误")]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required(ErrorMessage = "手机号码不能为空")]
        [Phone(ErrorMessage = "手机号码格式错误")]
        [Display(Name = "手机号码")]
        public string Number { get; set; }
    }
}

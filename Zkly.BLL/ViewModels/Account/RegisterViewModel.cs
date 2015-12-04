using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zkly.DAL.Models;

namespace Zkly.BLL.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^(?=.*[0-9]+)(?=.*[a-z]+).{6,16}$", ErrorMessage = "用户名长度是6到16位,必须有数字和小写字母")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "手机号码格式错误。")]
        [Display(Name = "手机号码")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "选择角色")]
        public EUserRole Role { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[0-9]+)(?=.*[a-z]+).{6,16}$", ErrorMessage = "密码长度6到16位,必须有数字和小写字母")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public DataDictionary DataDictionaries { get; set; }
    }
}

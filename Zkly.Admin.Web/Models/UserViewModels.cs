using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Models
{
    public class UserViewModels
    {
        public List<ApplicationUser> List { get; set; }

        public ApplicationUser User { get; set; }
    }

    public class UpdateEmailViewModels
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "手机号码格式错误。")]
        [Display(Name = "电话号码")]
        
        public string PhoneNumber { get; set; }

        [Display(Name = "用户名")]
        public string UserName { get; set; }
    }

    public class ChangePwdViewModel
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }

        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}
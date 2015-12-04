using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Models
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
     
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用 户 名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "登陆密码")]
        public string Password { get; set; }

        [Display(Name = "自动登陆")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"[\w\-_]+", ErrorMessage = "用户名格式错误，用户名只能包含大小写字母，数字，英文中划线和下划线。")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "手机号码格式错误。")]
        [Display(Name = "手机号码")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "角色")]
        public EUserRole Role { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}

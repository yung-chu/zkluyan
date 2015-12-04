using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.ViewModels
{
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
}

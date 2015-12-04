using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.BLL.ViewModels
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "输入手机号码有误")]
        [Display(Name = "手机号码")]
        public string Number { get; set; }
    }
}

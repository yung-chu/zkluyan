using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public enum EUserRole
    {
        [Display(Name = "企业用户")]
        Enterprise,
        [Display(Name = "投资人")]
        Investor,
        [Display(Name = "管理员")]
        Administrator
    }
}
